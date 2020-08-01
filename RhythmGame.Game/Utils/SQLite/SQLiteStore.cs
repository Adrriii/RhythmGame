using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace RhythmGame.Utils.SQLite
{
    public abstract class SQLiteStore
    {
        public static bool Logging = true;

        private SQLiteConnection db;
        private string filename;

        public List<SQLiteData> Tables { get; protected set; }

        public SQLiteStore(string name_)
        {
            filename = name_ + ".db";

            Tables = new List<SQLiteData>();

            InitTables();

            if (!File.Exists(filename))
            {
                SQLiteConnection.CreateFile(filename);
                Connect();
                InitDB();
            }
            else
            {
                Connect();
            }
        }

        public abstract void InitTables();

        public void Connect()
        {
            db = new SQLiteConnection($"Data Source={filename};Version=3;");
            db.Open();
        }

        public void Exec(string sql)
        {
            new SQLiteCommand(sql, db).ExecuteNonQuery();
        }

        public SQLiteDataReader Query(string sql)
        {
            return new SQLiteCommand(sql, db).ExecuteReader();
        }

        public List<object> QueryFirst(string sql)
        {
            List<object> res = new List<object>();
            SQLiteDataReader reader = Query(sql);

            reader.Read();
            res.Add(reader);
            
            return res;
        }

        public void Close()
        {
            db.Close();
            Tables.Clear();
        }

        public void InitDB()
        {
            foreach(SQLiteData data in Tables)
            {
                string r = $"CREATE TABLE {data.GetType().Name.ToLower()} (";

                bool first = true;
                foreach (PropertyInfo prop in data.GetType().GetProperties())
                {
                    if (prop.Name.ToCharArray()[0] == '_')
                    {
                        continue;
                    }
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        r += ",";
                    }

                    r += prop.Name.ToLower() + " " + prop.PropertyType.Name.ToString().ToLower();
                }

                r += ")";

                Exec(r);
            }
        }
    }
}
