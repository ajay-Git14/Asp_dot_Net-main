using ActionFlowDemo.Models;

namespace ActionFlowDemo.Services;

public sealed class InMemoryProductService : IProductService
{
    private readonly object _syncRoot = new();

    private readonly List<Product> _products =
    [
        new Product
        {
            Id = 1,
            Name = "Developer Laptop",
            Price = 75000,
            IsFeatured = true,
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        },
        new Product
        {
            Id = 2,
            Name = "Mechanical Keyboard",
            Price = 4500,
            IsFeatured = false,
            CreatedAt = DateTime.UtcNow.AddDays(-3)
        },
        new Product
        {
            Id = 3,
            Name = "27-inch Monitor",
            Price = 22000,
            IsFeatured = true,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        }
    ];

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        await SimulateIoAsync();

        lock (_syncRoot)
        {
            return _products
                .OrderBy(product => product.Id)
                .Select(Clone)
                .ToList();
        }
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await SimulateIoAsync();

        lock (_syncRoot)
        {
            var product = _products.FirstOrDefault(item => item.Id == id);
            return product is null ? null : Clone(product);
        }
    }

    public async Task<IReadOnlyList<Product>> GetFeaturedAsync()
    {
        await SimulateIoAsync();

        lock (_syncRoot)
        {
            return _products
                .Where(product => product.IsFeatured)
                .OrderBy(product => product.Name)
                .Select(Clone)
                .ToList();
        }
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string? term)
    {
        await SimulateIoAsync();
        term = term?.Trim();

        lock (_syncRoot)
        {
            IEnumerable<Product> query = _products;

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(product =>
                    product.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            return query
                .OrderBy(product => product.Name)
                .Select(Clone)
                .ToList();
        }
    }

    public async Task<Product> AddAsync(CreateProductViewModel model)
    {
        await SimulateIoAsync();

        lock (_syncRoot)
        {
            var nextId = _products.Count == 0
                ? 1
                : _products.Max(product => product.Id) + 1;

            var product = new Product
            {
                Id = nextId,
                Name = model.Name.Trim(),
                Price = model.Price,
                IsFeatured = model.IsFeatured,
                CreatedAt = DateTime.UtcNow
            };

            _products.Add(product);
            return Clone(product);
        }
    }

    private static Task SimulateIoAsync()
    {
        // This delay represents a database, web API, file, or cloud operation.
        return Task.Delay(250);
    }

    private static Product Clone(Product product)
    {
        return new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            IsFeatured = product.IsFeatured,
            CreatedAt = product.CreatedAt
        };
    }
}
