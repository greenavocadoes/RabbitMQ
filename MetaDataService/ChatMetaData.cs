using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MetaDataService
{
    public class ChatMetaData
    {
        public async Task<HttpResponseMessage>  makeAPICall(String action, String method)
        {
            using (var handler = new HttpClientHandler())
            {
                handler.Credentials = new System.Net.NetworkCredential("guest", "guest");    
                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri("http://ec2-54-213-74-83.us-west-2.compute.amazonaws.com:15672/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));                    

                    HttpResponseMessage response = await client.GetAsync(action);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }

                }

            }
            return null;
        }

        /*
        public static void Main(String [] args)
        {
            HttpResponseMessage responseTask = makeAPICall("api/exchanges", "GET")
            int i = 0;        

        }

        */

        public void getExchanges()
        {
            var response = await makeAPICall("api/exchanges", "GET");            
            int i = 0;        

        
        }

    }
}
