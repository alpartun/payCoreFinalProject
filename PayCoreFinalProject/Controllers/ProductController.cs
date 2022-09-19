using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Base.Product;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.ProductService.Abstract;

namespace PayCoreFinalProject.Controllers;
[NonController]
[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    protected readonly IProductService _product;
    protected readonly IMapper _mapper;
    public ProductController(IProductService product, IMapper mapper)
    {
        _product = product;
        _mapper = mapper;
    }




    
    private int GetCurrentUserId()
    {
        var currentUser = User;

        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }

    /*[HttpPut]
    public IActionResult Update(int id, ProductDto category)
    {
        var entity = _product.Update(id, category);
        return Ok(entity);
    }*/


    
}