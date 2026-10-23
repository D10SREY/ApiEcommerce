using System;
using System.Data.Common;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace ApiEcommerce.Repository;

public class ProductRepository : IProductRepository

{
 private readonly ApplicationDbContext _db;

 public ProductRepository (ApplicationDbContext db)
    {
        _db = db;
    }

    public bool BuyProduct(string name, int quantity)
    {
    var product = _db.products.FirstOrDefault(p=> p.Name.Contains(name));

    if(product == null)
        {
            return false;
        }
        if(product.Stock < quantity)
        {
            return false;
        }
        product.Stock = product.Stock - quantity;
        _db.products.Update(product);
        return Save();     
    }
    public bool UpdateProduct(Product product)
    {

    if(product == null)
        {
            return false;
        }
     
        product.UpdateDate = DateTime.Now;
        _db.products.Update(product);
        return Save();
    }

    public bool CreateProduct(Product product)
    {
           if (product == null)
        {
            return false;
        }
        product.CreationDate = DateTime.Now;
        _db.products.Add(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
           if (product == null)
        {
            return false;
        }
         _db.products.Remove(product);
        return Save();
        

    }

    public Product? GetProduct(int id)
    {
        if (id <= 0)
        {
            return null;
        }
        return _db.products.Include(p => p.category).FirstOrDefault(p => p.ProductId == id);
    }

    public ICollection<Product> GetProducts()
    {
       return _db.products.Include(p => p.category).OrderBy(p =>p.Name).ToList();
    }

    public ICollection<Product> GetProductsForCategory(int CategoryId)
    {
        if(CategoryId <= 0)

        {
            return  new List<Product>();
        }
      return  _db.products.Include(p => p.category                                                      ).Where( p => p.CategoryId  == CategoryId ).OrderBy(p => p.Name).ToList();
    }

    public bool ProductExists(int id)
    {
            if(id <= 0)

        {
            return  false;
        }
        return _db.products.Any(p=> p.ProductId == id);
    }

    public bool ProductExists(string name)
    {
         if(String.IsNullOrWhiteSpace(name))
        {
            return  false;
        }
        return _db.products.Any(p=> p.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool Save()
    {
        return _db.SaveChanges()>= 0;
    }

    public ICollection<Product> SearchProducts(string SearchTerm)
    {
       IQueryable<Product> query = _db.products;
       var searchTermLowerd = SearchTerm.ToLower().Trim();
       if(!String.IsNullOrEmpty(SearchTerm))
        {
          
           query = query.Include(p => p.category).Where
           (p => p.Name.ToLower().Contains(searchTermLowerd) || 
            p.Description.ToLower().Contains(searchTermLowerd));
                
        }
        return query.OrderBy(p=> p.Name).ToList();     
         }


    
}
