using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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


        public static APIModel_StockClosingInfo GetStockClosingInfo(string StockID)
        {
            try
            {
                string apiUrl = API_URIs.TWSE + API_URIs.STOCK_DAY + "date=" + DateTime.Today.ToString("yyyyMMdd") + "&stockNo=" + StockID;
                using (HttpClient client = new HttpClient())
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    var t = Task.Run(async delegate
                    {
                        Random rd = new Random(DateTime.Now.Second);
                        await Task.Delay(rd.Next(1, 3) * 1000, source.Token);
                        var response = await client.GetAsync(apiUrl);
                        return response;
                    });
                    return t.Result.Content.ReadAsAsync<APIModel_StockClosingInfo>().Result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
