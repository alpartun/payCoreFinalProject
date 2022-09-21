using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Base.Category;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.CategoryService.Abstract;

namespace PayCoreFinalProject.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class CategoryDetailsController : ControllerBase
{
    protected readonly ICategoryService _category;

    public CategoryDetailsController(ICategoryService category)
    {
        _category = category;

    }
    // get all categories
    [HttpGet("Category/All")]
    public IActionResult GetAll()
    {
        var results = _category.GetAllCategories();
        if (results.Success == false)
        {
            return BadRequest(results.Message);
        }

        return Ok(results);
    }
    // get specific category with categoryId
    [HttpGet("Category/{id}")]

    public IActionResult GetById(int id)
    {
        var result = _category.GetById(id);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
    // create category
    [HttpPost("Category")]
    public IActionResult Create(CategoryRequest categoryRequest)
    {
        var result = _category.Create(categoryRequest);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);

    }
    // edit category
    [HttpPut("Category")]
    public IActionResult Update(int id, CategoryRequest categoryRequest)
    {
        var entity = _category.Edit(id, categoryRequest);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }
        return Ok(entity);
    }
    // delete category
    [HttpDelete("Category/{id}")]
    public IActionResult Delete(int id)
    {
        var currentUserId = GetCurrentUserId();
        var entity = _category.Delete(id,currentUserId);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }

        return Ok(entity);
    }
    private int GetCurrentUserId()
    {
        ClaimsPrincipal currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }

    
}