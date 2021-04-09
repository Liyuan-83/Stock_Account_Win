using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MySQLiteDB.Model
{
    public class Order : _DefaultModel, INotifyPropertyChanged
    {
        public static string[] OrderType = { "現股", "融資", "融券", "零股" };
        private string _accountName;
        private string _stockID;
        private string _stockName;
        private string _date;
        private bool _isBuy;
        private double _price;
        private int _count;
        private int _fee;
        private int _tax;
        private int _type;
        private string _mark;

        public static new string TABLE_NAME = "order";

        public event PropertyChangedEventHandler PropertyChanged;

        public string AccountName { get { return _accountName; } set { _accountName = value; OnPropertyChanged(); } }

        public string StockID { get { return _stockID; } set { _stockID = value; OnPropertyChanged(); } }

        public string StockName { get { return _stockName; } set { _stockName = value; OnPropertyChanged(); } }

        public string Date { get { return _date; } set { _date = value; OnPropertyChanged(); } }

        public bool IsBuy { get { return _isBuy; } set { _isBuy = value; OnPropertyChanged(); } }

        public double Price { get { return _price; } set { _price = value; OnPropertyChanged(); } }

        public int Count { get { return _count; } set { _count = value; OnPropertyChanged(); } }

        public int Fee { get { return _fee; } set { _fee = value; OnPropertyChanged(); } }

        public int Tax { get { return _tax; } set { _tax = value; OnPropertyChanged(); } }

        public int Type { get { return _type; } set { _type = value; OnPropertyChanged(); } }

        public string Mark { get { return _mark; } set { _mark = value; OnPropertyChanged(); } }

        public override String TableName() => TABLE_NAME;

        public Order()
        {
            IsBuy = true;
        }

        public Order(SQLiteDataReader reader)
        {
            ID = Int32.Parse(reader["id"].ToString());
            AccountName = reader["account_name"].ToString();
            StockID = reader["stock_id"].ToString();
            StockName = reader["stock_name"].ToString();
            Date = reader["date"].ToString();
            IsBuy = Boolean.Parse(reader["is_buy"].ToString());
            Price = double.Parse(reader["price"].ToString());
            Count = Int32.Parse(reader["count"].ToString());
            Fee = Int32.Parse(reader["fee"].ToString());
            Tax = Int32.Parse(reader["tax"].ToString());
            Type = Int32.Parse(reader["type"].ToString());
            Mark = reader["mark"].ToString();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, account_name TEXT, stock_id TEXT, stock_name TEXT, date TEXT, is_buy INTERGER, price REAL, count INTEGER, fee INTEGER, tax INTEGER, type INTEGER, mark TEXT)";
        }

        public override string InsertOrUpdateValue()
        {
            return "INSERT INTO " + TABLE_NAME + " VALUES (null, '" + AccountName + "','" + StockID + "','" + StockName + "','" + Date + "','" + IsBuy + "','" + Price + "','" + Count + "','" + Fee + "','" + Tax + "','" + Type + "','" + Mark + "');";
        }
    }
}
