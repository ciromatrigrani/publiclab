using gRPCDiscountAPI.Domain;
using gRPCDiscountAPI.Protos;

namespace gRPCDiscountAPI.Repository
{
    public interface IDiscountRepository
    {
        Task<DiscountModel> GetDiscount(string productName);
        Task<DiscountModel> CreateDiscount(DiscountModel request);
        Task<bool> RemoveDiscount(string productName);
        Task<bool> UpdateDiscount(DiscountModel request);
    }
}
