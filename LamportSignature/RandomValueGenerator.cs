using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LamportSignature
{
    static class RandomValueGenerator
    {
        static Random rnd = new Random();
        public static List<int> GetRandomValue(int min, int max)
        {
            try
            {
                string json;
                string key = "b5e357d5-ebb4-4292-8814-39cdee26a09e";
                var request = WebRequest.Create("https://api.random.org/json-rpc/1/invoke");
                request.ContentType = "application/json";

                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    json = "{\"jsonrpc\":\"2.0\",\"method\":\"generateIntegers\",\"params\":{\"apiKey\":\"" + key + "\",\"n\": 4096, \"min\":" + min + ", \"max\": " + max + ", \"replacement\": true}, \"id\": 42}";
                    streamWriter.Write(json);
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    json = reader.ReadToEnd();
                }

                ResponseEntity responce = JsonConvert.DeserializeObject<ResponseEntity>(json);

                return responce.result.random.data;
            }
            catch (Exception)
            {
                List<int> result = new List<int>();

                for (int i = 0; i <= 4096; i++)
                {
                    result.Add(rnd.Next());
                }

                return result;
            }
        }
    }
}
