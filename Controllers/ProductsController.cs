using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase 
    {
        

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;     
        private readonly IMapper _mapper;
        public ProductsController(IProductRepository productRepository, IMapper mapper,ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _mapper = mapper; 
            
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


         public IActionResult GetProduct()
        {
            var products = _productRepository.GetProducts();
        
           var productsDto = _mapper.Map<List<ProductDto>>(products);  
            return Ok(productsDto);
        }
         [HttpGet ("{productId:int}",Name ="GetProduct")]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public IActionResult GetProduct(int productId)
        {
            var product = _productRepository.GetProduct(productId);
            if (product == null)
            {
                return NotFound($"el producto con el Id {productId} no existe");        
            }
              var productyDto = _mapper.Map<ProductDto>(product);
            return Ok(productyDto);      
        }

        [HttpPost]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status201Created)]
              [ProducesResponseType(StatusCodes.Status500InternalServerError)]
         
         public IActionResult CreateProduct([FromBody] CreateProductDto CreateProductDto )
        {
          if (CreateProductDto == null )
            {
                return BadRequest(ModelState);
            }
            if (_productRepository.ProductExists(CreateProductDto.Name))
            {
                ModelState.AddModelError("Custon Error", "el producto ya existe");
                return BadRequest(ModelState);
            }
            if(!_categoryRepository.CategoryExists(CreateProductDto.CategoryId))
            {
                ModelState.AddModelError("Custon Error", $"la categoria con el {CreateProductDto.CategoryId} no existe");
                   return BadRequest(ModelState);
            }
            var  Product = _mapper.Map<Product>(CreateProductDto);
            if(!_productRepository.CreateProduct(Product))

            {
                ModelState.AddModelError("Custon Error",$"Algo salio mal al gurdar el registro{Product.Name}");
                return StatusCode(500,ModelState);                
            }
            var createdProduct = _productRepository.GetProduct(Product.ProductId);
            var productDto = _mapper.Map<ProductDto>(createdProduct );
            return CreatedAtRoute("GetProduct",new { productId =Product.ProductId},productDto);
        }

        [HttpGet ("searchByCategory/{categoryId:int}",Name ="GetProductsForCategory")]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public IActionResult GetProductsForCategory(int categoryId)
        {
             var products = _productRepository.GetProductsForCategory(categoryId);
            if(products.Count == 0)
            {
                return NotFound($"Los productos con la categoria {categoryId} no existem");
            }
             
              var productsDto = _mapper.Map<List<ProductDto>>(products);  
            return Ok(productsDto);
    }
    [HttpGet ("searchProductByNameDesccription/{SearchTerm}",Name ="SearchProducts")]


        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]

         public IActionResult SearchProducts (string SearchTerm)

        {
            var products = _productRepository.SearchProducts(SearchTerm);
            if(products.Count == 0)
            {
                return NotFound($"Los productos con el nombre o descripcion'{SearchTerm}' no existen");
            }
            
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return Ok (productsDto);
        }
        [HttpPatch ("buyProduct/{name}/{quantity:int}",Name ="BuyProduct")]

        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]

         public IActionResult BuyProduct (string name, int quantity)
        {
            if(string.IsNullOrEmpty(name) || quantity <=0)
            {   
                return BadRequest("El nombre del producto o la cantidad no son validas");
            }
            var foundProduct = _productRepository.ProductExists(name );
            if (!foundProduct)
            {
                return NotFound($"El producto con el nombre{name} no existe");
            }
            if(!_productRepository.BuyProduct(name,quantity))
            {  
                ModelState.AddModelError("Custom error",$"No se pudo comprar el producto{name} o la cantidad solicitada es mayor al stock disponible");
                      
            }           
              var units = quantity == 1? "Unidad" : "Unidades";
            return Ok($"Se compro la cantidad {quantity} {units} del producto'{name}'");
        }

        [HttpPut("updateProduct/{productId:int}",Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]

         public IActionResult UpdateProduct(int productId ,[FromBody] UpdateProductDto updateProductDto)
            {
                if( updateProductDto == null)
                {
                return BadRequest ("Necesita enviar un valor valido");
                }
                if (productId != updateProductDto.ProductId)
                {
                ModelState.AddModelError("CustomError", $"El Id del producto '{productId}' no coincide con el Id del cuerpo de la petición.");
                return BadRequest(ModelState);
                }
                var foundProduct = _productRepository.ProductExists(productId );
                if(! foundProduct)
                {
                return NotFound($"El producto con el iD {productId} no existe");
                 }   
                var product = _mapper.Map<Product>(updateProductDto);
                 _productRepository.UpdateProduct(product);
                return NoContent();
        }

        [HttpDelete ("{productId:int}",Name ="DeleteProduct")]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status204NoContent)]
         public IActionResult DeleteProduct(int productId)
        {
             if (productId  == 0)
            {
                return BadRequest(ModelState);       
            }
            var product = _productRepository.GetProduct(productId);
            if (product == null)
            {
                return NotFound($"el producto con el id {productId} no existe");       
            }

             if (!_productRepository.DeleteProduct(product))
                {
                ModelState.AddModelError("CustomError", $"Algo salio mal al eliminar el registro{product.Name }");
                return StatusCode(500, ModelState);
                }
        
            return NoContent();      
        }

    }
}   
