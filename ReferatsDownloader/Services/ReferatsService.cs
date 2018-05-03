using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReferatsDownloader.DAL;
using ReferatsDownloader.Models;

namespace ReferatsDownloader.Services
{
    public class ReferatsService
    {
        /// <summary>
        /// Check if downloaded referat is not already in DB and add it to DB
        /// </summary>
        /// <param name="referat">Downloaded referat</param>
        /// <returns></returns>
        public async Task AddReferatAsync(Referat referat)
        {
            using (var repository = new ReferatsRepository())
            {
                // Check if we have already download such a referat (COMPARE BY TOPIC)
                var referats = await repository.GetAllReferats();
                if (referats.Any(r => r.Topic.Equals(referat.Topic, StringComparison.CurrentCultureIgnoreCase)))
                    return;

                await repository.Insert(referat);
            }
        }

        public async Task<List<Referat>> GetAllReferats()
        {
            using (var repository = new ReferatsRepository())
            {
                var referats = await repository.GetAllReferats();
                return referats;
            }
        }

        public async Task<List<Category>> GetAllCategories()
        {
            using (var repository = new ReferatsRepository())
            {
                var categories = await repository.GetAllCategories();
                return categories;
            }
        }

        public async Task<Dictionary<Category, int>> GetTopCategories(int topCategoriesAmount)
        {
            string[] separators = { " ", "\n", "\r", " ", ",", ".", "!", "?", ";", ":", "-" };
            
            var allCategories = await GetAllCategories();
            
            var topCategories = new Dictionary<Category, int>();

            foreach (var category in allCategories)
            {
                var wordsAmount = 0;
                var referats = category.Referats;
                foreach (var item in referats)
                {
                    wordsAmount += item.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;
                }
                topCategories.Add(category, wordsAmount);
            }

            var result = topCategories.OrderByDescending(x => x.Value).Take(topCategoriesAmount)
                .ToDictionary(x => x.Key, x => x.Value);
            return result;
        }

        public async Task<List<Referat>> FindPhrase(String searchString)
        {
            var referats = await GetAllReferats();

            if (referats == null || referats.Count == 0)
                return null;

            var result = new List<Referat>();
            
            foreach (var referat in referats)
            {
                if (referat.Text.IndexOf(searchString, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    result.Add(referat);
                }
            }

            return result;
        }
    }
}
