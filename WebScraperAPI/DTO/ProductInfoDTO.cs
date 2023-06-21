namespace WebScraperAPI.DTO
{
    public class ProductInfoDTO
    {

        public string ProductName { get; set; }
        public string ProductValue { get; set; }

        public ProductInfoDTO()
        {
            
        }

        public ProductInfoDTO(string name, string value)
        {
            ProductName = name;
            ProductValue = value;  
        }
    }
}
