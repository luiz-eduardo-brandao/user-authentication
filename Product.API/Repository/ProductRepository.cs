using Product.API.Data;

namespace Product.API.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<ProductEntity> _products;

        public ProductRepository()
        {
            _products = new List<ProductEntity>();
        }
        
        public async Task<List<ProductEntity>> GetAllAsync() 
        {
            return _products.ToList();
        }

        public async Task<ProductEntity?> GetByIdAsync(int id) 
        {
            return _products.ToList().FirstOrDefault(p => p.Id == id);
        }

        public async Task AddProductAsync(ProductEntity product) 
        {
            _products.Add(product);
        }

        public async Task UpdateProductAsync(ProductEntity product) 
        {
            var result = _products.ToList().FirstOrDefault(p => p.Id == product.Id);

            result = product;
        }
    }
}