using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Accounting.Manager
{
    public class InternetManager
    {
        public static InternetManager share = new InternetManager();
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

        public void UpdateCompanyData()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://mopsfin.twse.com.tw/opendata/t187ap03_L.csv", "company.csv");
            }
        }
    }
}
