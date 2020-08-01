using osu.Framework.Logging;
using RhythmGame.Utils.SQLite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

#pragma warning disable IDE1006 // Naming Styles
namespace RhythmGame.Domain.Beatmap
{
    public class Beatmap : SQLiteData
    {

        // Stored values
        public string artist { get; set; }
        public string title { get; set; }
        public string creator { get; set; }
        public string version { get; set; }
        public double difficulty { get; set; }

        public int keycount { get; set; }

        public string path { get; set; }
        public string filename { get; set; }

        public string background { get; set; }
        public string audio { get; set; }
        public int previewtime { get; set; }

        // in-memory only
        public List<Note> _notes { get; set; }
        public bool _loaded { get; set; }

        public string _format { get; set; }

        // init memory part
        private void init()
        {
            _loaded = false;
            _notes = new List<Note>();
        }

        public Beatmap()
        {
            init();
        }

        public Beatmap(SQLiteDataReader data) : base(data)
        {
            init();
        }

        public List<Note> GetColumnNotes(int col) => _notes.FindAll(n => n.Col == col); 

        public bool FoundByString(string search)
        {
            int right = 0;
            string[] searchWords = search.ToLower().Split(" ");

            foreach (string s in searchWords)
            {
                foreach (string c in title.ToLower().Split(" "))
                {
                    if (c.StartsWith(s)) right++;
                }
                foreach (string c in artist.ToLower().Split(" "))
                {
                    if (c.StartsWith(s)) right++;
                }
                foreach (string c in creator.ToLower().Split(" "))
                {
                    if (c.StartsWith(s)) right++;
                }
                foreach (string c in version.ToLower().Split(" "))
                {
                    if (c.StartsWith(s)) right++;
                }
            }

            if (right / (double)searchWords.Length > 0.5) return true;

            Logger.Log($"{search} not found in {ToString()} ({right / (double)searchWords.Length})");
            return false;
        }

        public override string ToString()
        {
            return $"{artist} - {title} [{version}] ({creator})";
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
