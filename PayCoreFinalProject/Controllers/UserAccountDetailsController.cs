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
    [HttpGet("GetOffer/{id}")]
    public IActionResult GetOfferById(int id)
    {
        var result = _offerService.GetById(id);
        return Ok(result);
    }

    // all offers that user made
    [HttpGet("GetAllOffers")]

    public IActionResult GetAllMyOffers()
    {
        //get current id method and returns userId claim
        var currentUserId = GetCurrentUserId();
        var result = _offerService.GetAllMyOffers(currentUserId);

        return Ok(result);
    }
    //get all offered products for that user
    [HttpGet("GetAllOfferedProducts")]
    public IActionResult GetAllMyOfferedProducts()
    {
        var currentUserId = GetCurrentUserId();
        var result = _productService.GetAllMyProductOffers(currentUserId);

        return Ok(result);
        
    }
    // get all products that user bought
    [HttpGet("GetAllOrders")]
    public IActionResult OrdersGetAll()
    {
        var currentUserId = GetCurrentUserId();
        var entity = _offerService.OrdersGetAll(currentUserId);

        return Ok(entity);
    }
    // get all products that user sold
    [HttpGet("GetAllSoldProducts")]
    public IActionResult SoldProductsGetAll()
    {
        var currentUserId = GetCurrentUserId();
        var entity = _offerService.SoldProductsGetAll(currentUserId);

        return Ok(entity);
    }
    
    //user offer update 
    [HttpPut("Offer/Update")]
    public IActionResult UpdateOffer(OfferRequest offerRequest)
    {
        var currentUser = GetCurrentUserId();
        var entity = _offerService.UpdateOffer(offerRequest, currentUser);

        return Ok(entity.Message);
    }
    // user offer delete
    [HttpDelete("Offer/Delete")]
    public IActionResult DeleteOffer(int offerId)
    {
        var result = _offerService.Remove(offerId);

        return Ok(result.Message);
    }
    
    private int GetCurrentUserId()
    {
        ClaimsPrincipal currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }
    
}