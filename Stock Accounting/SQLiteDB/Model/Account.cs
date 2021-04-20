using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.ComponentModel;
using System.Collections;
using System.Runtime.CompilerServices;
using Stock_Accounting.Manager;
using System.Threading;

namespace MySQLiteDB.Model
{
    public class Account : _DefaultModel, INotifyPropertyChanged
    {
        private string _name;
        private int _firstCash;
        private double _fee;

        public static new string TABLE_NAME = "account";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(); } }

        public int FirstCash { get { return _firstCash; } set { _firstCash = value; OnPropertyChanged(); } }

        public double Fee { get { return _fee; } set { _fee = value; OnPropertyChanged(); } }

        public int Assets { get { return CalculateAssets(); } }

        public int Cash { get { return CalculateCash(); } }

        public int StockValue { get { return CalculateStockValue(); } }

        public int BenefitValue { get { return CalculateRealBenefit(); } }

        public override String TableName() => TABLE_NAME;

        public Account() { }
        public Account(SQLiteDataReader reader)
        {
            ID = Int32.Parse(reader["id"].ToString());
            Name = reader["name"].ToString();
            FirstCash = Int32.Parse(reader["first_cash"].ToString());
            Fee = Double.Parse(reader["fee"].ToString());
            Reload();
        }

        public void Reload()
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                OnPropertyChanged("Assets");
                OnPropertyChanged("Cash");
                OnPropertyChanged("StockValue");
                OnPropertyChanged("BenefitValue");
            });
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string CreateTable()
        {
            return @"CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, first_cash INTEGER, fee REAL)";
        }

        public override string InsertOrUpdateValue()
        {
            string _ID = (ID >= 0) ? ID.ToString() : "NULL";
            return "INSERT OR IGNORE INTO " + TABLE_NAME + " VALUES (" + _ID + ", '" + Name + "','" + FirstCash + "','" + Fee + "');" +
                "UPDATE " + TABLE_NAME + " SET name = '" + Name + "',first_cash = " + FirstCash + ",fee = " + Fee + " WHERE id = " + _ID;
        }

        private int CalculateAssets()
        {
            return CalculateCash() + CalculateStockValue();
        }

        private int CalculateCash()
        {
            List<Order> orderList = (List<Order>)DBManager.share.GetAllListFromTable(Order.TABLE_NAME, typeof(Order));
            int totalCost = 0;
            if (orderList != null && orderList.Count > 0)
            {
                var myOrders = orderList.FindAll(x => x.AccountName == Name);
                totalCost = myOrders.Sum(x => x.Cost);
            }
            return FirstCash + totalCost;
        }

        private int CalculateStockValue()
        {
            List<Stock> stockList = (List<Stock>)DBManager.share.GetAllListFromTable(Stock.TABLE_NAME, typeof(Stock));
            int totalValue = 0;
            if (stockList != null && stockList.Count > 0)
            {
                var myStocks = stockList.FindAll(x => x.AccountName == Name);
                foreach (Stock stock in myStocks)
                {
                    var nowValue = DBManager.share.GetStockClosingInfo(stock.StockID);
                    if (nowValue != null)
                    {
                        totalValue += (int)(nowValue.ClosingPrice * stock.Count * (1 - 0.001425 * Fee - 0.003));
                    }
                }
            }
            return totalValue;
        }

        private int CalculateRealBenefit()
        {
            List<Order> orderList = (List<Order>)DBManager.share.GetAllListFromTable(Order.TABLE_NAME, typeof(Order));
            int totalBenefit = 0;
            if (orderList != null && orderList.Count > 0)
            {
                var myOrders = orderList.FindAll(x => x.AccountName == Name);
                foreach (Order o in myOrders)
                {
                    totalBenefit += o.Benefit;
                }
            }
            return totalBenefit;
        }
    }
}
