using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Base.Offer;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.OfferService.Abstract;
using PayCoreFinalProject.Service.ProductService.Abstract;

namespace PayCoreFinalProject.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]/User")]
public class UserAccountDetailsController : ControllerBase
{
    protected readonly IOfferService _offerService;
    protected readonly IProductService _productService;

    public UserAccountDetailsController(IOfferService offerService, IProductService productService)
    {
        _offerService = offerService;
        _productService = productService;
    }
    [HttpGet("GetOffer/{id}")]
    public IActionResult GetSpesificOfferById(int id)
    {
        var result = _offerService.GetById(id);
        return Ok(result);
    }

    [HttpGet("GetAllOffers")]

    public IActionResult GetAllMyOffers()
    {
        var currentUserId = GetCurrentUserId();
        var result = _offerService.GetAllMyOffers(currentUserId);

        return Ok(result);
    }

    [HttpGet("GetAllOfferedProducts")]
    public IActionResult GetAllMyOfferedProducts()
    {
        var currentUserId = GetCurrentUserId();
        var result = _productService.GetAllMyProductOffers(currentUserId);

        return Ok(result);
        
    }
    [HttpGet("GetAllOrders")]
    public IActionResult OrdersGetAll()
    {
        var currentUserId = GetCurrentUserId();
        var entity = _offerService.OrdersGetAll(currentUserId);

        return Ok(entity);
    }
    [HttpGet("GetAllSoldProducts")]
    public IActionResult SoldProductsGetAll()
    {
        var currentUserId = GetCurrentUserId();
        var entity = _offerService.SoldProductsGetAll(currentUserId);

        return Ok(entity);
    }
    [HttpPut("Offer/Update")]
    public IActionResult UpdateOffer(OfferRequest offerRequest)
    {
        var currentUser = GetCurrentUserId();
        var entity = _offerService.UpdateOffer(offerRequest, currentUser);

        return Ok(entity.Message);
    }
    
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