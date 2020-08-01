using System;
using System.Collections.Generic;
using System.Text;

namespace RhythmGame.Domain.Beatmap
{
    interface IBeatmapConverter
    {
        /// <summary>
        /// Convert a folder of some other game beatmaps to beatmaps
        /// </summary>
        /// <param name="folder_path">The path to the maps-to-be-converted folder</param>
        /// <returns>A list containing converted beatmaps found from the folder.</returns>
        List<Beatmap> Convert(string folder_path);

        /// <summary>
        /// Convert a folder of some other game beatmaps to compatible beatmaps, and then save the converted beatmaps to storage.
        /// </summary>
        /// <param name="folder_path">The path to the maps-to-be-converted folder</param>
        void Save(string folder_path);
    }
}
