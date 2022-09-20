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
        if (results.Success == false)
        {
            return BadRequest(results.Message);
        }

        return Ok(results);
    }
    
    [HttpGet("Get/{id}")]
    public IActionResult GetProduct(int id)
    {

        var result = _productService.GetById(id);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
    // get offer able products
    [HttpGet("GetAllOfferableProducts")]
    public IActionResult OfferableProduct()
    {
        var currentUserId = GetCurrentUserId();
        var results = _productService.OfferableProducts(currentUserId);
        if (results.Success == false)
        {
            return BadRequest(results.Message);
        }

        return Ok(results);
    }
    // get offer able products using specific category 
    [HttpGet("GetOfferableByCategory/{id:int}")]
    public IActionResult OfferableProductsByCategoryId(int id)
    {
        var currentUserId = GetCurrentUserId();
        var result = _productService.OfferableProductsByCategoryId(id, currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }
        return Ok(result);
    }
        
    [HttpPost("Create")]
    public IActionResult Create(ProductRequest productRequest)
    {
        var currentUserId = GetCurrentUserId();
        var entity = _productService.Create(productRequest, currentUserId);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }
        return Ok(entity.Message);
    }
    
    [HttpPut("Edit")]
    public IActionResult Edit(int productId,ProductSpecialRequest productRequest)
    {
        var currentUserId = GetCurrentUserId();
        productRequest.Id = productId;
        var entity = _productService.Edit(productId,productRequest, currentUserId);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }
        return Ok(entity.Message);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        var entity = _productService.Remove(id);
        if (entity.Success == false)
        {
            return BadRequest(entity.Message);
        }

        return Ok(entity);
    }
    // order product
    [HttpPost("Order")]
    public IActionResult Order(int productId)
    {
        var currentUserId = GetCurrentUserId();
        
        // order is an offer so this one goes to OfferService.
        var result = _offerService.Order(productId, currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
    }
    // offer to product
    [HttpPost("SendOffer")]
    public IActionResult SendOffer(int productId, double price)
    {
        var currentUserId = GetCurrentUserId();
        
        // SendOffer is in OfferService.
        var result = _offerService.SendOffer(productId, price, currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);

    }
    
    // accept offer
    [HttpPost("Offer/Accept")]
    public IActionResult Accept(int offerId)
    {
        var currentUserId = GetCurrentUserId();
        // AcceptOffer is in OfferService.
        var result = _offerService.AcceptOffer(offerId, currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }
    // reject offer
    [HttpPost("Offer/Reject")]
    public IActionResult Reject(int offerId)
    {
        var currentUserId = GetCurrentUserId();
        
        // RejectOffer is in OfferService.
        var result = _offerService.RejectOffer(offerId, currentUserId);
        if (result.Success == false)
        {
            return BadRequest(result.Message);
        }
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
