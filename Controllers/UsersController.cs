using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;    
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Authorize( Roles = "Admin")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsersController : ControllerBase

    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController (IUserRepository userRepository, IMapper mapper)
        {
            
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsers ()
        {
            var users = _userRepository.GetUsers();
            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok (usersDto);

        }
     [HttpGet ("{userId}",Name ="GetUser")]
       
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status200OK)]
         public IActionResult GetUser(string userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
            {
                return NotFound($"el Usuario con el Id {userId} no existe");        
            }
              var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);      
        }
         [HttpPost(Name = "RegisterUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status500InternalServerError)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
         [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task <IActionResult> RegisterUser ([FromBody] CreateUserDto createUserDto)
        {
            if(createUserDto == null || !ModelState.IsValid)
            {
                return  BadRequest (ModelState);
            }
              if (string .IsNullOrWhiteSpace(createUserDto.Username))
            {
                
                return BadRequest("UserName es requerido");
            }
            if (!_userRepository.IsUniqueUser(createUserDto.Username))
            {
                return BadRequest("El usuario ya existe");

            }
            var result =  await _userRepository.Register(createUserDto);
            if(result == null)
            {
                return StatusCode (StatusCodes.Status500InternalServerError, "Error al regustrar el usuario");

            }
            return CreatedAtRoute ("GetUser",new {userId = result.Id}, result);
                
            
            
        }

        [HttpPost("Login",Name = "LoginUser")]
       [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
         [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status500InternalServerError)]
         [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task <IActionResult> LoginUser ([FromBody] UserLoginDto userLoginDto)
        {
            if(userLoginDto == null || !ModelState.IsValid)
            {
                return  BadRequest (ModelState);
            }
            
            var user =  await _userRepository.login(userLoginDto);
            if(user == null)
            {
                return Unauthorized();

            }
            return  Ok (user);
                
            
            
        }
        

    }
}
