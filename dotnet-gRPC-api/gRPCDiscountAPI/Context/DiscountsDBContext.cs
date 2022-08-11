using gRPCDiscountAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace gRPCDiscountAPI.Context
{
    public class DiscountsDBContext : DbContext, IDBContext
    {
        public DbSet<DiscountModel> Discounts { get; set; }
    }
}
