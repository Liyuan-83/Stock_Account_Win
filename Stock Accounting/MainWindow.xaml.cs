using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using MySQLiteDB;
using MySQLiteDB.Model;
using Stock_Accounting.Pages.Alert;

namespace Stock_Accounting
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBManager db = new DBManager();
        private List<Account> _accountItems = new List<Account>();
        private List<Account> AccountItems
        {
            set
            {
                _accountItems = value ?? new List<Account>();
                Account_List.Items.Clear();
                int totalAssets = 0;
                int totalStock = 0;
                int totalCash = 0;
                foreach (Account item in _accountItems)
                {
                    totalAssets += item.Assets;
                    totalStock += item.StockValue;
                    totalCash += item.Cash;
                    Account_List.Items.Add(item);
                }
                Total_Assets.Content = totalAssets;
                Total_Cash.Content = totalCash;
                Total_Value.Content = totalStock;
            }
            get
            {
                return _accountItems;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            AccountItems = db.GetData(Account.TABLE_NAME, typeof(Account)) as List<Account>;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Rm_Btn.IsEnabled = Account_List.SelectedCells.Count > 0;
        }

        private void Added_Button_Click(object sender, RoutedEventArgs e)
        {

            var alert = new NewAccountAlert();
            switch (alert.ShowDialog())
            {
                case true:
                    db.NewData(alert.account);
                    AccountItems = db.GetData(Account.TABLE_NAME, typeof(Account)) as List<Account>;
                    break;
                default:

                    break;
            }
        }

        private void DataGridCell_MouceDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = Account_List.SelectedIndex;
            Console.WriteLine(AccountItems[index].Name);
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            Account selectedItem = Account_List.SelectedItem as Account;
            db.RemoveData(selectedItem);
            AccountItems = db.GetData(Account.TABLE_NAME, typeof(Account)) as List<Account>;
            Rm_Btn.IsEnabled = false;
        }
    }
}
