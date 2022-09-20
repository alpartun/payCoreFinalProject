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
    protected readonly IProductService _productService;
    protected readonly IOfferService _offerService;
    //injection
    public ProductDetailsController(IProductService product,IOfferService offer)
    {
        _offerService = offer;
        _productService = product;
    }
    
    [HttpGet("GetAll")]
    public IActionResult GetAllProducts()
    {
        var results = _productService.GetAllProducts();

        return Ok(results);
    }
    
    [HttpGet("Get/{id}")]
    public IActionResult GetProduct(int id)
    {

        var result = _productService.GetById(id);

        return Ok(result);
    }
    // get offer able products
    [HttpGet("GetAllOfferableProducts")]
    public IActionResult OfferableProduct()
    {
        var currentUserId = GetCurrentUserId();
        var results = _productService.OfferableProducts(currentUserId);

        return Ok(results);
    }
    // get offer able products using specific category 
    [HttpGet("GetOfferableByCategory/{id:int}")]
    public IActionResult OfferableProductsByCategoryId(int id)
    {
        var currentUserId = GetCurrentUserId();
        var result = _productService.OfferableProductsByCategoryId(id, currentUserId);
        return Ok(result);
    }
        
    [HttpPost("Create")]
    public IActionResult Create(ProductRequest productRequest)
    {
        var currentUserId = GetCurrentUserId();
        var entity = _productService.Create(productRequest, currentUserId);
        return Ok(entity.Message);
    }
    
    [HttpPut("Edit")]
    public IActionResult Edit(int productId,ProductSpecialRequest productRequest)
    {
        var currentUserId = GetCurrentUserId();
        productRequest.Id = productId;
        var entity = _productService.Edit(productId,productRequest, currentUserId);
        return Ok(entity.Message);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        var entity = _productService.Remove(id);

        return Ok(entity);
    }
    // order product
    [HttpPost("Order")]
    public IActionResult Order(int productId)
    {
        var currentUserId = GetCurrentUserId();
        
        // order is an offer so this one goes to OfferService.
        var result = _offerService.Order(productId, currentUserId);

        return Ok(result.Message);
    }
    // offer to product
    [HttpPost("SendOffer")]
    public IActionResult SendOffer(int productId, double price)
    {
        var currentUserId = GetCurrentUserId();
        
        // SendOffer is in OfferService.
        var result = _offerService.SendOffer(productId, price, currentUserId);
        return Ok(result.Message);

    }
    
    // accept offer
    [HttpPost("Offer/Accept")]
    public IActionResult Accept(int offerId)
    {
        var currentUserId = GetCurrentUserId();
        // AcceptOffer is in OfferService.
        var result = _offerService.AcceptOffer(offerId, currentUserId);
        return Ok(result.Message);
    }
    // reject offer
    [HttpPost("Offer/Reject")]
    public IActionResult Reject(int offerId)
    {
        var currentUserId = GetCurrentUserId();
        
        // RejectOffer is in OfferService.
        var result = _offerService.RejectOffer(offerId, currentUserId);
        return Ok(result.Message);
    }

    //get current user id 
    private int GetCurrentUserId()
    {
        var currentUser = this.User;
        var currentUserId = Int32.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier).Value);
        return currentUserId;
    }


    
    
    
    
    
}
