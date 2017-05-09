using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Net;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using CodingMonkeyNet.SumpPumpMonitor.Data.Configuration;
using CodingMonkeyNet.SumpPumpMonitor.Data.Utilities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public class TwinRepository
    {
        private static readonly JsonSerializerSettings JsonSettings;
        private static readonly long UtcReference = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks;
        private static HttpClient client = null;
        private readonly IoTHubConfiguration Config;

        static TwinRepository()
        {
            JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            Mapper.Initialize(cfg => {
                cfg.CreateMap<DeviceTwinEntity, DeviceTwinUpdateEntity>();
                cfg.CreateMap<SumpPumpSettingPair, SumpPumpSettingUpdate>();
            });
        }

        public TwinRepository(IoTHubConfiguration config)
        {
            Config = config;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://" + config.HostName);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<IEnumerable<DeviceTwinEntity>> All()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/devices/query?api-version=2016-11-14");
            request.Content = new StringContent("{ \"query\": \"SELECT * from devices\" }", Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("SharedAccessSignature",
                GetSharedAccessSignature(Config.SharedAccessKeyName, Config.SharedAccessKey, Config.HostName, new TimeSpan(1, 0, 0)));

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<DeviceTwinEntity>>(responseString);
        }

        public async Task<DeviceTwinEntity> ById(string deviceId)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("/twins/{0}?api-version=2016-11-14", deviceId));
            request.Headers.Authorization = new AuthenticationHeaderValue("SharedAccessSignature",
                GetSharedAccessSignature(Config.SharedAccessKeyName, Config.SharedAccessKey, Config.HostName, new TimeSpan(1, 0, 0)));

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DeviceTwinEntity>(responseString);
        }

        public async Task Update(DeviceTwinEntity entity)
        {
            DeviceTwinUpdateEntity patchEntity = Mapper.Map<DeviceTwinUpdateEntity>(entity);
            var method = new HttpMethod("PATCH");
            HttpRequestMessage request = new HttpRequestMessage(method, string.Format("/twins/{0}?api-version=2016-11-14", entity.DeviceId));
            string json = JsonConvert.SerializeObject(patchEntity, JsonSettings);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("SharedAccessSignature",
                GetSharedAccessSignature(Config.SharedAccessKeyName, Config.SharedAccessKey, Config.HostName, new TimeSpan(1, 0, 0)));

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private static string GetSharedAccessSignature(string keyName, string sharedAccessKey, string resource, TimeSpan tokenTimeToLive)
        {
            string expiry = ((long)(DateTime.UtcNow - new DateTime(UtcReference, DateTimeKind.Utc) + tokenTimeToLive).TotalSeconds()).ToString();
            string encodedUri = WebUtility.UrlEncode(resource);

            HMACSHA256 sha = new HMACSHA256(Convert.FromBase64String(sharedAccessKey));
            byte[] hmac = sha.ComputeHash(Encoding.UTF8.GetBytes(encodedUri + "\n" + expiry));

            string sig = Convert.ToBase64String(hmac);

            if (keyName != null)
            {
                return string.Format("sr={0}&sig={1}&se={2}&skn={3}",
                encodedUri,
                WebUtility.UrlEncode(sig),
                WebUtility.UrlEncode(expiry),
                WebUtility.UrlEncode(keyName));
            }
            else
            {
                return string.Format("sr={0}&sig={1}&se={2}",
                    encodedUri,
                    WebUtility.UrlEncode(sig),
                    WebUtility.UrlEncode(expiry));
            }
        }
    }
}
