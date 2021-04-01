using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MySQLiteDB.Model
{
    public class _DefaultModel
    {
        public static String TABLE_NAME = "default";

        public int ID { get; set; }

        public virtual String TableName() => TABLE_NAME;

        public virtual String CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT)";
        }

        public virtual String InsertValue()
        {
            return "INSERT INTO " + TABLE_NAME + " VALUES (null);";
        }

        public virtual void SetValue(SQLiteDataReader reader)
        {
            this.ID = Int32.Parse(reader["id"].ToString());
        }
    }
}
