using RhythmGame.Utils.SQLite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

#pragma warning disable IDE1006 // Naming Styles
namespace RhythmGame.Domain.Score
{
    public class Score : SQLiteData
    {
        // Play Data
        public string map { get; set; }
        public string datet { get; set; }
        public string player { get; set; }

        // Player modifiers
        public double rate { get; set; }
        public int mods { get; set; }

        // Play Results
        public int score { get; set; }
        public double accuracy { get; set; }
        public int maxCombo { get; set; }
        public string grade { get; set; }

        // Judgements
        public int max { get; set; }
        public int perfect { get; set; }
        public int great { get; set; }
        public int good { get; set; }
        public int bad { get; set; }
        public int miss { get; set; }

        public Score()
        {
        }

        public Score(SQLiteDataReader data) : base(data) { }
    }
}
#pragma warning restore IDE1006 // Naming Styles

