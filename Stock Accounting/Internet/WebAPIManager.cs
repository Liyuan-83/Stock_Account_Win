using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Stock_Accounting.APIModel;

namespace Stock_Accounting.Manager
{


    class WebAPIManager
    {
        private class API_URIs
        {
            public static string TWSE = "https://www.twse.com.tw/exchangeReport/";
            public static string STOCK_DAY = "STOCK_DAY?";
        }


        public static Task<StockClosingInfo> GetStockClosingInfo(string StockID)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string apiUrl = API_URIs.TWSE + API_URIs.STOCK_DAY + "date=" + DateTime.Today.ToString("yyyyMMdd") + "&stockNo=" + StockID;
                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetAsync(apiUrl);
                    response.Wait();
                    return response.Result.Content.ReadAsAsync<StockClosingInfo>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
