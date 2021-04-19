using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySQLiteDB;
using MySQLiteDB.Model;

namespace Stock_Accounting.Pages.Alert
{
    /// <summary>
    /// SelectStockAlert.xaml 的互動邏輯
    /// </summary>
    public partial class SelectStockAlert : Window
    {
        private List<Stock> _stocks;
        private List<Stock> _selectStocks = new List<Stock>();

        public List<Stock> SelectedStocks
        {
            get { return _selectStocks; }
            set
            {
                _selectStocks = value;
                Clear_Btn.IsEnabled = _selectStocks.Count > 0;
                OK_Btn.IsEnabled = _selectStocks.Count > 0;
            }
        }
        private List<Stock> Stocks
        {
            get
            {
                return _stocks;
            }
            set
            {
                _stocks = value ?? new List<Stock>();
                Stock_DataGrid.Items.Clear();
                foreach (Stock item in _stocks)
                {
                    Stock_DataGrid.Items.Add(item);
                }
            }
        }

        public SelectStockAlert(string id)
        {
            InitializeComponent();
            List<Stock> list = (List<Stock>)DBManager.share.GetAllListFromTable(Stock.TABLE_NAME, typeof(Stock));
            Stocks = list.FindAll(x => (x.StockID == id || id == "") && x.Count > 0);
            Select_All_Btn.Visibility = id != "" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void DataReload()
        {
            SelectedStocks = Stocks.FindAll(x => x.IsSelected);

            if (SelectedStocks.Count > 0)
            {
                foreach (Stock s in Stocks) { s.IsCanSelect = s.StockID == SelectedStocks.First().StockID; }
            }
            else
            {
                foreach (Stock s in Stocks) { s.IsCanSelect = true; }
            }
        }

        private void Select_All_Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Stock s in Stocks) { s.IsSelected = true; s.SaleCount = s.Count; }
            DataReload();
        }

        private void Clear_Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Stock s in Stocks) { s.IsSelected = false; s.IsCanSelect = true; s.SaleCount = 0; }
            DataReload();
        }

        private void OK_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Sale_Count_TextChange(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Console.WriteLine(Stock_DataGrid.SelectedIndex);

            Console.WriteLine(box.Text);
            if (Stock_DataGrid.SelectedIndex >= 0)
            {
                Stock item = (Stock)Stock_DataGrid.SelectedItem;
                if (Int32.TryParse(box.Text, out int count))
                {
                    if (count > item.Count)
                    {
                        count = item.Count;
                    }
                }
                else if (box.Text == "")
                {
                    count = 0;
                }
                else
                {
                    count = item.SaleCount;
                }
                item.SaleCount = count;
                item.IsSelected = count > 0;
                DataReload();
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (Stock_DataGrid.SelectedIndex >= 0 && checkBox.IsChecked != null)
            {
                Stocks[Stock_DataGrid.SelectedIndex].IsSelected = (bool)checkBox.IsChecked;
                Stocks[Stock_DataGrid.SelectedIndex].SaleCount = (bool)checkBox.IsChecked ? Stocks[Stock_DataGrid.SelectedIndex].Count : 0;
                DataReload();
            }
        }
    }
}
