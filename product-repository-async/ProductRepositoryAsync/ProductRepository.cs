using System.Globalization;
using System.Net.Mail;

namespace ProductRepositoryAsync;

/// <summary>
/// Represents a product storage service and provides a set of methods for managing the list of products.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly string productCollectionName;
    private readonly IDatabase database;

    public ProductRepository(string productCollectionName, IDatabase database)
    {
        this.productCollectionName = productCollectionName;
        this.database = database;
    }

    public async Task<int> AddProductAsync(Product product)
    {
        // Validate product data

        // Prepare the data for the database
        var data = new Dictionary<string, string>
        {
            { "name", product.Name },
            { "category", product.Category },
            { "price", product.UnitPrice.ToString(CultureInfo.InvariantCulture) },
            { "in-stock", product.UnitsInStock.ToString(CultureInfo.InvariantCulture) },
            { "discontinued", product.Discontinued.ToString() }
        };

        // Add product to the database
        OperationResult result = await this.database.InsertCollectionElementAsync(this.productCollectionName, product.Id, data);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        // Return a status or product ID
        // Assuming that the AddCollectionElementAsync returns an int indicating the result of the operation
        return product.Id; // Return the ID or any other relevant result
    }


    public async Task<Product> GetProductAsync(int productId)
    {

        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new ProductNotFoundException();
        }

        result = await this.database.GetCollectionElementAsync(this.productCollectionName, productId, out IDictionary<string, string> data);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }


        return new Product
        {
            Id = productId,
            Name = data["name"],
            Category = data["category"],
            UnitPrice = decimal.Parse(data["price"], CultureInfo.InvariantCulture),
            UnitsInStock = int.Parse(data["in-stock"], CultureInfo.InvariantCulture),
            Discontinued = bool.Parse(data["discontinued"]),
        };


    }

    public async Task RemoveProductAsync(int productId)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }
        else if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new ProductNotFoundException();
        }

        await this.database.DeleteCollectionElementAsync(this.productCollectionName, productId);
    }

    public Task UpdateProductAsync(Product product)
    {
        // TODO Implement the method to update a product int the repository.
        throw new NotImplementedException();
    }
}
