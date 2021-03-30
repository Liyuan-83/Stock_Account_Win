using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;

namespace MySQLiteDB.Model
{
    class Account : _DefaultModel
    {
        public static new string TABLE_NAME = "account";

        public string Name { get; set; }
        public int Assets { get; set; }

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, assets INTEGER)";
        }

        public override string InsertValue()
        {
            return "INSERT INTO " + TABLE_NAME + " VALUES (null, '" + Name + "','" + Assets + "');";
        }

        public override void SetValue(SQLiteDataReader reader)
        {
            base.SetValue(reader);
            this.Name = reader["name"].ToString();
            this.Assets = Int32.Parse(reader["assets"].ToString());
        }
    }
}
