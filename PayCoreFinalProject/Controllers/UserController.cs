using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Service.UserService.Abstract;
using ISession = NHibernate.ISession;

namespace PayCoreFinalProject.Controllers;
[Authorize]

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    protected readonly IUserService _userService;

    public UserController(ISession session,IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _userService.GetAll();
        return Ok(result);
    }

    [HttpGet("{id}")]

    public IActionResult GetById(int id)
    {
        var result = _userService.GetById(id);
        return Ok(result);
    }
    

}