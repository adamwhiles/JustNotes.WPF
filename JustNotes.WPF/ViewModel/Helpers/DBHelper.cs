using JustNotes.WPF.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JustNotes.WPF.ViewModel.Helpers
{
    public class DBHelper
    {
        private static string dbFile = Path.Combine(Environment.CurrentDirectory, "justnotesDB.db3");
        private static string API_KEY = ConfigurationManager.AppSettings.Get("fireBaseApiKey");
        private static string dbPath = ConfigurationManager.AppSettings.Get("fireBaseDb");

        public static async Task<bool> Insert<T>(T item)
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using(var client = new HttpClient())
            {
                var result = await client.PostAsync($"{dbPath}{item.GetType().Name.ToLower()}.json", content);

                if(result.IsSuccessStatusCode)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public static async Task<bool> Update<T>(T item) where T : HasId
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result = await client.PutAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<bool> Delete<T>(T item) where T : HasId
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var result = await client.DeleteAsync($"{dbPath}{item.GetType().Name.ToLower()}/{item.Id}.json");

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static async Task<List<T>> Read<T>() where T : HasId
        {
            List<T> items = new List<T>();


            using (var client = new HttpClient())
            {
                var result = await client.GetAsync($"{dbPath}{typeof(T).Name.ToLower()}.json");
                var jsonResult = await result.Content.ReadAsStringAsync();                

                if (result.IsSuccessStatusCode)
                {
                    var objects = JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonResult);
                    if (objects == null) return items;
                    foreach(var o in objects)
                    {
                        o.Value.Id = o.Key;
                        items.Add(o.Value);
                    }

                    return items;
                }
                else
                {
                    return items;
                }
            }
        }
    }
}
