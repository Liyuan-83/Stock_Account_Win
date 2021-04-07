using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MySQLiteDB.Model
{
    public abstract class _DefaultModel
    {
        public static String TABLE_NAME = "default";

        public int ID { get; set; }

        public abstract String TableName();

        public abstract String CreateTable();

        public abstract String InsertOrUpdateValue();

        public abstract void GetValue(SQLiteDataReader reader);
    }
}
