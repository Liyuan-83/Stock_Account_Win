using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;

namespace MySQLiteDB.Model
{
    public abstract class _DefaultModel
    {
        private int _id;

        public static String TABLE_NAME = "default";

        public int ID { get { return _id; } set { _id = value; } }

        public abstract String TableName();

        public abstract String CreateTable();

        public abstract String InsertOrUpdateValue();
    }
}
