using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;   
        }

        public void Update(Product obj)
        {
           //_db.Products.Update(obj);
           var productToUpdate = _db.Products.FirstOrDefault( p => p.Id == obj.Id );
            if ( productToUpdate != null )
            {
                productToUpdate.Id = obj.Id;
                productToUpdate.Title= obj.Title;
                productToUpdate.Description= obj.Description;
                productToUpdate.CategoryId= obj.CategoryId;
                productToUpdate.CoverTypeId= obj.CoverTypeId;
                productToUpdate.Author= obj.Author;
                productToUpdate.ISBN= obj.ISBN; 
                productToUpdate.ListPrice = obj.ListPrice;
                productToUpdate.Price = obj.Price;
                productToUpdate.Price50 = obj.Price50;  
                productToUpdate.Price100= obj.Price100;

                if (productToUpdate.ImageURL != null)
                {
                    productToUpdate.ImageURL = obj.ImageURL;
                }

            }

        }
    }
}
