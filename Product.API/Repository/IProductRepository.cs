using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product.API.Data;

namespace Product.API.Repository
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(int id);
        Task AddProductAsync(ProductEntity product);
    }
}