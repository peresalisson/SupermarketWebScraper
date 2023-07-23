
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using WebScraperAPI.DTO;

namespace ContinenteScrapingController.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("GetProductsInfo")]
    public class ContinenteScrapingController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductInfoDTO>>> Get()
        {
            try
            {
                string url = "https://www.continente.pt/pesquisa/?q=ra%C3%A7%C3%A3o+c%C3%A3o";

                List<ProductInfoDTO> scrapedData = await ScrapeWebsite(url);

                return Ok(scrapedData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        private async Task<List<ProductInfoDTO>> ScrapeWebsite(string url)
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            List<ProductInfoDTO> scrapedData = new List<ProductInfoDTO>();

            var productName = doc.DocumentNode.SelectNodes("//a[@class='pwc-tile--description col-tile--description']");
            var productValue = doc.DocumentNode.SelectNodes("//span[@class='ct-price-formatted']");

            if (productName != null && productValue!= null && productName.Count == productValue.Count)
            {
                for (int i = 0; i < productName.Count; i++)
                {
                    string name = productName[i].InnerText.Trim();
                    string value = productValue[i].InnerText.Trim();

                    scrapedData.Add(new ProductInfoDTO { ProductName = HtmlEntity.DeEntitize(name), ProductValue = HtmlEntity.DeEntitize(value)});
                }
            }

            return scrapedData;
        }
    }
}

