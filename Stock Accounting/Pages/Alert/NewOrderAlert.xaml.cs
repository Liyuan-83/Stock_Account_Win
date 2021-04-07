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
using MySQLiteDB.Model;
using MySQLiteDB;

namespace Stock_Accounting.Pages.Alert
{
    /// <summary>
    /// NewOrderAlert.xaml 的互動邏輯
    /// </summary>
    public partial class NewOrderAlert : Window
    {
        private List<Account> Accounts = (List<Account>)DBManager.share.GetAllListFromTable(Account.TABLE_NAME, typeof(Account));
        private Order Order = new Order();

        public NewOrderAlert(int selectIndex = 0)
        {
            InitializeComponent();
            List<string> nameArr = new List<string>();

            foreach (Account ac in Accounts)
            {
                nameArr.Add(ac.Name);
            }
            Account_Selection.ItemsSource = nameArr;
            Account_Selection.SelectedIndex = selectIndex;
        }

        private void Account_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order.AccountName = Accounts[Account_Selection.SelectedIndex].Name;
        }
    }
}
