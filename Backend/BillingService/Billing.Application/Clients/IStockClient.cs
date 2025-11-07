namespace Billing.Application.Clients
{
    public interface IStockClient
    {
        Task<bool> UpdateStockAsync(string productCode, int quantityChange);
        Task<bool> CheckProductExistsAsync(string productCode);
        Task<int> GetStockQuantityAsync(string productCode);
    }
}
