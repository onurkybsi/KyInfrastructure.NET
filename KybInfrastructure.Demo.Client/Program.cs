﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KybInfrastructure.Demo.Client
{
    class Program
    {
        static List<long> mss = new List<long>();
        static object alock = new();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string bearerToken = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJyZXBvc2l0b3J5LW1hbmFnZXItdGVzdC1jb25zdW1lciJ9._JPxctPm6IP7MbFM04e3Tl4vzTNZsl9l7iDq7kWDM_g";
            object response = SendGetRequest<object>("http://localhost:8080/maas/repositoryManager/getDomains?clientName=repository-manager-test-consumer&name=test&offset=0&limit=0", 0, bearerToken);
        }

        public static Stopwatch ExecuteGetRequest(int requestCount, string url)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            for (int i = 0; i < requestCount; i++)
            {
                List<Product> products = SendGetRequest<List<Product>>(url, i);
                Console.WriteLine($"{i}. response: {products}");
            }
            stopwatch.Stop();
            return stopwatch;
        }

        public static Stopwatch ExecuteGetRequestParallel(int requestCount, string url)
        {
            List<Task> tasks = new();
            Stopwatch stopwatch = new();
            stopwatch.Start();
            for (int i = 0; i < requestCount; i++)
            {
                Task task = new((ix) =>
                {
                    List<Product> products = SendGetRequest<List<Product>>(url, (int)ix);
                    Console.WriteLine($"{ix}. response: {JsonConvert.SerializeObject(products)}");
                }, i);
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            return stopwatch;
        }

        public static Stopwatch ExecutePostRequest(int requestCount, string url)
        {
            Stopwatch stopwatch = new();
            User user = new()
            {
                FirstName = "Aleyna",
                LastName = "Guner",
                Email = "aleynaguner@hotmail.com",
                Role = UserRole.User
            };
            stopwatch.Start();
            for (int i = 0; i < requestCount; i++)
            {
                _ = SendPostRequest<object>(url, user, i);
            }
            stopwatch.Stop();
            return stopwatch;
        }

        public static Stopwatch ExecutePostRequestParallel(int requestCount, string url)
        {
            List<Task> tasks = new();
            Stopwatch stopwatch = new();
            User user = new User
            {
                FirstName = "Aleyna",
                LastName = "Guner",
                Email = "aleynaguner@hotmail.com",
                Role = UserRole.User
            };
            stopwatch.Start();
            for (int i = 0; i < requestCount; i++)
            {
                Task task = new((ix) =>
                {
                    _ = SendPostRequest<object>(url, user, (int)ix);
                }, i);
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            return stopwatch;
        }

        public static TResponse SendGetRequest<TResponse>(string uri, int ix, string bearerToken = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = 1000000;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (bearerToken != null)
                request.Headers.Add(HttpRequestHeader.Authorization, bearerToken);
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

        private static TResponse SendPostRequest<TResponse>(string uri, object requestBody, int ix)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
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
