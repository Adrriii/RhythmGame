using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RhythmGame.Domain.Beatmap.Converters
{
    class Mania2RG : IBeatmapConverter
    {
        // Estimated offset difference between osu!mania and RG
        private const int msOffset = 0;

        /// <summary>
        /// Convert an osu!mania beatmap to a RG beatmap
        /// </summary>
        /// <param name="folder_path">The path to the osu!mania map folder.</param>
        /// <returns>A list containing all the difficulties as seperate Beatmaps.</returns>
        public List<Beatmap> Convert(string folder_path)
        {
            List<Beatmap> results = new List<Beatmap>();

            // See if the provided folder exists
            if (Directory.Exists(folder_path))
            {
                // Look for .osu files, there should be one for each difficulty
                foreach (string file in Directory.GetFiles(folder_path, "*.osu"))
                {
                    Beatmap result = new Beatmap();
                    ManiaBeatmap maniaBeatmap = new ManiaBeatmap(file);

                    // Fill in metadata
                    result._format = "1";
                    result.creator = maniaBeatmap.Creator;
                    result.artist = maniaBeatmap.Artist;
                    result.title = maniaBeatmap.Title;
                    result.version = maniaBeatmap.Version;
                    result.audio = maniaBeatmap.AudioFilename;
                    result.previewtime = maniaBeatmap.PreviewTime;
                    result.keycount = maniaBeatmap.CircleSize;

                    if (maniaBeatmap.Events.Count > 0)
                    {
                        // Remove the "0,0,"" and "",0,0" on the background line.
                        string backgroundName = maniaBeatmap.Events[0];
                        string[] charsToRemove = new string[] { ",", "\"", "0" };

                        foreach (string c in charsToRemove)
                        {
                            backgroundName = backgroundName.Replace(c, "");
                        }

                        result.background = backgroundName;
                    }

                    // Look at each HitObject, and assign the appropriate bit to it.
                    foreach (string str in maniaBeatmap.HitObjects)
                    {
                        string[] parts = str.Split(',');
                        int col = 0;

                        switch (parts[0])
                        {
                            case "64":
                                col = 1;
                                break;
                            case "192":
                                col = 2;
                                break;
                            case "320":
                                col = 3;
                                break;
                            case "448":
                                col = 4;
                                break;
                        }

                        int time = Int32.Parse(parts[2]) + msOffset;
                        result._notes.Add(new Note(time, col));
                    }
                    results.Add(result);
                }
            }

            return results;
        }

        /// <summary>
        /// Convert a folder of osu!mania beatmaps to compatible beatmaps, and then save the converted Beatmaps to storage.
        /// </summary>
        /// <param name="folder_path">The path to the maps-to-be-converted folder</param>
        public void Save(string folder_path)
        {
            foreach (Beatmap map in Convert(folder_path))
            {
                if (map.audio != null)
                {
                    string audioPath = $"{folder_path}/{map.audio}";

                    if (File.Exists(audioPath))
                    {
                        int id = 0;
                        // The folder name will look like "0 - Artist - SongTitle - (Mapper)"
                        string folderName = string.Join("_", ($"{id} - {map.artist} - {map.title} ({map.creator})").Split(Path.GetInvalidFileNameChars()));
                        string dirName = $"Songs/{folderName}";

                        if (!Directory.Exists(dirName))
                            Directory.CreateDirectory(dirName);

                        // Copy Audio File
                        File.Copy(audioPath, $"{dirName}/{map.audio}", true);

                        // Copy Background Image
                        string backgroundPath = $"{folder_path}/{map.background}";

                        if (File.Exists(backgroundPath))
                        {
                            File.Copy(backgroundPath, $"{dirName}/{map.background}", true);
                        }
                        else
                        {
                            map.background = "";
                        }

                        // The file name will look like "Artist - SongTitle [Converted] (Mapper).rgb"
                        string difficultyFileName = string.Join("_", ($"{map.artist} - {map.title} [{map.version}] ({map.creator})").Split(Path.GetInvalidFileNameChars()));

                        BeatmapHelper.Save(map, $"{dirName}/{difficultyFileName}.rgb");
                    }
                }
            }
        }
    }
}
