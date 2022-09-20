using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.RegisterService.Abstract;

namespace PayCoreFinalProject.Controllers;
[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    protected readonly IRegisterService _register;
    public RegisterController(IRegisterService register)
    {
        _register = register;

    }
    
    // Register operation
    [HttpPost()]
    public IActionResult Register([FromBody] UserRegisterDto userRegister)
    {
        // Send request to Service/RegisterService
        var result = _register.Register(userRegister);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }
    
}