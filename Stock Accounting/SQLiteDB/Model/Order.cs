using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace MySQLiteDB.Model
{
    public class Order : _DefaultModel
    {
        public static new string TABLE_NAME = "order";

        public string AccountName { get; set; }

        public string StockID { get; set; }

        public string StockName { get; set; }

        public DateTime Date { get; set; }

        public bool IsBuy { get; set; }
        
        public int Price { get; set; }

        public int Count { get; set; }

        public int Fee { get; set; }

        public int Tax { get; set; }

        public int Type { get; set; }

        public string Mark { get; set; }

        public override String TableName() => TABLE_NAME;

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, account_name TEXT, stock_id TEXT, stock_name TEXT, date TEXT, is_buy INTERGER, price INTERGER, count INTEGER, fee INTEGER, tax INTEGER, type INTEGER, mark TEXT)";
        }

        public override string InsertOrUpdateValue()
        {
            return "INSERT INTO " + TABLE_NAME + " VALUES (null, '" + AccountName + "','" + StockID + "','" + StockName + "','" + Date.ToShortDateString() + "','" + IsBuy + "','" + Price + "','" + Count + "','" + Fee + "','" + Tax + "','" + Type + "','" + Mark + "');";
        }

        public override void GetValue(SQLiteDataReader reader)
        {
            ID = Int32.Parse(reader["id"].ToString());
            AccountName = reader["account_name"].ToString();
            StockID = reader["stock_id"].ToString();
            StockName = reader["stock_name"].ToString();
            Date = DateTime.Parse(reader["date"].ToString());
            IsBuy = Boolean.Parse(reader["is_buy"].ToString());
            Price = Int32.Parse(reader["price"].ToString());
            Count = Int32.Parse(reader["count"].ToString());
            Fee = Int32.Parse(reader["fee"].ToString());
            Tax = Int32.Parse(reader["tax"].ToString());
            Type = Int32.Parse(reader["type"].ToString());
            Mark = reader["mark"].ToString();
        }
    }
}
