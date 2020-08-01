using osu.Framework.Logging;
using System.Data.SQLite;
using System.Reflection;

namespace RhythmGame.Utils.SQLite
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Database compatibility")]
    public abstract class SQLiteData
    {
        public SQLiteData() { }

        public SQLiteData(SQLiteDataReader data)
        {
            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name.ToCharArray()[0] == '_')
                {
                    continue;
                }
                try
                { 
                    switch(prop.Name.ToLower())
                    {
                        case "datet":
                            prop.SetValue(this, data[prop.Name.ToLower()].ToString());
                            break;
                        default:
                            prop.SetValue(this, data[prop.Name.ToLower()]);
                            break;
                    }
                }
                catch
                {
                    try
                    {
                        Logger.Log($"Data field in SQLite request could not be set as a {prop.PropertyType.Name} : {data[prop.Name.ToLower()]}", LoggingTarget.Database, LogLevel.Error);
                    }
                    catch
                    {
                        Logger.Log($"Unexpected data field in SQLite request : {prop.Name}", LoggingTarget.Database, LogLevel.Error);
                    }
                }
            }
        }

        public void SaveData(SQLiteStore db)
        {
            string r = $"INSERT INTO {GetType().Name.ToLower()} (";
            string vals = "";
            bool f = true;

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.Name.ToCharArray()[0] == '_')
                {
                    continue;
                }

                r += f ? "" : ",";
                vals += f ? "" : ",";
                f = false;

                r += prop.Name;
                if (prop.PropertyType.Name.ToLower() == "string" || prop.PropertyType.Name.ToLower() == "char")
                {
                    vals += "'" + prop.GetValue(this).ToString().Replace("'" , "\'" + "'") + "'";
                }
                else
                {
                    vals += prop.GetValue(this);
                }
            }

            r += $") VALUES ({vals})";
            db.Exec(r);
        }
    }
}
