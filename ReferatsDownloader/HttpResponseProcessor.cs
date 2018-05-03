using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using ReferatsDownloader.Interfaces;
using ReferatsDownloader.Models;
using ReferatsDownloader.Services;

namespace ReferatsDownloader
{
    public class HttpResponseProcessor: IHttpResponseProcessor
    {
        private ReferatsService service = new ReferatsService();

        async public Task<string> Process(HttpResponseMessage responseMessage)
        {
            try
            {
                var stream = await responseMessage.Content.ReadAsStreamAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.Load(stream);

                var referat = CreateReferatFromHtml(doc);
                if (referat == null)
                    return string.Empty;

                await service.AddReferatAsync(referat);

                var sb = new StringBuilder();
                sb.Append(referat.Category.Name).Append(Environment.NewLine);
                sb.Append(referat.Topic).Append(Environment.NewLine);
                sb.Append(referat.Text);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "ERROR! There is an error while saving downloaded referat to database: " + ex.Message;
            }
        }

        private Referat CreateReferatFromHtml(HtmlDocument htmlDoc)
        {
            var referatNodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(concat(' ', normalize-space(@class), ' '), 'referats__text')]")
                .Descendants("#text");

            if (referatNodes == null)
                return null;

            var referat = new Referat();

            var categories = service.GetAllCategories();
            
            var referatText = new StringBuilder();
            foreach (var node in referatNodes)
            {
                var parentNodeName = node.ParentNode.Name;
                switch (parentNodeName)
                {
                    case "div":
                        {
                            var categoryName = node.InnerHtml.Trim();
                            var category = categories.Result
                                .FirstOrDefault(x => x.Name != null && x.Name.Equals(categoryName, StringComparison.CurrentCultureIgnoreCase));
                            if (category == null)
                            {
                                referat.Category = new Category() { Name = categoryName, CreatedDate = DateTime.UtcNow };
                            }
                            else
                            {
                                referat.Category = category;
                                referat.CategoryId = category.Id;
                            }
                        }
                        break;
                    case "strong":
                        referat.Topic = node.InnerHtml;
                        break;
                    case "p":
                        referatText.Append($"{node.InnerText}{Environment.NewLine}");
                        break;
                    default:
                        break;
                }
            }

            referat.Text = referatText.ToString();

            return referat;
        }
    }
}
