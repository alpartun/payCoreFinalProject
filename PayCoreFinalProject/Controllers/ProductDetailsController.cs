using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Service.OfferService.Abstract;
using PayCoreFinalProject.Service.ProductService.Abstract;
using System.Security.Claims;
using PayCoreFinalProject.Base.Product;


namespace PayCoreFinalProject.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/Product")]
public class ProductDetailsController : Controller
{
    protected readonly IProductService _product;
    protected readonly IOfferService _offer;
    
    public ProductDetailsController(IProductService product,IOfferService offer)
    {
        _offer = offer;
        _product = product;
    }
    
    [HttpGet("GetAll")]
    public IActionResult GetAllProducts()
    {
        var results = _product.GetAllProducts();

        return Ok(results);
    }
    
    [HttpGet("Get/{id}")]
    public IActionResult GetProduct(int id)
    {
        var result = _product.GetById(id);

        return Ok(result);
    }
    
    [HttpGet("GetAllOfferableProducts")]
    public IActionResult OfferableProduct()
    {
        var currentUserId = GetCurrentUserId();
        var results = _product.OfferableProducts(currentUserId);

        return Ok(results);
    }
    [HttpGet("GetOfferableByCategory/{id:int}")]
    public IActionResult OfferableProductsByCategoryId(int id)
    {
        var currentUserId = GetCurrentUserId();
        var result = _product.OfferableProductsByCategoryId(id, currentUserId);
        return Ok(result);
    }
        
    [HttpPost("Create")]
    public IActionResult Create(ProductRequest productRequest)
    {
        var currentUserId = GetCurrentUserId();
        var entity = _product.Create(productRequest, currentUserId);
        return Ok(entity.Message);
    }
    
    [HttpPut("Edit")]
    public IActionResult Edit(int productId,ProductSpecialRequest productRequest)
    {
        var currentUserId = GetCurrentUserId();
        productRequest.Id = productId;
        var entity = _product.Edit(productId,productRequest, currentUserId);
        return Ok(entity.Message);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        var entity = _product.Remove(id);

        return Ok(entity);
    }
    
    [HttpPost("Order")]
    public IActionResult Order(int productId)
    {
        var currentUserId = GetCurrentUserId();
        var result = _offer.Order(productId, currentUserId);

        return Ok(result.Message);
    }

    [HttpPost("SendOffer")]
    public IActionResult SendOffer(int productId, double price)
    {
        var currentUserId = GetCurrentUserId();
        var result = _offer.SendOffer(productId, price, currentUserId);
        return Ok(result.Message);

    }
    [HttpPost("Offer/Accept")]
    public IActionResult Accept(int offerId)
    {
        var currentUserId = GetCurrentUserId();
        var result = _offer.AcceptOffer(offerId, currentUserId);
        return Ok(result.Message);
    }
    [HttpPost("Offer/Reject")]
    public IActionResult Reject(int offerId)
    {
        var currentUserId = GetCurrentUserId();
        var result = _offer.RejectOffer(offerId, currentUserId);
        return Ok(result.Message);
    }

    private int GetCurrentUserId()
    {
        ClaimsPrincipal currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }


    
    
    
    
    
}
