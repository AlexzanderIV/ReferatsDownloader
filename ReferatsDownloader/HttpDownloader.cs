using ReferatsDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReferatsDownloader
{
    public class HttpDownloader
    {
        // TODO: Dependency Injection should goes here
        private IUriBuilder uriBuilder = new UriBuilder();
        private IHttpResponseProcessor responseProcessor = new HttpResponseProcessor();

        public async Task DownloadAsync(InputParameters input)
        {
            using (HttpClient client = new HttpClient())
            {
                var taskList = new List<Task<string>>(input.ReferatsAmount);
                var rand = new Random();
                // Create and start the tasks.
                for (int i = 0; i < input.ReferatsAmount; i++)
                {
                    var uri = uriBuilder.Build(input);
                    var task = ProcessURLAsync(uri, client);
                    taskList.Add(task);
                }

                foreach (var task in taskList)
                {
                    var res = await task;
                }
            }
        }

        async Task<string> ProcessURLAsync(string url, HttpClient client)
        {
            try
            {
                var httpResponseMessage = await client.GetAsync(url);
                var result = await responseProcessor.Process(httpResponseMessage);

                DisplayResults(url, result);
                return result;
            }
            catch (Exception ex)
            {
                return "ERROR! There is an error while downloading the referat: " + ex.Message;
            }
        }

        private void DisplayResults(string url, string result)
        {
            Console.WriteLine(string.Format("\n{0,-58}\n{1,8}", url, result));
        }       
    }
}
