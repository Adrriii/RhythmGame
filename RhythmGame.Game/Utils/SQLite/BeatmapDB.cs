using osu.Framework.Logging;
using RhythmGame.Domain.Beatmap;
using System.Collections.Generic;
using System.Data.SQLite;

namespace RhythmGame.Utils.SQLite
{
    public class BeatmapDB : SQLiteStore
    {
        public BeatmapDB() : base("beatmap") { }

        public override void InitTables()
        {
            Tables.Add(new Beatmap());
        }

        public void AddBeatmap(Beatmap map)
        {
            map.SaveData(this);
        }

        public void ClearBeatmaps()
        {
            Exec("DELETE FROM beatmap");
        }

        public List<Beatmap> GetBeatmaps()
        {
            List<Beatmap> maps = new List<Beatmap>();

            SQLiteDataReader r = Query("SELECT * FROM beatmap");

            while(r.Read())
            {
                maps.Add(new Beatmap(r));
            }

            return maps;
        }
    }
}
