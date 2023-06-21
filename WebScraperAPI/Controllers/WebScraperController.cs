using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using WebScraperAPI.DTO;

namespace WebScraperAPI.Controllers
{
    [ApiController]
    public class WebScraperController : ControllerBase
    {
        [Route("GetProductsInfo")]
        [AcceptVerbs("GET")]
        public async Task<List<string>> GetProductsInfo()
        {
            List<string> ProductList = new List<string>();

            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"https://www.continente.pt/pesquisa/?q=ra%C3%A7%C3%A3o+c%C3%A3o");
            Stream stream = await result.Content.ReadAsStreamAsync();

            HtmlDocument doc = new HtmlDocument();            
            doc.Load(stream);

            //Organizar dados para saírem organizados:
            //{"Ração abc, 8.77",
            //"Racão dce, 17.74"}
            try
            {
                var productName = doc.DocumentNode.SelectNodes("//a[@class='pwc-tile--description col-tile--description']");
                foreach (var item in productName)
                {
                    var allowEncode = HtmlEntity.DeEntitize(item.InnerText);
                    ProductList.Add(allowEncode);
                }

                var productValue = doc.DocumentNode.SelectNodes("//span[@class='ct-price-formatted']");
                foreach (var item in productValue)
                {
                    var x = HtmlEntity.DeEntitize(item.InnerText);
                    ProductList.Add(x.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return ProductList;
        }
    }
}
