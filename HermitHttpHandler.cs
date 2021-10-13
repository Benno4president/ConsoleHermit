using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHermit
{
    public class HermitHttpHandler
    {
        internal static class Creds 
        {
            /* good practice.. this is not */
            public const string username = "";
            public const string password = "";
        }

        internal async Task<string> GetJsonAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Creds.username}:{Creds.password}"));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var response = await httpClient.SendAsync(request);

                    string temp = await response.Content.ReadAsStringAsync();
                    return temp;
                }
            }

        }
    }
}
