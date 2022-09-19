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

    [HttpGet("Category/GetAll")]
    public IActionResult GetAll()
    {
        var results = _category.GetAllCategories();

        return Ok(results);
    }
    [HttpGet("Category/{id}")]

    public IActionResult GetById(int id)
    {
        var result = _category.GetById(id);

        return Ok(result);
    }

    [HttpPost("Category/Create")]
    public IActionResult Create(CategoryRequest categoryRequest)
    {
        var result = _category.Create(categoryRequest);

        return Ok(result.Message);

    }
    [HttpPut("Category/Edit")]
    public IActionResult Update(int id, CategoryRequest categoryRequest)
    {
        var entity = _category.Edit(id, categoryRequest);
        return Ok(entity);
    }
    [HttpDelete("Category/Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var currentUserId = GetCurrentUserId();
        var entity = _category.Delete(id,currentUserId);

        return Ok(entity);
    }
    private int GetCurrentUserId()
    {
        ClaimsPrincipal currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }

    
}