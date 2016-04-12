using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FoxReport.Helper
{
    public class HttpSteamQuerier
    {
        public Identity Identity
        {
            get;
            set;
        }

        public async Task<Stream> ReadAsStreamAsync(string uri)
        {
            try
            {
                var resourceUri = new Uri(uri);
                var request = new HttpRequestMessage(HttpMethod.Get, resourceUri);
                EnsureRequestWithIdentity(request);
                using (HttpMessageHandler handler = new HttpClientHandler()
                {
                    UseCookies = false
                })
                {
                    using (HttpClient client = new HttpClient(handler))
                    {
                        return await client.SendAsync(request)
                            .Result.EnsureSuccessStatusCode().Content.ReadAsStreamAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnsureRequestWithIdentity(HttpRequestMessage request)
        {
            var iterator = Identity.Headers.GetEnumerator();
            while (iterator.MoveNext())
            {
                if (!iterator.Current.ToString().Contains("Accept-Encoding"))
                {
                    request.Headers.TryAddWithoutValidation(iterator.Current.ToString(), Identity.Headers[iterator.Current.ToString()]);
                }
            }
        }
    }
}