using System;
using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.IRepository;

public interface IProductRepository
{
ICollection<Product> GetProducts();
 Product? GetProduct (int id);
ICollection<Product> GetProductsForCategory(int CategoryId);
ICollection<Product> SearchProducts(string SearchTerm);

bool BuyProduct (string name, int quantity);
bool ProductExists(int id);
bool ProductExists(string name);
bool CreateProduct(Product product);
bool UpdateProduct(Product product);
bool DeleteProduct(Product product);
bool Save();

}