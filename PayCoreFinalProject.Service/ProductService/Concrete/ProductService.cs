using AutoMapper;
using Microsoft.AspNetCore.Http;
using NHibernate;
using PayCoreFinalProject.Base.Product;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Concrete;
using PayCoreFinalProject.Service.EmailService.Abstract;
using PayCoreFinalProject.Service.ProductService.Abstract;
using PayCoreFinalProject.Service.RabbitMQ.Abstract;
using Serilog;

namespace PayCoreFinalProject.Service.ProductService.Concrete;

public class ProductService : BaseService<ProductDto,Product>, IProductService
{
    protected readonly ISession _session;
    protected readonly IMapper _mapper;

    protected readonly IRabbitMQProducer _rabbitMqProducer;

    protected readonly IHibernateRepository<Product> _hibernateRepository;
    protected readonly IHibernateRepository<User> _userHibernateRepository;
    protected readonly IHibernateRepository<Category> _categoryHibernateRepository;
    protected readonly IHibernateRepository<Offer> _offerHibernateRepository;
    //injections
    public ProductService(ISession session, IMapper mapper, IRabbitMQProducer rabbitMqProducer) : base(session, mapper)
    {
        _session = session;
        _mapper = mapper;
        _rabbitMqProducer = rabbitMqProducer;
        _hibernateRepository = new HibernateRepository<Product>(session);
        _userHibernateRepository = new HibernateRepository<User>(session);
        _categoryHibernateRepository = new HibernateRepository<Category>(session);
        _offerHibernateRepository = new HibernateRepository<Offer>(session);
    }

    public async Task<BaseResponse<ProductResponse>> Create(ProductRequest productRequest, int currentUserId)
    {
        //map operation ProductRequest -> Product, using productRequest
        var entity = _mapper.Map<ProductRequest, Product>(productRequest);
        
        //is currentUser exists or not
        var user = _userHibernateRepository.Entities.FirstOrDefault(x => x.Id == currentUserId);
        if (user == null)
        {
            return new BaseResponse<ProductResponse>("Product creation failed");
        }
        //assign user to product. That user is the OWNER of the product.
        entity.User = user;
        try
        {
            // begin transaction and save
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var result = _mapper.Map<Product,ProductResponse>(entity);


            var productCreatedEmailToProductOwner = new Email
            {
                EmailAdress = user.Email,
                EmailTitle = "New Product",
                EmailMessage = $"Your product is listed.\n" +
                               $"Product Name : {entity.Name}\n" +
                               $"Product Category : {entity.Category.Name}\n" +
                               $"Product Brand : {entity.Brand}\n" +
                               $"Product Price : {entity.Price}\n" +
                               $"Product Description : {entity.Description}\n"
            };
            await _rabbitMqProducer.Produce(productCreatedEmailToProductOwner);
            
            return new BaseResponse<ProductResponse>(result);

        }
        catch (Exception e)
        {
            Log.Error("ProductService.Create", e);

            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<ProductResponse>(e.Message);
        }
    }

    //Edit Product
    public BaseResponse<ProductResponse> Edit(int productId, ProductSpecialRequest productRequest, int currentUserId)
    {
        //Get product using productId
        var product = _hibernateRepository.GetById(productId);
        // get user using currentUserId
        var user = _userHibernateRepository.Entities.FirstOrDefault(x => x.Id == currentUserId);
        // check user is null or not
        if (user is null)
        {
            return new BaseResponse<ProductResponse>("User can not be found.");
        }
        //check products user Id(ownerId) is equal to current user, if its not, product not belongs to that user so edit operation must be fail.
        if (product.User.Id != currentUserId)
        {
            return new BaseResponse<ProductResponse>("This product is not belongs to you.");
        }
        // map
        var entity = _mapper.Map<ProductSpecialRequest, Product>(productRequest);
        // assign ownerUser to entity user(objects)
        entity.User = user;
        // if the product will be update then offers must be drop, get all offers belongs to that product.
        var deleteOffers = _offerHibernateRepository.Entities.Where(x => x.Product.Id == productId).Select(x=>x.Id).ToList();
        try
        {
            // save new updated entity. delete old one with offers 
            _offerHibernateRepository.BeginTransaction();
            _hibernateRepository.BeginTransaction();

            foreach (var offerId in deleteOffers)
            {
                _offerHibernateRepository.Delete(offerId);
            }
            _offerHibernateRepository.Commit();
            _hibernateRepository.Save(entity);
            _hibernateRepository.Delete(product.Id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            _offerHibernateRepository.CloseTransaction();
            
            var resource = _mapper.Map<Product, ProductResponse>(entity);
            return new BaseResponse<ProductResponse>(resource);
        }
        catch (Exception e)
        {
            Log.Error("ProductService.Edit", e);

            _offerHibernateRepository.Rollback();
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            _offerHibernateRepository.CloseTransaction();
            return new BaseResponse<ProductResponse>(e.Message);
        }
    }
    //Delete
    public BaseResponse<ProductResponse> Delete(int productId, int currentUserId)
    {
        // get product for delete operation
        var deleteProduct = _hibernateRepository.GetById(productId);
        //check deleteProduct is null or not
        if (deleteProduct is null)
        {
            return new BaseResponse<ProductResponse>("Product is not found.");
        }
        //check product belong to current user or not
        if (deleteProduct.User.Id != currentUserId)
        {
            return new BaseResponse<ProductResponse>("Product is not belongs to you. You have not access to delete this product.");
        }
        //get offers that belongs to product
        var deleteOffers = _offerHibernateRepository.Entities.Where(x => x.Product.Id == productId).Select(x => x.Id);
        try
        {   
            // delete offers and product
            _offerHibernateRepository.BeginTransaction();
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
            Log.Error("ProductService.Delete", e);

            _offerHibernateRepository.Rollback();
            _offerHibernateRepository.CloseTransaction();
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<ProductResponse>(e.Message);
        }
    }



    // get all offerable products
    public BaseResponse<IEnumerable<ProductResponse>>  OfferableProducts(int id)
    {
        // get products that user can offer
        var offerableProducts = _hibernateRepository.Entities.Where(x => x.IsOfferable != false && x.User.Id !=id && x.IsSold!=true);
        //check offerableProducts is null or not
        if (offerableProducts is null)
        {
            return new BaseResponse<IEnumerable<ProductResponse>>("There is no offerable products.");

        }
        //map
        var entity = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductResponse>>(offerableProducts);
        
        return new BaseResponse<IEnumerable<ProductResponse>>(entity);
    }
    // offerableProducts by specific categoryId
    public BaseResponse<IEnumerable<ProductResponse>>  OfferableProductsByCategoryId  (int categoryId,int userId)
    {
        // get categorized offerable products,(in here user's products not included)
        var result = _hibernateRepository.Entities.Where(x => x.IsOfferable != false && x.User.Id !=userId && x.Category.Id == categoryId && (x.IsSold==false));
        //map
        var entity = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductResponse>>(result);
        
        return new BaseResponse<IEnumerable<ProductResponse>>(entity);
    }

    //get all products and offers for that product
    public BaseResponse<IEnumerable<ProductResponse>> GetAllMyProductOffers(int currentUserId)
    {
        //get products
        //that users products +
        //product must be an offer +
        //isSold area must be false
        var products = _hibernateRepository.Entities.Where(x => (x.User.Id == currentUserId) && (x.Offers.Count > 0) && (x.IsSold==false)).ToList();
        //check product is empty or not
        if (products.Count == 0)
        {
            return new BaseResponse<IEnumerable<ProductResponse>>("Your products has no offer.");
        }
        
        var entity = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);

        return new BaseResponse<IEnumerable<ProductResponse>>(entity);
    }

    // get all products, except sold out ones
    public BaseResponse<IEnumerable<ProductDto>> GetAllProducts()
    {
        try
        {
            var tempEntity = _hibernateRepository.Entities.Where(x=>x.IsSold == false).ToList();
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(tempEntity);
            return new BaseResponse<IEnumerable<ProductDto>>(result);
        }
        catch (Exception e)
        {
            Log.Error("ProductService.GetAllProducts", e);

            return new BaseResponse<IEnumerable<ProductDto>>(e.Message);
        }
    }


}