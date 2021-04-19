using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySQLiteDB.Model;
using MySQLiteDB;
using System.Threading;
using System.Windows.Threading;

namespace Stock_Accounting.Manager
{
    public class InternetManager
    {
        public static InternetManager share = new InternetManager();

        public Action<string> UpdateMsgFunc;

        public Action<bool> ShowLoading;

        public bool CheckConnection()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://google.com");
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateCompanyData(Thread main)
        {
            using (WebClient client = new WebClient())
            {
                Dispatcher.FromThread(main).Invoke(DispatcherPriority.Normal, ShowLoading, false);
                
                client.DownloadFile("http://mopsfin.twse.com.tw/opendata/t187ap03_L.csv", "listed.csv");
                client.DownloadFile("http://mopsfin.twse.com.tw/opendata/t187ap03_O.csv", "OTC.csv");
                //興櫃
                //client.DownloadFile("http://mopsfin.twse.com.tw/opendata/t187ap03_R.csv", "hing_counter.csv");
                CompanyConverter("listed.csv");
                CompanyConverter("OTC.csv");
                Dispatcher.FromThread(main).Invoke(DispatcherPriority.Normal, ShowLoading, true);
            }
        }

        private void CompanyConverter(string filePath)
        {
            string[] strArr = File.ReadAllLines(filePath);
            int lineCount = strArr.Length;
            var level = (filePath == "listed.csv") ? CompanyInfo.Company_level.listed : CompanyInfo.Company_level.OTC;
            int count = 1;

            for (int line = 1; line < lineCount - 1; line++)
            {
                string[] rows = Regex.Split(strArr[line], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i] = rows[i].Replace("  ", "").Replace("\"", "").Replace("\'", "");
                }
                CompanyInfo temp = new CompanyInfo(rows, level);
                DBManager.share.InsertOrUpdateData(temp);
                count++;
                share.UpdateMsgFunc(count + "/" + (lineCount - 1));
            }
        }
    }
}
