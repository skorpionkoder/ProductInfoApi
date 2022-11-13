using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Interfaces
{
    public interface IColourInfoRepository
    {
        Task<Colour> GetColoursAsync(Guid colourId);
    }
}
