using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace KybInfrastructure.Demo.Client
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Stopwatch stopwatch = new Stopwatch();
            //ExecuteRequest(1000, stopwatch);
            ExecuteRequestParallel(1000, stopwatch);
            Console.WriteLine($"All requests completed in: {stopwatch.ElapsedMilliseconds}ms");
        }

        public static void ExecuteRequest(int requestCount, Stopwatch stopwatch)
        {
            stopwatch.Start();
            for (int i = 0; i < requestCount; i++)
            {
                List<Product> products = SendGetRequest<List<Product>>("http://localhost:5000/product/getallproducts", i);
                Console.WriteLine($"{i}. response: {products}");
            }
            stopwatch.Stop();
        }

        public static void ExecuteRequestParallel(int requestCount, Stopwatch stopwatch)
        {
            List<Task> tasks = new();
            stopwatch.Start();
            for (int i = 0; i < requestCount; i++)
            {
                Task task = new((ix) =>
                {
                    List<Product> products = SendGetRequest<List<Product>>("http://localhost:5000/product/getallproducts", (int)ix);
                    Console.WriteLine($"{i}. response: {JsonConvert.SerializeObject(products)}");
                }, i);
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
        }

        public static TResponse SendGetRequest<TResponse>(string uri, int ix)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            Stopwatch timer = new();
            timer.Start();
            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            timer.Stop();
            Console.WriteLine($"{ix}. completed in: {timer.ElapsedMilliseconds}ms");
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new(stream);
            string result = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<TResponse>(result);
        }

        private static TResponse SendPostRequest<TResponse>(object requestBody, int ix)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://url");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(JsonConvert.SerializeObject(requestBody));
            }

            Stopwatch timer = new();
            timer.Start();
            using HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            timer.Stop();
            Console.WriteLine($"{ix}. completed in: {timer.ElapsedMilliseconds}ms");
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new(stream);
            string result = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<TResponse>(result);
        }
    }
}
