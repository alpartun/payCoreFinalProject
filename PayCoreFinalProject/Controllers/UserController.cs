using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Service.UserService.Abstract;
using ISession = NHibernate.ISession;

namespace PayCoreFinalProject.Controllers;

[Authorize]
[NonController]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    // This controller for admin and we have no roles right now.
    protected readonly IUserService _userService;

    public UserController(ISession session, IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _userService.GetAll();
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var result = _userService.GetById(id);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
}