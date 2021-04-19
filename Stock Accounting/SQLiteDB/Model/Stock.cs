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
    public class Stock : _DefaultModel, INotifyPropertyChanged
    {
        public static string[] OrderType = { "現股", "融資", "融券" };
        private string _stockID;
        private string _stockName;
        private List<int> _orderIDs;
        private string _accountName;
        private double _price;
        private int _count;
        private int _cost;
        private int _borrow;
        private int _type;
        private bool _isSelected;
        private bool _isCanSelect;
        private int _saleCount;

        public static new string TABLE_NAME = "stock";

        public event PropertyChangedEventHandler PropertyChanged;

        public string StockID { get { return _stockID; } set { _stockID = value; OnPropertyChanged(); } }

        public string StockName { get { return _stockName; } set { _stockName = value; OnPropertyChanged(); } }

        public List<int> OrderIDs { get { return _orderIDs; } set { _orderIDs = value; OnPropertyChanged(); } }

        public string AccountName { get { return _accountName; } set { _accountName = value; OnPropertyChanged(); } }

        public double Price { get { return _price; } set { _price = value; OnPropertyChanged(); } }

        public int Count { get { return _count; } set { _count = value; OnPropertyChanged(); } }

        public int Cost { get { return _cost; } set { _cost = value; OnPropertyChanged(); } }

        public int Borrow { get { return _borrow; } set { _borrow = value; OnPropertyChanged(); } }

        public int Type { get { return _type; } set { _type = value; OnPropertyChanged(); } }

        public int Value { get { return CalculateValue();  } }

        public string Type_Value { get { return OrderType[Type]; } }

        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; OnPropertyChanged(); } }

        public bool IsCanSelect { get { return _isCanSelect; } set { _isCanSelect = value; OnPropertyChanged(); } }

        public int SaleCount { get { return _saleCount; } set { _saleCount = value; OnPropertyChanged(); } }

        public override String TableName() => TABLE_NAME;

        public Stock(string id, int orderID)
        {
            ID = -1;
            StockID = id;
            IsCanSelect = true;
            OrderIDs = new List<int>(orderID);
        }

        public Stock(Order order)
        {
            ID = -1;
            StockID = order.StockID;
            StockName = order.StockName;
            OrderIDs = new List<int>();
            OrderIDs.Add(order.ID);
            AccountName = order.AccountName;
            Price = order.Price;
            Count = order.Count;
            Cost = order.Cost;
            Borrow = order.Borrow;
            Type = (order.Type == 3) ? 0 : order.Type;
            IsCanSelect = true;
        }

        public Stock(SQLiteDataReader reader)
        {
            ID = Int32.Parse(reader["id"].ToString());
            StockID = reader["stock_id"].ToString();
            StockName = reader["stock_name"].ToString();
            var list = new List<int>();
            foreach (string str in reader["orderIDs"].ToString().Split(','))
            {
                list.Add(Int32.Parse(str));
            }
            Console.WriteLine(list.ToString());
            OrderIDs = list;
            AccountName = reader["account_name"].ToString();
            Price = double.Parse(reader["price"].ToString());
            Count = Int32.Parse(reader["count"].ToString());
            Cost = Int32.Parse(reader["cost"].ToString());
            Borrow = Int32.Parse(reader["borrow"].ToString());
            Type = Int32.Parse(reader["type"].ToString());
            IsCanSelect = true;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, stock_id TEXT, stock_name TEXT, orderIDs TEXT, account_name TEXT, price REAL, count INTEGER, cost INTEGER, borrow INTEGER, type INTEGER)";
        }

        public override string InsertOrUpdateValue()
        {
            string _ID = (ID >= 0) ? ID.ToString() : "NULL";
            string _OrderID = "";
            foreach(int i in OrderIDs)
            {
                _OrderID = _OrderID + ((_OrderID == "") ? "" : ",") + i;
            }

            return "INSERT OR IGNORE INTO " + TABLE_NAME + " VALUES (" + _ID + ", '" + StockID + "','" + StockName + "','" + _OrderID + "','" + AccountName + "','" + Price + "','" + Count + "','" + Cost + "','" + Borrow + "','" + Type + "');" +
                "UPDATE " + TABLE_NAME + " SET stock_id = '" + StockID + "', stock_name  = '" + StockName + "', orderIDs  = '" + _OrderID + "',account_name = '" + AccountName + "', price  = '" + Price + "', count  = '" + Count + "', cost  = '" + Cost + "', borrow  = '" + Borrow + "', type  = '" + Type + "' WHERE id = " + _ID;
        }

        private int CalculateValue()
        {
            return 0;
        }

        public int GetSaleCost()
        {
            return (Count != SaleCount) ? (int)Math.Ceiling(Cost / (double)(Count * SaleCount / 100)) * 100 : Cost;
        }

        public int GetSaleBorrow()
        {
            return (Count != SaleCount) ? (int)Math.Ceiling(Borrow / (double)(Count * SaleCount / 100)) * 100 : Borrow;
        }

        public int GetSaleBorrowInterest(DateTime date)
        {
            int borrow = GetSaleBorrow();
            Order buyin = ((List<Order>)DBManager.share.GetAllListFromTable(Order.TABLE_NAME, typeof(Order))).Find(x => x.ID == OrderIDs.First());
            return (int)(borrow * 0.05 * (new TimeSpan(date.Ticks - buyin.Date.Ticks).TotalDays + 2) / 365 );
        }
    }
}

