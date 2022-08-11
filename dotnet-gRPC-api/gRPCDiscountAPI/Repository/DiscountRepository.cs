using gRPCDiscountAPI.Context;
using gRPCDiscountAPI.Domain;
using gRPCDiscountAPI.Exceptions;
using gRPCDiscountAPI.Protos;
using Microsoft.EntityFrameworkCore;

namespace gRPCDiscountAPI.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDBContext _context;

        public DiscountRepository(IDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DiscountModel> CreateDiscount(DiscountModel discount)
        {
            _context.Discounts.Add(discount);
            await ((DbContext)_context).SaveChangesAsync(default);
            return await GetDiscount(discount.ProductName);
        }

        public async Task<DiscountModel> GetDiscount(string productName)
        {
            return await _context.Discounts.Where(d => d.ProductName == productName).FirstAsync(default);
        }

        public async Task<bool> RemoveDiscount(string productName)
        {
            _context.Discounts.Remove(new DiscountModel { ProductName = productName });
            return (await ((DbContext)_context).SaveChangesAsync(default)) > 0;

        }

        public async Task<bool> UpdateDiscount(DiscountModel discount)
        {
            try
            {
                await _context.Discounts.Where(d => d.ProductName.Equals(discount.ProductName)).FirstAsync(default);
                _context.Discounts.Update(discount);
            }
            catch (Exception ex)
            {
                _context.Discounts.Add(discount);
                throw new NotFoundException(discount.ProductName, ex);
            }

            return await((DbContext)_context).SaveChangesAsync(default) > 0;
        }
    }
}
