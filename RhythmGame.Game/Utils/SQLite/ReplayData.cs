using RhythmGame.Utils.SQLite;
using System.Data.SQLite;

#pragma warning disable IDE1006 // Naming Styles
namespace RhythmGame.Utils.SQLite
{
    public class ReplayData : SQLiteData
    {
        public string map;
        public string replaydata;

        public ReplayData() : base() { }

        public ReplayData(SQLiteDataReader data) : base(data) { }

        public ReplayData(string map_, string replaydata_)
        {
            map = map_;
            replaydata = replaydata_;
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
