using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)

        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            var categoriesDto = new List <CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDto>(category));

            } 
            return Ok(categoriesDto);
        }
        [HttpGet ("{id:int}",Name ="GetCategory")]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public IActionResult GetCategory(int id )
        {
            var category = _categoryRepository.GetCategory(id);
            if (category == null)
            {
                return NotFound($"La categoria con el id {id} no existe");
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
          
        }
         [HttpPost]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status201Created)]
              [ProducesResponseType(StatusCodes.Status500InternalServerError)]
         
         public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto )
        {
          if (createCategoryDto == null )
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError("Custon Erro", "la categoria ya existe");
                return BadRequest(ModelState);
            }
            var  Category = _mapper.Map<Category>(createCategoryDto);
            if(!_categoryRepository.CreateCategory(Category))

            {
                ModelState.AddModelError("Custon Error",$"Algo salio mal al gurdar el registro{Category.Name}");
                return StatusCode(500,ModelState);                
            }
            return CreatedAtRoute("GetCategory",new {id =Category.Id},Category);
        }
        [HttpPatch("{id:int}",Name ="UpdateCategory")]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
         
         public IActionResult UpdateCategory(int id,[FromBody] CreateCategoryDto updateCategoryDto )
        {

            if(!_categoryRepository.CategoryExists(id))
            {
            return NotFound($"La categoria con el id {id} no existe");
            }
          if (updateCategoryDto == null )
            {
                return BadRequest(ModelState);
            }
            if (_categoryRepository.CategoryExists(updateCategoryDto.Name))
            {
                ModelState.AddModelError("CustonError", "la categoria ya existe");
                return BadRequest(ModelState);
            }
            var  Category = _mapper.Map<Category>(updateCategoryDto);
            Category.Id =id;
            if(!_categoryRepository.UpdateCategory(Category))

            {
                ModelState.AddModelError("Custon Error",$"Algo salio mal al actualizar el registro{Category.Name}");
                return StatusCode(500,ModelState);                
            }
            return NoContent();
        }
        [HttpDelete("{id:int}",Name ="DeleteCategory")]

        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);
        if (category == null)
            {
            return NotFound($"La categoria con el id {id} no existe");
            }
            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("CustomError",$"Algo salio mal al eliminar el registro {category.Name}");
                return StatusCode(500,ModelState);                
            }
              return NoContent(); 
        }
                                     

            
            
        
    }

}
