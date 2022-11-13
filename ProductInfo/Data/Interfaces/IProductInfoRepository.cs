using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Interfaces
{
    public interface IProductInfoRepository
    {
        // General
        void Add<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Products
        Task<Product> GetProductAsync(Guid productId);
        string GetProductCodeForOnlineChannel(int channelId, int productYear);
        string GetProductCodeForAllChannel(int channelId);
        bool ProductCodeExists(int channelId, string productCode);
    }
}
