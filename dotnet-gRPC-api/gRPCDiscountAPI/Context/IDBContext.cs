using gRPCDiscountAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace gRPCDiscountAPI.Context;

public interface IDBContext
{
    DbSet<T> Set<T>() where T : class;

    DbSet<DiscountModel> Discounts { get; set; }
}
