using Billing.Application.DTOs;
using System.Net.Http.Json;

namespace Billing.Application.Clients
{
    public class StockClient : IStockClient
    {
        private readonly HttpClient _httpClient;

        public StockClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> UpdateStockAsync(string productCode, int quantityChange)
        {
            var dto = new { QuantityChange = quantityChange };
            var response = await _httpClient.PutAsJsonAsync($"/api/Product/code/{productCode}/stock", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckProductExistsAsync(string productCode)
        {
            var response = await _httpClient.GetAsync($"/api/Product/code/{productCode}");
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetStockQuantityAsync(string productCode)
        {
            var response = await _httpClient.GetAsync($"/api/Product/code/{productCode}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Produto {productCode} não encontrado.");

            var product = await response.Content.ReadFromJsonAsync<StockProductDto>();
            return product?.StockQuantity ?? 0;
        }
    }
}
