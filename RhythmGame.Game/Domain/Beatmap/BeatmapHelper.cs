using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using osu.Framework.Logging;
using RhythmGame.Utils;
using System.Linq;
using System.Reflection;

namespace RhythmGame.Domain.Beatmap
{
    static class BeatmapHelper
    {

        static public List<Beatmap> Beatmaps;

        /// <summary>
        /// Load a single Beatmap.
        /// </summary>
        /// <param name="path">The path to the beatmap folder.</param>
        /// <param name="fileName">The fileName of the .psc file.</param>
        static public Beatmap Load(string path, string fileName)
        {
            Beatmap parsed = new Beatmap();
            parsed.path = path;
            parsed.filename = fileName;

            string state = "";

            IEnumerable<string> lines = File.ReadLines($"{path}/{fileName}");

            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    // Information like Metadata are in the form 'Type: Data'
                    Queue<string> lineParts = new Queue<string>(line.Split(':'));

                    // If we recognize this form, it's an attribute.
                    // Otherwise, it is a more complex data form, like an event
                    if (lineParts.Count > 1)
                    {
                        string type = lineParts.Dequeue();
                        string rightPart = string.Concat(lineParts.ToArray()).Trim();

                        // Use reflection to set the Beatmap's attributes
                        try
                        {
                            switch (type)
                            {
                                case "Events":
                                case "TimingPoints":
                                case "SpeedVariations":
                                case "Notes":
                                    state = type;
                                    continue;
                                default:
                                    break;
                            }

                            PropertyInfo prop = parsed.GetType().GetProperty(type.ToLower());

                            if(prop == null)
                            {
                                prop = parsed.GetType().GetProperty("_" + type.ToLower());
                            }

                            if(prop == null)
                            {
                                throw new Exception();
                            }

                            switch (type)
                            {
                                case "Format":
                                case "Title":
                                case "Artist":
                                case "Creator":
                                case "Version":
                                case "Audio":
                                case "Background":
                                    prop.SetValue(parsed, rightPart);
                                    break;
                                case "PreviewTime":
                                case "KeyCount":
                                    prop.SetValue(parsed, int.Parse(rightPart));
                                    break;
                                case "Difficulty":
                                    prop.SetValue(parsed, double.Parse(rightPart, CultureInfo.InvariantCulture));
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        catch
                        {
                            Logger.Log($"Unknown beatmap field : {type}", level: LogLevel.Error);
                        }
                    }
                    else
                    {
                        // Each event is comma separated and can have any amount of values.
                        var eventParts = line.Split(',');

                        // Handling depends on the data type (or the current reading state)
                        switch (state)
                        {
                            case "Events":
                                break;
                            case "TimingPoints":
                                break;
                            case "SpeedVariations":
                                break;
                            case "Notes":
                                try
                                {
                                    parsed._notes.Add(new Note(Convert.ToInt32(eventParts[0]), Convert.ToInt32(eventParts[1])));
                                }
                                catch
                                {
                                    Logger.Log($"Invalid note : {line}", level: LogLevel.Error);
                                }
                                break;
                            default:
                                Logger.Log($"Unknown state : {state}", level: LogLevel.Error);
                                break;

                        }
                    }
                }
            }

            // If no difficulty has been provided in the game file, process it now.
            if (parsed.difficulty == 0)
                parsed.difficulty = 1; // no diffcalc atm
                //parsed.difficulty = DifficultyCalculation.GetDifficulty(parsed);

            parsed._loaded = true;

            return parsed;
        }

        /// <summary>
        /// Save a single Beatmap as a .psc.
        /// </summary>
        /// <param name="beatmap">The Beatmap to be saved.</param>
        /// <param name="file_path">The filepath this Beatmap Should be saved to.</param>
        static public void Save(Beatmap beatmap, string file_path)
        {
            using (StreamWriter file = new StreamWriter(file_path))
            {
                // Write Game data/Metadata
                WriteProperty(file, beatmap, "Format");
                WriteProperty(file, beatmap, "Title");
                WriteProperty(file, beatmap, "Artist");
                WriteProperty(file, beatmap, "Creator");
                WriteProperty(file, beatmap, "Version");
                WriteProperty(file, beatmap, "Audio");
                WriteProperty(file, beatmap, "PreviewTime");
                WriteProperty(file, beatmap, "Background");

                file.WriteLine("");
                WriteProperty(file, beatmap, "KeyCount");
                WriteProperty(file, beatmap, "Difficulty");

                file.WriteLine("");

                // Write Events
                file.WriteLine("");
                file.WriteLine("Events:");

                /*foreach (Event evt in beatmap.Events)
                {
                    file.WriteLine(evt.ToString());
                }*/

                // Write Timing Points
                file.WriteLine("");
                file.WriteLine("TimingPoints:");

                /*foreach (TimingPoint timingPoint in beatmap.TimingPoints)
                {
                    file.WriteLine(timingPoint.ToString());
                }*/

                // Write Speed Variations
                file.WriteLine("");
                file.WriteLine("SpeedVariations:");

                // Write Arcs
                file.WriteLine("");
                file.WriteLine("Notes:");

                foreach (Note note in beatmap._notes)
                {
                    file.WriteLine(note.ToString());
                }
            }
        }

        public static void SaveAsZip(Beatmap beatmap)
        {
            using (FileStream output = File.Create($"Songs/{beatmap}.rgm"))
            using (ZipOutputStream zipStream = new ZipOutputStream(output))
            {
                zipStream.SetLevel(3);

                int folderOffset = beatmap.path.Length
                    + ((beatmap.path.EndsWith("/") || beatmap.path.EndsWith("\\")) ? 0 : 1);

                // Compress the folder
                CompressFolder(beatmap.path, zipStream, folderOffset);
            }

            void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
            {
                string[] fileNames = Directory.GetFiles(beatmap.path);

                foreach (string fileName in fileNames)
                {
                    FileInfo info = new FileInfo(fileName);

                    string entryName = fileName.Substring(folderOffset);
                    entryName = ZipEntry.CleanName(entryName);

                    ZipEntry entry = new ZipEntry(entryName)
                    {
                        DateTime = info.LastWriteTime,
                        Size = info.Length,
                    };

                    zipStream.PutNextEntry(entry);

                    byte[] buffer = new byte[4096];
                    using (FileStream input = File.OpenRead(fileName))
                    {
                        StreamUtils.Copy(input, zipStream, buffer);
                    }

                    zipStream.CloseEntry();
                }

                // Compress subfolders
                string[] folders = Directory.GetDirectories(beatmap.path);
                foreach (string folder in folders)
                {
                    CompressFolder(folder, zipStream, folderOffset);
                }
            }
        }

        /// <summary>
        /// Write a property to file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="beatmap"></param>
        /// <param name="property"></param>
        static private void WriteProperty(StreamWriter file, Beatmap beatmap, string property)
        {
            if(beatmap.GetType().GetProperty(property.ToLower()) != null)
            {
                file.WriteLine(property + ": " + beatmap.GetType().GetProperty(property.ToLower()).GetValue(beatmap, null));
            } else if (beatmap.GetType().GetProperty("_"+property.ToLower()) != null)
            {
                file.WriteLine(property + ": " + beatmap.GetType().GetProperty("_" + property.ToLower()).GetValue(beatmap, null));
            } else
            {
                Logger.Log($"Trying to write invalid property {property}", level: LogLevel.Error);
            }
        }

        /// <summary>
        /// Rescan the Beatmap data
        /// </summary>
        static public void RescanBeatmaps()
        {
            Beatmaps = new List<Beatmap>();

            DataManager.BeatmapDB.ClearBeatmaps();

            try
            {
                string[] directories = Directory.GetDirectories("Songs/");

                for (int i = 0; i < directories.Length; i++)
                {
                    string[] files = Directory.GetFiles(directories[i], "*.rgb");

                    for (int j = 0; j < files.Length; j++)
                    {
                        Beatmap debugData = Load(directories[i], Path.GetFileName(files[j]));
                        DataManager.BeatmapDB.AddBeatmap(debugData);
                        Beatmaps.Add(debugData);
                    }
                }
            }
            catch {
                Directory.CreateDirectory("Songs/");
                return;
            }
        }

        /// <summary>
        /// Sort the beatmaps by the provided metadata string.
        /// </summary>
        /// <param name="beatmaps">The list of beatmaps to sort.</param>
        /// <param name="sort">The way to sort. "Difficulty," "Artist", "Title", "Mapper", or "Version"</param>
        /// <param name="ascending">Whether the list should be sorted Ascending (A->Z,1->9), or Descending (Z->A,9->1)</param>
        /// <returns></returns>
        static public List<Beatmap> SortBeatmaps(List<Beatmap> beatmaps, string sort, bool ascending = true)
        {
            switch (sort)
            {
                case "difficulty":
                    return ascending
                        ? beatmaps.OrderBy(i => i.difficulty).ToList()
                        : beatmaps.OrderByDescending(i => i.difficulty).ToList();
                case "artist":
                    return ascending
                        ? beatmaps.OrderBy(i => i.artist).ToList()
                        : beatmaps.OrderByDescending(i => i.artist).ToList();
                case "title":
                    return ascending
                        ? beatmaps.OrderBy(i => i.title).ToList()
                        : beatmaps.OrderByDescending(i => i.title).ToList();
                case "creator":
                    return ascending
                        ? beatmaps.OrderBy(i => i.creator).ToList()
                        : beatmaps.OrderByDescending(i => i.creator).ToList();
                case "version":
                    return ascending
                        ? beatmaps.OrderBy(i => i.version).ToList()
                        : beatmaps.OrderByDescending(i => i.version).ToList();
                default:
                    return beatmaps;
            }
        }
    }
}
