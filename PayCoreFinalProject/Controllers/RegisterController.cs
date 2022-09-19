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
    [HttpPost("Register")]
    public IActionResult Register([FromBody] UserRegisterDto userRegister)
    {
        var result = _register.Register(userRegister);
        return Ok(result);
    }
    
}