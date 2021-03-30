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
                _accountItems = value;
                Account_List.ItemsSource = _accountItems;
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

        }

        private void Added_Button_Click(object sender, RoutedEventArgs e)
        {
            //var temp = new Account() { Name = "test" + count };
            //_db.Acounts.Add(temp);
            //_db.SaveChanges();
            //count++;
            Account account = new Account();
            account.Name = "test";
            account.Assets = 10000;
            db.NewData(account);
            AccountItems = db.GetData(Account.TABLE_NAME, typeof(Account)) as List<Account>;
            
        }
    }
}
