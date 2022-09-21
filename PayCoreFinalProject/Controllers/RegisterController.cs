using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.RabbitMQ.Abstract;
using PayCoreFinalProject.Service.RegisterService.Abstract;

namespace PayCoreFinalProject.Controllers;
[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    protected readonly IRegisterService _register;
    protected readonly IRabbitMQProducer _rabbitMqProducer;
    protected readonly IRabbitMQConsumer _rabbitMqConsumer;

    public RegisterController(IRegisterService register,IRabbitMQProducer rabbitMqProducer, IRabbitMQConsumer rabbitMqConsumer)
    {
        _register = register;
        _rabbitMqProducer = rabbitMqProducer;
        _rabbitMqConsumer = rabbitMqConsumer;

    }
    
    // Register operation
    [HttpPost()]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
    {
        // Send request to Service/RegisterService
        var result = _register.Register(userRegister);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        var user = GetCurrentUser();
        var email = new Email
        {
            EmailMessage = $"Welcome {userRegister.Name}, registration success. ",
            EmailAdress = userRegister.Email,
            EmailTitle = "Register Success",
            IsSent = true
        };
        
         _rabbitMqProducer.Produce(email);
        await _rabbitMqConsumer.Consume();
        return Ok(result);
    }
    private int GetCurrentUserId()
    {
        ClaimsPrincipal currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }    private IEnumerable<Claim> GetCurrentUser()
    {
        ClaimsPrincipal currentUser = User;
        return User.Claims;
    }
    
    
}
