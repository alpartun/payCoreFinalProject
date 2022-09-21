using PayCoreFinalProject.Base.Offer;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Abstract;

namespace PayCoreFinalProject.Service.OfferService.Abstract;

public interface IOfferService : IBaseService<OfferDto,Offer>
{
    public Task<BaseResponse<Offer>> SendOffer(int productId, decimal price, int currentUserId);
    public BaseResponse<Offer> AcceptOffer(int offerId, int currentUser);
    public BaseResponse<Offer> RejectOffer(int offerId, int currentUser);

    public Task<BaseResponse<Offer>> Order(int productId, int currentUserId);
    public BaseResponse<IEnumerable<OfferResponse>> GetAllMyOffers(int currentUserId);
    public BaseResponse<IEnumerable<OfferResponse>> OrdersGetAll(int currentUserId);
    public BaseResponse<IEnumerable<OfferResponse>> SoldProductsGetAll(int currentUserId);
    public BaseResponse<OfferResponse> UpdateOffer(OfferRequest offerRequest, int currentUser);




}