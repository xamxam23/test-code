using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.IO;

namespace Test_MultiChoice.Data
{
    public class RestClient
    {
        private static string SERVER_URL = "http://www.m6world.com/multichoice";
        public string Cmd; // command
        Dictionary<string, string> Map = new Dictionary<string, string>();
        public RestClient(string cmd)
        {
            try
            {
                SERVER_URL = File.ReadAllText("config.txt");
                if (SERVER_URL != null)
                    SERVER_URL = SERVER_URL.Trim();
            }
            catch (IOException e) { }
            this.Cmd = cmd;
        }

        private string Post()
        {
            var rs = "";
            try
            {
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["cmd"] = Cmd;
                    foreach (string key in Map.Keys)
                        values[key] = Map[key];
                    var r = client.UploadValues(SERVER_URL + "/server.php", values);
                    rs = Encoding.Default.GetString(r);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rs;
        }
        
        public RestClient Put(string key, string value)
        { // builder pattern;
            Map.Add(key, value);
            return this;
        }

        public T Execute<T>()
        {
            try
            {
                string result = Post();
                T response = JsonConvert.DeserializeObject<T>(result);
                return response;
            }
            catch (JsonException e)
            {
                Console.WriteLine(e);
            }
            return default(T);
        }
    }
}
