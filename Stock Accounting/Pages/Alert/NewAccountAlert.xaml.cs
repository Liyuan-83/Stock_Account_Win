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

namespace Stock_Accounting.Pages.Alert
{
    /// <summary>
    /// NewAccountAlert.xaml 的互動邏輯
    /// </summary>
    public partial class NewAccountAlert : Window
    {
        public Account account { get; } = new Account();

        public NewAccountAlert()
        {
            InitializeComponent();
        }

        private void OK_Btn_Click(object sender, RoutedEventArgs e)
        {
            account.Name = Account.Text;
            account.StockValue = 0;
            account.Cash = Int32.Parse(Cash.Text);
            account.Fee = Double.Parse((Fee.Text == "") ? "100" : Fee.Text) / 100;
            account.Assets = account.StockValue + account.Cash;

            DialogResult = true;
            Close();
        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TextBox_TextChangedChanged(object sender, TextChangedEventArgs e)
        {
            if (Account.Text != "" && Cash.Text != "" && Cash.Text.All(Char.IsDigit) && Fee.Text.All(Char.IsDigit))
            {
                OK_Btn.IsEnabled = true;
            }
            else
            {
                OK_Btn.IsEnabled = false;
            }
        }
    }
}
