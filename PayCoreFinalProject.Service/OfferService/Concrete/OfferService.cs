using AutoMapper;
using NHibernate;
using NHibernate.Criterion;
using PayCoreFinalProject.Base.Offer;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Concrete;
using PayCoreFinalProject.Service.OfferService.Abstract;

namespace PayCoreFinalProject.Service.OfferService.Concrete;

public class OfferService : BaseService<OfferDto,Offer>, IOfferService
{
    protected readonly IHibernateRepository<Offer> _hibernateRepository;
    protected readonly IHibernateRepository<Product> _productHibernateRepository;
    protected readonly IHibernateRepository<User> _userHibernateRepository;

    public OfferService(ISession session, IMapper mapper) : base(session, mapper)
    {
        _hibernateRepository = new HibernateRepository<Offer>(session);
        _productHibernateRepository = new HibernateRepository<Product>(session);
        _userHibernateRepository = new HibernateRepository<User>(session);
    }

    public BaseResponse<Offer> Order(int productId, int currentUserId)
    {
        var product = _productHibernateRepository.Entities.FirstOrDefault(x => x.Id== productId);
        if (product == null)
        {
            return new BaseResponse<Offer>("You can not buy this product. Product is not found or already sold.");
        }
        
        if ((product.IsSold) || (product.User.Id == currentUserId))
        {
            return new BaseResponse<Offer>("Failed");
        }
  
        var user = _userHibernateRepository.Entities.FirstOrDefault(x => x.Id == product.User.Id);
        if (user == null)
        {
            return new BaseResponse<Offer>("Seller is not found.");
        }
        var newBuyOffer = new Offer
        {
            OfferedById = currentUserId,
            OfferedPrice = product.Price,
            Product = product,
            User = product.User
        };

        product.IsSold = true;

        var deleteOtherOffers = _hibernateRepository.Entities.Where(x=>x.Product.IsSold==true && Math.Abs(x.Product.Price - x.OfferedPrice) > 0).Select(x=>x.Id);
        

        try
        {
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(newBuyOffer);
            foreach (var delete in deleteOtherOffers)
            {
                _hibernateRepository.Delete(delete);

                
            }
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Offer>(newBuyOffer);

        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Offer>(e.Message);
        }
    }

    public BaseResponse<Offer> SendOffer(int productId, double price, int currentUserId)
    {
        var product =
            _productHibernateRepository.Entities.FirstOrDefault(x => x.Id == productId && x.User.Id != currentUserId
                && x.IsOfferable==true);
        if (product == null)
        {
            return new BaseResponse<Offer>("Product not found.");
        }

        if (product.IsOfferable == false)
        {
            return new BaseResponse<Offer>("Product is not offerable.");
        }
        
        if (product.Price < price)
        {
            return new BaseResponse<Offer>(message: "Offer can not be higher than product price.");;
        }
        var user = _userHibernateRepository.Entities.FirstOrDefault(x => x.Id == product.User.Id);
        
        if (user == null)
        {
            return new BaseResponse<Offer>(message: "Seller could not be found.");
        }

        var deleteUserOffers = _hibernateRepository.Entities.Where(x => x.OfferedById == currentUserId && x.Product.Id == productId)
            .Select(x => x.Id).ToList();
        var entity = new Offer
        {
            Product = product,
            User = user,
            OfferedPrice = price,
            OfferedById = currentUserId
        };
        try
        {
            _hibernateRepository.BeginTransaction();
            foreach (var offerId in deleteUserOffers)
            {
                _hibernateRepository.Delete(offerId);
            }
            _hibernateRepository.Save(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();

            return new BaseResponse<Offer>(entity);

        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Offer>(e.Message);
        }
    }

    public BaseResponse<Offer> AcceptOffer(int offerId, int currentUser)
    {
        var acceptedOffer = _hibernateRepository.GetById(offerId);
        acceptedOffer.Product.IsSold = true;
        
        var product = acceptedOffer.Product;
        var otherOffers = product.Offers.Where(x => x.Id != offerId).Select(x=>x.Id).ToList();

        try
        {
            _hibernateRepository.BeginTransaction();
            foreach (var id in otherOffers)
            {
                _hibernateRepository.Delete(id);
            }
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();

            return new BaseResponse<Offer>(acceptedOffer);

        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Offer>(e.Message);


        }


    }

    public BaseResponse<Offer> RejectOffer(int offerId, int currentUser)
    {
        var user = _userHibernateRepository.GetById(currentUser);
        if (user != null)
        {
            
        }
        var rejectedOffer = _hibernateRepository.GetById(offerId);

        if (rejectedOffer.User.Id != currentUser)
        {
            return new BaseResponse<Offer>("This product is not belongs to you. You can not reject the offer.");
        }

        try
        {
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Delete(rejectedOffer.Id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();

            return new BaseResponse<Offer>(rejectedOffer);

        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Offer>(e.Message);
        }
        
        
        
    }

    public BaseResponse<IEnumerable<OfferResponse>> GetAllMyOffers(int currentUserId)
    {
        var offers = _hibernateRepository.Entities.Where(x => x.OfferedById == currentUserId).ToList();

        var mapper = _mapper.Map<IEnumerable<Offer>, IEnumerable<OfferResponse>>(offers);
        
        return new BaseResponse<IEnumerable<OfferResponse>>(mapper);
    }

    public BaseResponse<IEnumerable<OfferResponse>> OrdersGetAll(int currentUserId)
    {
        var orders = _hibernateRepository.Entities.Where(x => x.OfferedById == currentUserId && x.Product.IsOfferable==true && Math.Abs(x.OfferedPrice - x.Product.Price) == 0).ToList();

        if (orders == null)
        {
            return new BaseResponse<IEnumerable<OfferResponse>>("Fail");
        }

        var mapper = _mapper.Map<IEnumerable<Offer>, IEnumerable<OfferResponse>>(orders);

        return new BaseResponse<IEnumerable<OfferResponse>>(mapper);
    }

    public BaseResponse<IEnumerable<OfferResponse>> SoldProductsGetAll(int currentUserId)
    {
        var entities =
            _hibernateRepository.Entities.Where(x => x.Product.User.Id == currentUserId && x.Product.IsSold == true).ToList();

        if (entities == null)
        {
            return new BaseResponse<IEnumerable<OfferResponse>>("Fail");
        }

        var mapper = _mapper.Map<IEnumerable<Offer>, IEnumerable<OfferResponse>>(entities);

        return new BaseResponse<IEnumerable<OfferResponse>>(mapper);
    }

    public BaseResponse<OfferResponse> UpdateOffer(OfferRequest offerRequest, int currentUser)
    {
        try
        {
            var tempEntity = _hibernateRepository.GetById(offerRequest.Id);
            if (currentUser != tempEntity.OfferedById)
            {
                return new BaseResponse<OfferResponse>("This offer is not belongs to you.Update is denied. ");
            }
            if (tempEntity == null)
            {
                new BaseResponse<OfferResponse>("Record is not found.");
            }
            var entity = _mapper.Map<OfferRequest, Offer>(offerRequest,tempEntity);
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Update(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var resource = _mapper.Map<Offer, OfferResponse>(entity);
            return new BaseResponse<OfferResponse>(resource);
        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<OfferResponse>(e.Message);
        }
    }




    


        
    

}