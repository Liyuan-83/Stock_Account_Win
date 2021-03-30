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

namespace MySQLiteDB
{
    class DBManager
    {
        private SQLiteConnection sqlite_connect;
        private SQLiteCommand sqlite_cmd;

        private void ConnectDB() {
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

        public void NewData(_DefaultModel data)
        {
            ConnectDB();
            sqlite_cmd.CommandText = data.CreateTable();

            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = data.InsertValue();
            sqlite_cmd.ExecuteNonQuery();

            sqlite_connect.Close();
        }

        public IList GetData(String tableName, Type type)
        {
            ConnectDB();
            try
            {
                sqlite_cmd.CommandText = "SELECT * FROM " + tableName; //select table

                SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
                Type listType = typeof(List<>).MakeGenericType(new Type[] { type });

                IList list = Activator.CreateInstance(listType) as IList;
                while (sqlite_datareader.Read())
                {
                    var temp = Activator.CreateInstance(type) as _DefaultModel;
                    temp.SetValue(sqlite_datareader);
                    list.Add(temp);
                }
                sqlite_connect.Close();

                return list;
            }
            catch {
                return null;
            }
        }
    }
}
