using ReferatsDownloader.Interfaces;
using System;

namespace ReferatsDownloader
{
    public class UriBuilder: IUriBuilder
    {
        // For example https://yandex.ru/referats/?t=astronomy&s=59062
        private string downloadUrlFormat = "https://yandex.ru/referats/?t={0}&s={1}";

        private Random rand = new Random();

        public string Build(InputParameters input)
        {
            var seed = rand.Next(10000, 99999);
            return BuildUrl(input.ReferatCategory, seed);
        }

        private string BuildUrl(string category, int seed)
        {
            return string.Format(downloadUrlFormat, category, seed);
        }
    }
}
