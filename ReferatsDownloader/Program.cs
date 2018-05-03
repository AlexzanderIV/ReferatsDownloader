using ReferatsDownloader.Enums;
using ReferatsDownloader.Services;
using System;
using System.Threading.Tasks;

namespace ReferatsDownloader
{
    class Program
    {
        private const int TopCategoriesAmount = 5;
        private static ReferatsService referatsService = new ReferatsService();

        public static async Task Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                var cmd = Menu();

                switch (cmd)
                {
                    case CommandType.Download:
                        var input = DownloadReferatsMenu();
                        await DownloadReferatAsync(input);
                        break;
                    case CommandType.ShowTopCategories:
                        await ShowTopCategoriesAsync();
                        break;
                    case CommandType.FindPhrase:
                        await FindPhraseAsync();
                        break;
                    case CommandType.Exit:
                        exit = true;
                        break;
                    case CommandType.Empty:
                    default:
                        Console.WriteLine("Please enter valid menu item number!");
                        break;
                }
            }
        }

        private static CommandType Menu()
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine($"{(int)CommandType.Download} - Download referats");
            Console.WriteLine($"{(int)CommandType.ShowTopCategories} - Show top {TopCategoriesAmount} categories by words amount");
            Console.WriteLine($"{(int)CommandType.FindPhrase} - Show referats count with phrase");
            Console.WriteLine($"{(int)CommandType.Exit} - Exit");

            var input = Console.ReadLine();

            var cmdNum = -1;
            if (Int32.TryParse(input, out cmdNum))
            {
                return (CommandType)cmdNum;
            }
            return CommandType.Empty;
        }


        private static InputParameters DownloadReferatsMenu()
        {
            Console.Clear();
            Console.Write("Referats amount to download: ");
            var referatsAmount = Console.ReadLine();

            //Console.WriteLine("Threads amount:");
            //var threadsAmount = Console.ReadLine();

            var input = new InputParameters
            {
                ReferatsAmount = Int32.Parse(referatsAmount),
                //ThreadsAmount = Int32.Parse(threadsAmount)
            };

            Console.WriteLine("Enter referat category:");
            while (string.IsNullOrEmpty(input.ReferatCategory))
                input.ReferatCategory = ChooseReferatCategory();

            return input;
        }

        async private static Task DownloadReferatAsync(InputParameters input)
        {
            Console.WriteLine();
            Console.WriteLine($"Start downloading {input.ReferatsAmount} referats on {input.ReferatCategory}...");

            HttpDownloader downloader = new HttpDownloader();
            await downloader.DownloadAsync(input);

            Console.WriteLine($"{Environment.NewLine}Finish downloading.");
            BackToMainMenuPrompt();
        }

        async private static Task ShowTopCategoriesAsync()
        {
            Console.Clear();
            Console.WriteLine($"Top {TopCategoriesAmount} categories by words amount in referats:");

            var topCategories = await referatsService.GetTopCategories(TopCategoriesAmount);
            
            var count = 1;
            foreach (var item in topCategories)
            {
                Console.WriteLine($"{count++} - {item.Key.Name} ({item.Value} words)");
            }

            BackToMainMenuPrompt();
        }

        async private static Task FindPhraseAsync()
        {
            Console.Clear();
            Console.Write("Enter phrase to search: ");
            var inputPhrase = Console.ReadLine();

            var referats = await referatsService.FindPhrase(inputPhrase);

            Console.WriteLine($"Referats amount with phrase '{inputPhrase}': {referats.Count}");

            BackToMainMenuPrompt();
        }


        private static string ChooseReferatCategory()
        {
            string[] categories = { "astronomy", "geology", "gyroscope", "literature", "marketing" , "mathematics",
            "music", "polit", "agrobiologia", "law", "psychology", "geography", "physics",
            "philosophy", "chemistry", "estetica" };

            Console.WriteLine($"{0} - Random.");
            for (int i = 0; i < categories.Length - 1; i++)
            {
                Console.WriteLine($"{i + 1} - {categories[i]}, ");
            }
            Console.WriteLine($"{categories.Length} - {categories[categories.Length - 1]}.");            

            var inputCategory = Console.ReadLine();
            var categoryNumber = 0;
            if (Int32.TryParse(inputCategory, out categoryNumber))
            {
                if (categoryNumber > 0)
                    return categories[categoryNumber - 1];
                else
                {
                    var rand = new Random();
                    return categories[rand.Next(0, categories.Length - 1)];
                }                    
            }
            return null;        
        }

        private static void BackToMainMenuPrompt()
        {
            Console.WriteLine($"{Environment.NewLine}Press any key to back to main menu...");
            Console.ReadKey();
        }
    }
}
