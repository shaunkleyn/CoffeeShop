using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Common
{
    public static class Extensions
    {
        public static string AppendToURL(this string baseURL, params string[] segments)
        {
            {
                return string.Join("/", new[] { baseURL.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
            }
        }

        public static async Task<T> PostAsJsonAsync<T>(this HttpClient httpClient, string url, object postData)
        {
            var json = JsonConvert.SerializeObject(postData);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json"); 
            var result = await httpClient.PostAsync(url, stringContent);
            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());

        }
    }


}
