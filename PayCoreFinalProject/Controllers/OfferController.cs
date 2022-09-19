using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.OfferService.Abstract;

namespace PayCoreFinalProject.Controllers;
[ApiController]
[NonController]
[Route("[controller]")]
public class OfferController : ControllerBase
{
    protected readonly IOfferService _offer;
    public OfferController(IOfferService offer)
    {
        _offer = offer;
    }
    
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        var result = _offer.GetAll();

        return Ok(result);
    }



    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var result = _offer.Remove(id);

        return Ok(result.Message);
    }

    [HttpPut]
    public IActionResult Update(int id, OfferDto offerDto)
    {
        var entity = _offer.Update(id, offerDto);

        return Ok(entity.Message);
    }


}