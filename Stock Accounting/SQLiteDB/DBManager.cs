using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Windows;
using System.Diagnostics;
using MySQLiteDB.Model;
using System.Collections;
using System.Globalization;
using Stock_Accounting.Manager;

namespace MySQLiteDB
{
    class DBManager
    {
        public static DBManager share = new DBManager();
        private SQLiteConnection sqlite_connect;
        private SQLiteCommand sqlite_cmd;

        private void ConnectDB()
        {
            if (!File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + @"\myData.db"))
            {
                SQLiteConnection.CreateFile("myData.db");
                Debug.Print("Creat DB File");
                Debug.Print(System.AppDomain.CurrentDomain.BaseDirectory + @"\myData.db");
            }
            else
            {
                Debug.Print("DB File Exist");
            }
            sqlite_connect = new SQLiteConnection("Data source=myData.db");

            sqlite_connect.Open();
            sqlite_cmd = sqlite_connect.CreateCommand();
        }

        public void InsertOrUpdateData(_DefaultModel data)
        {
            ConnectDB();
            sqlite_cmd.CommandText = data.CreateTable();

            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = data.InsertOrUpdateValue();

            sqlite_cmd.ExecuteNonQuery();

            sqlite_connect.Close();
        }

        public void RemoveData(_DefaultModel data)
        {
            ConnectDB();
            try
            {
                sqlite_cmd.CommandText = "DELETE FROM '" + data.TableName() + "' WHERE ID = " + data.ID;
                sqlite_cmd.ExecuteNonQuery();

                sqlite_cmd.CommandText = "SELECT count(*) FROM '" + data.TableName() + "'";
                var count = Int32.Parse(sqlite_cmd.ExecuteScalar().ToString());
                if (count == 0)
                {
                    sqlite_cmd.CommandText = "DROP TABLE '" + data.TableName() + "'";
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch { }
            sqlite_connect.Close();
        }

        public IList GetAllListFromTable(String tableName, Type type)
        {
            ConnectDB();
            try
            {
                sqlite_cmd.CommandText = "SELECT * FROM '" + tableName + "'"; //select table

                SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
                Type listType = typeof(List<>).MakeGenericType(new Type[] { type });

                IList list = Activator.CreateInstance(listType) as IList;
                while (sqlite_datareader.Read())
                {
                    var temp = Activator.CreateInstance(type, sqlite_datareader);
                    list.Add(temp);
                }
                sqlite_connect.Close();

                return list;
            }
            catch
            {
                sqlite_connect.Close();
                return null;
            }
        }

        public _DefaultModel GetNewestData(String tableName, Type type)
        {
            ConnectDB();
            try
            {
                sqlite_cmd.CommandText = "select* from '" + tableName + "' order by id desc limit 0,1"; //select table
                SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
                _DefaultModel temp = (_DefaultModel)Activator.CreateInstance(type);
                while (sqlite_datareader.Read())
                {
                    temp = (_DefaultModel)Activator.CreateInstance(type, sqlite_datareader);
                }
                sqlite_connect.Close();
                return temp;
            }
            catch
            {
                sqlite_connect.Close();
                return null;
            }

        }

        public bool ShouldUpdateCompanyData()
        {
            ConnectDB();
            try
            {
                sqlite_cmd.CommandText = "SELECT * FROM " + CompanyInfo.TABLE_NAME + " LIMIT 1";
                SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
                CompanyInfo temp = new CompanyInfo();
                while (sqlite_datareader.Read())
                {
                    temp = (CompanyInfo)Activator.CreateInstance(typeof(CompanyInfo), sqlite_datareader);
                }
                sqlite_connect.Close();
                var twCulture = new CultureInfo("zh-TW", true);
                twCulture.DateTimeFormat.Calendar = new TaiwanCalendar();
                string str = temp.UpdateDate;
                str = str.PadLeft(8, '0');
                DateTime today = DateTime.Now;
                DateTime updateTime = DateTime.ParseExact(str, "yMMdd", twCulture);
                TimeSpan sp = today.Subtract(updateTime);
                return sp.TotalDays > 10;
            }
            catch
            {
                sqlite_connect.Close();
                return true;
            }
        }

        public StockClosingInfo GetStockClosingInfo(string id)
        {
            List<StockClosingInfo> list = (List<StockClosingInfo>)GetAllListFromTable(StockClosingInfo.TABLE_NAME, typeof(StockClosingInfo));
            if (list == null ||
                list.Find(x => x.ID == id) == null ||
                (list.Find(x => x.ID == id).Date.ToString("yyyyMMdd") != DateTime.Today.ToString("yyyyMMdd") && DateTime.Now.Hour > 15))
            {
                CompanyInfo company = ((List<CompanyInfo>)GetAllListFromTable(CompanyInfo.TABLE_NAME, typeof(CompanyInfo))).Find(x => x.ID == id);
                StockClosingInfo info = new StockClosingInfo(id, company.Nickname, WebAPIManager.GetStockClosingInfo(id));
                InsertOrUpdateData(info);
                return GetStockClosingInfo(id);
            }
            else
            {
                return list.Find(x => x.ID == id);
            }
        }
    }
}
