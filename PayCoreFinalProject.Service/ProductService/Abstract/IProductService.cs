using PayCoreFinalProject.Base.Product;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Abstract;

namespace PayCoreFinalProject.Service.ProductService.Abstract;

public interface IProductService : IBaseService<ProductDto,Product>
{

    public List<ProductResponse> OfferableProducts(int id);
    public List<ProductResponse> OfferableProductsByCategoryId(int category,int userId);
    public BaseResponse<IEnumerable<ProductResponse>>  GetAllMyProductOffers(int currentUserId);
    public BaseResponse<ProductResponse> Create(ProductRequest productRequest, int currentUserId);
    public BaseResponse<ProductResponse> Edit(int productId,ProductSpecialRequest productRequest, int currentUserId);
    public BaseResponse<ProductResponse> Delete(int productId, int currentUserId);



    public BaseResponse<IEnumerable<ProductDto>> GetAllProducts();





}