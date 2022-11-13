using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Interfaces
{
    public interface ISizeScaleInfoRepository
    {
        Task<ICollection<SizeScale>> GetSizeScalesAsync(Guid sizeId);
    }
}
