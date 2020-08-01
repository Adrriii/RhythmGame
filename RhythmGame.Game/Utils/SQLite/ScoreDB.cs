using RhythmGame.Domain.Score;
using RhythmGame.Utils.SQLite;
using System.Collections.Generic;
using System.Data.SQLite;

namespace RhythmGame.Utils.SQLite
{
    public class ScoreDB : SQLiteStore
    {
        public ScoreDB() : base("scores") { }

        public override void InitTables()
        {
            Tables.Add(new Score());
            //Tables.Add(new ReplayData());
        }

        public void AddScore(Score score)
        {
            score.SaveData(this);
        }

        /*public void AddReplay(ReplayData replay)
        {
            replay.SaveData(this);
        }*/

        public List<Score> GetScores(string map)
        {
            List<Score> scores = new List<Score>();

            SQLiteDataReader r = Query($"SELECT * FROM scoredata WHERE map = '{map}' ORDER BY score DESC, datet ASC");

            // r can be null due to the current issues with ScoreDB
            while (r != null && r.Read())
            {
                scores.Add(new Score(r));
            }

            //foreach (ScoreData score in scores)
                //PulsarcLogger.Debug(score.ToString(), LogType.Runtime);

            return scores;
        }
    }
}
