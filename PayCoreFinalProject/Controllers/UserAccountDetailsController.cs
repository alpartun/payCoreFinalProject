using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Base.Offer;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.OfferService.Abstract;
using PayCoreFinalProject.Service.ProductService.Abstract;

namespace PayCoreFinalProject.Controllers;

[Authorize] // Authorize attribute for controller, if user not authorize then can not be able to access that controller methods
[ApiController]
[Route("api/[controller]/User")]
public class UserAccountDetailsController : ControllerBase
{
    protected readonly IOfferService _offerService;

    protected readonly IProductService _productService;

    // injection
    public UserAccountDetailsController(IOfferService offerService, IProductService productService)
    {
        _offerService = offerService;
        _productService = productService;
    }

    // Get Users spesific offer using id
    [HttpGet("Offer/{id}")]
    public IActionResult GetOfferById(int id)
    {
        var result = _offerService.GetById(id);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    // all offers that user made
    [HttpGet("AllOffers")]
    public IActionResult GetAllMyOffers()
    {
        //get current id method and returns userId claim
        var currentUserId = GetCurrentUserId();
        var result = _offerService.GetAllMyOffers(currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    //get all offered products for that user
    [HttpGet("AllOfferedProducts")]
    public IActionResult GetAllMyOfferedProducts()
    {
        var currentUserId = GetCurrentUserId();
        var result = _productService.GetAllMyProductOffers(currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }

    // get all products that user bought
    [HttpGet("AllOrders")]
    public IActionResult OrdersGetAll()
    {
        var currentUserId = GetCurrentUserId();
        var entity = _offerService.OrdersGetAll(currentUserId);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }

        return Ok(entity);
    }

    // get all products that user sold
    [HttpGet("AllSoldProducts")]
    public IActionResult SoldProductsGetAll()
    {
        var currentUserId = GetCurrentUserId();
        var entity = _offerService.SoldProductsGetAll(currentUserId);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }

        return Ok(entity);
    }

    //user offer update 
    [HttpPut("Offer")]
    public IActionResult UpdateOffer(OfferRequest offerRequest)
    {
        var currentUser = GetCurrentUserId();
        var entity = _offerService.UpdateOffer(offerRequest, currentUser);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }

        return Ok(entity.Message);
    }

    // user offer delete
    [HttpDelete("Offer")]
    public IActionResult DeleteOffer(int offerId)
    {
        var result = _offerService.Remove(offerId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
    }

    private int GetCurrentUserId()
    {
        ClaimsPrincipal currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }
}