using Stock_Accounting.APIModel;
using Stock_Accounting.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MySQLiteDB.Model
{
    class StockClosingInfo : _DefaultModel, INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private DateTime _date;
        private long _totalDealNo;              //成交股數
        private long _turnover;                 //成交金額
        private double _openingPrice;           //開盤價
        private double _maxPrice;               //最高價
        private double _minPrice;               //最低價
        private double _closingPrice;           //收盤價
        private double _spread;                 //漲跌價差
        private long _totalTransactionsNo;      //成交筆數

        public static new string TABLE_NAME = "StockClosingInfo";
        private Task<APIModel_StockClosingInfo> task;

        public event PropertyChangedEventHandler PropertyChanged;

        public new string ID { get { return _id; } set { _id = value; OnPropertyChanged(); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }
        public DateTime Date { get { return _date; } set { _date = value; OnPropertyChanged(); } }
        public long TotalDealNo { get { return _totalDealNo; } set { _totalDealNo = value; OnPropertyChanged(); } }
        public long Turnover { get { return _turnover; } set { _turnover = value; OnPropertyChanged(); } }
        public double OpeningPrice { get { return _openingPrice; } set { _openingPrice = value; OnPropertyChanged(); } }
        public double MaxPrice { get { return _maxPrice; } set { _maxPrice = value; OnPropertyChanged(); } }
        public double MinPrice { get { return _minPrice; } set { _minPrice = value; OnPropertyChanged(); } }
        public double ClosingPrice { get { return _closingPrice; } set { _closingPrice = value; OnPropertyChanged(); } }
        public double Spread { get { return _spread; } set { _spread = value; OnPropertyChanged(); } }
        public long TotalTransactionsNo { get { return _totalTransactionsNo; } set { _totalTransactionsNo = value; OnPropertyChanged(); } }

        public override String TableName() => TABLE_NAME;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public StockClosingInfo(string id, string name, APIModel_StockClosingInfo info)
        {
            ID = id;
            Name = name;
            var twCulture = new CultureInfo("zh-TW", true);
            twCulture.DateTimeFormat.Calendar = new TaiwanCalendar();
            string str = info.data.Last()[0];
            str = str.PadLeft(8, '0');
            Date = DateTime.ParseExact(str, "y/MM/dd", twCulture);
            TotalDealNo = Int64.Parse(info.data.Last()[1].Replace(",",""));
            Turnover = Int64.Parse(info.data.Last()[2].Replace(",", ""));
            OpeningPrice = double.Parse(info.data.Last()[3]);
            MaxPrice = double.Parse(info.data.Last()[4]);
            MinPrice = double.Parse(info.data.Last()[5]);
            ClosingPrice = double.Parse(info.data.Last()[6]);
            Spread = double.Parse(info.data.Last()[7].Replace("+", ""));
            TotalTransactionsNo = Int64.Parse(info.data.Last()[8].Replace(",", ""));
        }

        public StockClosingInfo(SQLiteDataReader reader)
        {
            ID = reader["id"].ToString();
            Name = reader["name"].ToString();
            Date = DateTime.Parse(reader["date"].ToString());
            TotalDealNo = Int64.Parse(reader["total_deal_num"].ToString());
            Turnover = Int64.Parse(reader["turnover"].ToString());
            OpeningPrice = double.Parse(reader["opening_price"].ToString());
            MaxPrice = double.Parse(reader["max_price"].ToString());
            MinPrice = double.Parse(reader["min_price"].ToString());
            ClosingPrice = double.Parse(reader["closing_price"].ToString());
            Spread = double.Parse(reader["spread"].ToString());
            TotalTransactionsNo = Int64.Parse(reader["total_transactions_num"].ToString());
        }

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id TEXT PRIMARY KEY, name TEXT, date TEXT, total_deal_num INTEGER, turnover INTEGER, opening_price REAL, max_price REAL, min_price REAL, closing_price REAL, spread REAL, total_transactions_num REAL)";
        }

        public override string InsertOrUpdateValue()
        {
            return "INSERT OR IGNORE INTO " + TABLE_NAME + " VALUES (" + ID + ", '" + Name + "','" + Date.ToString("yyyy/MM/dd") + "'," + TotalDealNo + "," + Turnover + "," + OpeningPrice + "," + MaxPrice + "," + MinPrice + "," + ClosingPrice + "," + Spread + "," + TotalTransactionsNo + ");" +
                "UPDATE " + TABLE_NAME + " SET name = '" + Name + "', date  = '" + Date.ToString("yyyy/MM/dd") + "', total_deal_num  = " + TotalDealNo + ",turnover = " + Turnover + ", opening_price  = " + OpeningPrice + ", max_price  = " + MaxPrice + ", min_price  = " + MinPrice + ", closing_price  = " + ClosingPrice + ", spread  = " + Spread + ", total_transactions_num  = " + TotalTransactionsNo + " WHERE id = " + ID;
        }
    }
}
