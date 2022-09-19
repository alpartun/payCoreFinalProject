using System.Security.Claims;
using AutoMapper;
using FluentNHibernate.Data;
using Microsoft.AspNetCore.Http;
using NHibernate;
using NHibernate.Mapping;
using PayCoreFinalProject.Base.Product;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Concrete;
using PayCoreFinalProject.Service.ProductService.Abstract;

namespace PayCoreFinalProject.Service.ProductService.Concrete;

public class ProductService : BaseService<ProductDto,Product>, IProductService
{
    protected readonly ISession _session;
    protected readonly IMapper _mapper;

    protected readonly IHttpContextAccessor _context;
    protected readonly IHibernateRepository<Product> _hibernateRepository;
    protected readonly IHibernateRepository<User> _userHibernateRepository;
    protected readonly IHibernateRepository<Category> _categoryHibernateRepository;
    protected readonly IHibernateRepository<Offer> _offerHibernateRepository;

    public ProductService(ISession session, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(session, mapper)
    {
        _session = session;
        _mapper = mapper;
        _hibernateRepository = new HibernateRepository<Product>(session);
        _userHibernateRepository = new HibernateRepository<User>(session);
        _categoryHibernateRepository = new HibernateRepository<Category>(session);
        _offerHibernateRepository = new HibernateRepository<Offer>(session);


        _context = httpContextAccessor;
    }

    public BaseResponse<ProductResponse> Create(ProductRequest productRequest, int currentUserId)
    {
        var mapper = _mapper.Map<ProductRequest, Product>(productRequest);
        var user = _userHibernateRepository.Entities.FirstOrDefault(x => x.Id == currentUserId);
        if (user == null)
        {
            return new BaseResponse<ProductResponse>("Product creation failed");
        }
        mapper.User = user;
        try
        {
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(mapper);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var result = _mapper.Map<Product,ProductResponse>(mapper);
            return new BaseResponse<ProductResponse>(result);

        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<ProductResponse>(e.Message);
        }
    }

    public BaseResponse<ProductResponse> Edit(int productId, ProductSpecialRequest productRequest, int currentUserId)
    {
        var product = _hibernateRepository.GetById(productId);
        var user = _userHibernateRepository.Entities.FirstOrDefault(x => x.Id == currentUserId);
        if (user == null)
        {
            return new BaseResponse<ProductResponse>("Product update failed");
        }

        
        if (product == null)
        {
            return new BaseResponse<ProductResponse>("Product is not found.");
        }

        if (product.User.Id != currentUserId)
        {
            return new BaseResponse<ProductResponse>("This product is not belongs to you.");
        }

        var entity = _mapper.Map<ProductSpecialRequest, Product>(productRequest);
        entity.User = user;

        var deleteOffers = _offerHibernateRepository.Entities.Where(x => x.Product.Id == productId).Select(x=>x.Id).ToList();





        try
        {   _offerHibernateRepository.BeginTransaction();
            foreach (var offerId in deleteOffers)
            {
                _offerHibernateRepository.Delete(offerId);
            }
            _offerHibernateRepository.Commit();
            _offerHibernateRepository.CloseTransaction();
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(entity);
            _hibernateRepository.Delete(product.Id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();

            var resource = _mapper.Map<Product, ProductResponse>(entity);
            return new BaseResponse<ProductResponse>(resource);
        }
        catch (Exception e)
        {
            _offerHibernateRepository.Rollback();
            _offerHibernateRepository.CloseTransaction();
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<ProductResponse>(e.Message);
        }
    }

    public BaseResponse<ProductResponse> Delete(int productId, int currentUserId)
    {
        var deleteProduct = _hibernateRepository.GetById(productId);
        if (deleteProduct == null)
        {
            return new BaseResponse<ProductResponse>("Product is not found.");
        }

        if (deleteProduct.User.Id != currentUserId)
        {
            return new BaseResponse<ProductResponse>("Product is not belongs to you. You have not access to delete this product.");
        }
        var deleteOffers = _offerHibernateRepository.Entities.Where(x => x.Product.Id == productId).Select(x => x.Id);
        try
        {   _offerHibernateRepository.BeginTransaction();
            foreach (var offerId in deleteOffers)
            {
                _offerHibernateRepository.Delete(offerId);
            }
            _offerHibernateRepository.Commit();
            _offerHibernateRepository.CloseTransaction();
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Delete(deleteProduct.Id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();

            return new BaseResponse<ProductResponse>(success:true);
        }
        catch (Exception e)
        {
            _offerHibernateRepository.Rollback();
            _offerHibernateRepository.CloseTransaction();
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<ProductResponse>(e.Message);
        }





    }



    public List<ProductResponse> OfferableProducts(int id)
    {
        var result = _hibernateRepository.Entities.Where(x => x.IsOfferable != false && x.User.Id !=id );
        var mapper = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductResponse>>(result);
        
        return mapper.ToList();
    }
    public List<ProductResponse> OfferableProductsByCategoryId  (int categoryId,int userId)
    {
        var result = _hibernateRepository.Entities.Where(x => x.IsOfferable != false && x.User.Id !=userId && x.Category.Id == categoryId);
        var mapper = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductResponse>>(result);
        
        return mapper.ToList();
    }

    public BaseResponse<IEnumerable<ProductResponse>> GetAllMyProductOffers(int currentUserId)
    {
        var products = _hibernateRepository.Entities.Where(x => (x.User.Id == currentUserId) && (x.Offers.Count > 0) && (x.IsSold==false)).ToList();
        if (products.Count == 0)
        {
            return new BaseResponse<IEnumerable<ProductResponse>>("Your products has no offer.");
        }
        
        var mapper = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);

        return new BaseResponse<IEnumerable<ProductResponse>>(mapper);
    }

    public BaseResponse<IEnumerable<ProductDto>> GetAllProducts()
    {
        try
        {
            var tempEntity = _hibernateRepository.Entities.Where(x=>x.IsSold == false).ToList();
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto
            >>(tempEntity);
            return new BaseResponse<IEnumerable<ProductDto>>(result);
        }
        catch (Exception e)
        {
            return new BaseResponse<IEnumerable<ProductDto>>(e.Message);
        }
    }


}