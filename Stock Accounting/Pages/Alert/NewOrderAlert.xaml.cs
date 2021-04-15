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
using Stock_Accounting.Manager;

namespace Stock_Accounting.Pages.Alert
{
    /// <summary>
    /// NewOrderAlert.xaml 的互動邏輯
    /// </summary>
    public partial class NewOrderAlert : Window
    {
        private List<Account> Accounts = (List<Account>)DBManager.share.GetAllListFromTable(Account.TABLE_NAME, typeof(Account));
        private List<CompanyInfo> CompanyInfos = (List<CompanyInfo>)DBManager.share.GetAllListFromTable(CompanyInfo.TABLE_NAME, typeof(CompanyInfo));
        private Order Order = new Order();
        private CompanyInfo selectedCompany;
        private double StockClosingPrice = 0;

        public NewOrderAlert(int selectIndex = 0)
        {
            InitializeComponent();
            SetupDataSource();
            SetupDataBinding();
            Account_Selection.SelectedIndex = selectIndex;
        }

        public NewOrderAlert(Order order, int selectIndex = 0)
        {
            InitializeComponent();
            SetupDataSource();
            SetupDataBinding();
            Account_Selection.SelectedIndex = selectIndex;
            Order = order;
        }

        private void SetupDataSource()
        {
            DataContext = Order;
            List<string> NameArr = new List<string>();
            foreach (Account ac in Accounts)
            {
                NameArr.Add(ac.Name);
            }
            Account_Selection.ItemsSource = NameArr;

            List<string> ComanyNameArr = new List<string>();
            foreach (CompanyInfo company in CompanyInfos)
            {
                ComanyNameArr.Add(company.ID + " " + company.Nickname);
            }
            Stock_Info.ItemsSource = ComanyNameArr;

            Type_Selection.ItemsSource = Order.OrderType;
        }

        private void SetupDataBinding()
        {
            var DateBinding = new Binding("Date")
            {
                Source = this.Order
            };
            Order_Date_Label.SetBinding(Label.ContentProperty, DateBinding);

            var AccountBinding = new Binding("AccountName")
            {
                Source = this.Order
            };
            Order_Account_Label.SetBinding(Label.ContentProperty, AccountBinding);

            var IsBuyBinding = new Binding("Content")
            {
                Source = IsBuy_Button
            };
            Order_IsBuy_Label.SetBinding(Label.ContentProperty, IsBuyBinding);

            var StockNameBinding = new Binding("StockName")
            {
                Source = this.Order
            };
            Order_StockName_Label.SetBinding(Label.ContentProperty, StockNameBinding);

            var StockIDBinding = new Binding("StockID")
            {
                Source = this.Order
            };
            Order_StockID_Label.SetBinding(Label.ContentProperty, StockIDBinding);

            var TypeBinding = new Binding("SelectedItem")
            {
                Source = Type_Selection
            };
            Order_Type_Label.SetBinding(Label.ContentProperty, TypeBinding);

            var CountBinding = new Binding("Count")
            {
                Source = Order
            };
            Order_Count_Label.SetBinding(Label.ContentProperty, CountBinding);

            var PriceBinding = new Binding("Price")
            {
                Source = Order
            };
            Order_Price_Label.SetBinding(Label.ContentProperty, PriceBinding);

            var FeeBinding = new Binding("Fee")
            {
                Source = Order
            };
            Order_Fee_Label.SetBinding(Label.ContentProperty, FeeBinding);

            var TaxBinding = new Binding("Tax")
            {
                Source = Order
            };
            Order_Tax_Label.SetBinding(Label.ContentProperty, TaxBinding);

            var CostBinding = new Binding("Cost")
            {
                Source = Order
            };
            Total_Cost_Label.SetBinding(Label.ContentProperty, CostBinding);
        }

        private void Calculation()
        {
            OK_Btn.IsEnabled = false;
            if (Account_Selection.SelectedIndex < 0 || Stock_Info.SelectedIndex < 0 || Order.Count <= 0 || Order.Price <= 0)
            {
                Order.Fee = 0;
                Order.Tax = 0;
                return;
            }

            int fee = (int)Math.Ceiling(Order.Count * Order.Price * 0.001425 * Accounts[Account_Selection.SelectedIndex].Fee);
            fee = (Order.Type == 3 || fee >= 20) ? fee : 20;
            Order.Fee = fee;

            int tax = (int)Math.Ceiling(Order.Count * Order.Price * 0.003);
            tax = (Order.IsBuy) ? 0 : tax;
            Order.Tax = tax;
            if (Order.Type == 1)
            {
                //使用融資
                if (Order.IsBuy)
                {
                    Order.Cost = (int)Math.Ceiling(Order.Price * Order.Count * ((selectedCompany.Level == CompanyInfo.Company_level.listed) ? 0.4 : 0.5) / 100) * 100 - Order.Fee;
                    Order.Borrow = (int)(Order.Price * Order.Count * ((selectedCompany.Level == CompanyInfo.Company_level.listed) ? 0.6 : 0.5) / 100) * 100;
                }
                else
                {

                }
            }
            else if (Order.Type == 2)
            {
                //使用融券
                //int borrow = (int)((Order.IsBuy ? 0 : 0.08) * Order.Price * Order.Count);
                //int guarantee = (int)Math.Ceiling((Order.IsBuy ? 0 : -0.9) * Order.Cost / 100) * 100;
                //Order.Cost = guarantee - borrow;
            }
            else
            {
                //現股 & 零股
                Order.Cost = (int)(Order.Price * Order.Count) * (Order.IsBuy ? -1 : 1) - Order.Fee - Order.Tax;
            }
            Total_Cost_Label.Foreground = (Order.Cost >= 0) ? Brushes.DarkRed : Brushes.DarkGreen;
            Total_Cost_Label.Background = (Order.Cost >= 0) ? Brushes.LightPink : Brushes.LightGreen;
            Total_Value_Label.Content = (int)(StockClosingPrice * Order.Count);
            OK_Btn.IsEnabled = true;
        }

        private void UIUpdate()
        {
            Value_Grid.Visibility = Order.IsBuy ? Visibility.Visible : Visibility.Collapsed;
            Benefit_Grid.Visibility = Order.IsBuy ? Visibility.Collapsed : Visibility.Visible;
            Tax_Grid.Visibility = Order.IsBuy ? Visibility.Collapsed : Visibility.Visible;
            Order.Count = 0;
            if (Order.Type == 2)
            {
                //使用融券
                Count_Button.Visibility = Order.IsBuy ? Visibility.Visible : Visibility.Collapsed;
                Count_TextBox.Visibility = Order.IsBuy ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                //現股 & 零股 & 融資
                Count_Button.Visibility = Order.IsBuy ? Visibility.Collapsed : Visibility.Visible;
                Count_TextBox.Visibility = Order.IsBuy ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Account_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            Order.AccountName = Accounts[cmb.SelectedIndex].Name;
            Calculation();
        }

        private void IsBuy_Button_Click(object sender, RoutedEventArgs e)
        {
            Order.IsBuy = !Order.IsBuy;
            Button btn = (Button)sender;
            btn.Content = Order.IsBuy ? "買進" : "賣出";
            btn.Background = Order.IsBuy ? Brushes.DarkRed : Brushes.DarkGreen;
            UIUpdate();
            Calculation();
        }

        private void Stock_Info_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(cmb.ItemsSource);

            itemsViewOriginal.Filter = ((o) =>
            {
                if (string.IsNullOrEmpty(cmb.Text)) return false;
                else
                {
                    if (((string)o).Contains(cmb.Text)) return true;
                    else return false;
                }
            });

            cmb.IsDropDownOpen = true;
            itemsViewOriginal.Refresh();
        }

        private void Stock_Info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            string item = (string)cmb.SelectedItem;

            Price_TextBox.Text = "";
            Count_TextBox.Text = "";
            Order.Count = 0;
            Order.Price = 0;

            if (item != null && item.Contains(" ") && item.Split(' ').Length > 1)
            {
                string[] itemArr = item.Split(' ');
                selectedCompany = CompanyInfos.Find(x => x.ID == itemArr[0]);
                Order.StockID = selectedCompany.ID;
                Order.StockName = selectedCompany.Nickname;
                StockClosingPrice = DBManager.share.GetStockClosingInfo(selectedCompany.ID).ClosingPrice;
            }
            Calculation();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            Order.Date = datePicker.SelectedDate?.ToString("yyyy/MM/dd");
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            Order.Type = cmb.SelectedIndex;
            UIUpdate();
            Calculation();
        }

        private void Count_TextBox_TextChangedChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            var isSuccess = Int32.TryParse(box.Text, out int count);
            if (isSuccess)
            {
                Order.Count = count;
                Calculation();
            }
        }

        private void Count_Button_Click(object sender, RoutedEventArgs e)
        {
            Calculation();
        }

        private void Price_TextBox_TextChangedChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            var isSuccess = double.TryParse(box.Text, out double price);
            if (isSuccess)
            {
                Order.Price = price;
                Calculation();
            }
        }

        private void Mark_TextBox_TextChangedChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            Order.Mark = box.Text;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Stock_Info.SelectedIndex = -1;
            Price_TextBox.Text = "";
            Count_TextBox.Text = "";
            Mark_TextBox.Text = "";
            Order.StockID = "";
            Order.StockName = "";
            Order.Count = 0;
            Order.Price = 0;
            Order.Cost = 0;
            Order.Borrow = 0;
            Order.Mark = "";
            Calculation();
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Order.AccountName + (Order.IsBuy ? "買進" : "賣出") + "(" + Order.StockID + ")" + Order.StockName + " " + Order.Price + "元 " + Order.Count + "股" + " 共" + Order.Cost + "元");

            DBManager.share.InsertOrUpdateData(Order);
            var _order = (Order)DBManager.share.GetNewestData(Order.TABLE_NAME, typeof(Order));
            Stock stock = new Stock(_order);
            DBManager.share.InsertOrUpdateData(stock);

            DialogResult = true;
            Close();
        }
    }
}
