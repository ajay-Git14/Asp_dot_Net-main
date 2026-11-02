using ActionFlowDemo.Models;

namespace ActionFlowDemo.Services;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetAllAsync();

    Task<Product?> GetByIdAsync(int id);

    Task<IReadOnlyList<Product>> GetFeaturedAsync();

    Task<IReadOnlyList<Product>> SearchAsync(string? term);

    Task<Product> AddAsync(CreateProductViewModel model);
}
