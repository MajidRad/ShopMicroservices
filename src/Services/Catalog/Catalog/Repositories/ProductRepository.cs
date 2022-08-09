using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;

    public ProductRepository(ICatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }
    public async Task CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        var result = await _catalogContext.Products.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        var products = await _catalogContext.Products
            .Find(p => true)
            .ToListAsync();
        return products;
    }

    public async Task<Product> GetProduct(string id)
    {
        var product = await _catalogContext.Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
        return product;
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>
            .Filter
            .Eq(p => p.Category, categoryName);

        var products = await _catalogContext.Products
            .Find(filter)
            .ToListAsync();
        return products;
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

        var products = await _catalogContext.Products
            .Find(filter)
            .ToListAsync();
        return products;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var res = await _catalogContext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, product);
        return res.IsAcknowledged && res.ModifiedCount > 0;
    }
}
