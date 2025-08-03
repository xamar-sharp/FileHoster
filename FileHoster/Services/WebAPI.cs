using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using FileHoster.Models;
using Newtonsoft.Json;
#nullable disable
namespace FileHoster.Services
{
    public sealed class WebAPI
    {
        internal const string ApiIP = "http://0.0.0.0:5000/filehoster/";
        internal static HttpClient client = new HttpClient(new HttpClientHandler() { MaxConnectionsPerServer = Environment.ProcessorCount, AutomaticDecompression = DecompressionMethods.All })
        {
            MaxResponseContentBufferSize = int.MaxValue,
            BaseAddress = new Uri(ApiIP),
            Timeout = TimeSpan.FromSeconds(300)
        };
        static void EnsureSigned()
        {
            if (!App.IsSign)
            {
                throw new AccessViolationException();
            }
        }
        internal static async Task<FileModel[]> GetFiles()
        {
            try
            {
                return JsonConvert.DeserializeObject<FileModel[]>(await client.GetStringAsync($"{App.UserName}"));
            }
            catch(Exception ex) {
                client.CancelPendingRequests();
                return Array.Empty<FileModel>();
            }
        }
        internal static async Task<bool> PostFile(FileModel model)
        {
            EnsureSigned();
            var msg = await client.PostAsync("", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            return msg.IsSuccessStatusCode;
        }
        internal static async Task<byte[]> DownloadFile(string fileName)
        {
            EnsureSigned();
            return await client.GetByteArrayAsync($"{App.UserName}/{fileName}");
        }
        internal static async Task<bool> DeleteFile(string fileName)
        {
            return (await client.DeleteAsync($"{fileName}")).IsSuccessStatusCode;
        }
    }
}
