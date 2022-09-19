using AutoMapper;
using NHibernate;
using PayCoreFinalProject.Base.Category;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Concrete;
using PayCoreFinalProject.Service.CategoryService.Abstract;

namespace PayCoreFinalProject.Service.CategoryService.Concrete;

public class CategoryService : BaseService<CategoryDto,Category>,ICategoryService
{
    protected readonly IHibernateRepository<Category> _hibernateRepository;
    protected readonly IHibernateRepository<Offer> _offerHibernateRepository;
    protected readonly IHibernateRepository<Product> _productHibernateRepository;
    public CategoryService(ISession session, IMapper mapper) : base(session, mapper)
    {
        _hibernateRepository = new HibernateRepository<Category>(session);
        _offerHibernateRepository = new HibernateRepository<Offer>(session);
        _productHibernateRepository = new HibernateRepository<Product>(session);
    }

    public BaseResponse<Category> Create(CategoryRequest categoryRequest)
    {
        var mapper = _mapper.Map<CategoryRequest, Category>(categoryRequest);
        if (mapper == null)
        {
            return new BaseResponse<Category>("Failed");
        }
        try
        {
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(mapper);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Category>(mapper);
        }
        catch (Exception e)
        {

            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            
            return new BaseResponse<Category>(e.Message);
        }
        
    }
    
    public BaseResponse<CategoryResponse> Edit(int id, CategoryRequest request)
    {
        try
        {
            var tempEntity = _hibernateRepository.GetById(id);
            if (tempEntity is null)
            {
                return new BaseResponse<CategoryResponse>("Record is not found.");
            }
            var entity = _mapper.Map<CategoryRequest, Category>(request,tempEntity);
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Update(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var resource = _mapper.Map<Category, CategoryResponse>(entity);
            return new BaseResponse<CategoryResponse>(resource);
        }
        catch (Exception e)
        {
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<CategoryResponse>(e.Message);
        }
            
    }

    public BaseResponse<CategoryResponse> Delete(int id, int currentUserId)
    {
        var deleteCategory = _hibernateRepository.GetById(id);
        if (deleteCategory == null)
        {
            return new BaseResponse<CategoryResponse>("Category is not found.");
        }

        var deleteProducts = _productHibernateRepository.Entities.Where(x => x.Category.Id == deleteCategory.Id)
            .Select(x => x.Id);
        var deleteOffers = _offerHibernateRepository.Entities.Where(x => x.Product.Category.Id == deleteCategory.Id)
            .Select(x => x.Id);

        try
        {
            _offerHibernateRepository.BeginTransaction();
            _productHibernateRepository.BeginTransaction();
            _hibernateRepository.BeginTransaction();
            foreach (var deleteOfferId in deleteOffers)
            {
                _offerHibernateRepository.Delete(deleteOfferId);
            }
            _offerHibernateRepository.Commit();
            foreach (var deleteProductId in deleteProducts)
            {
                _productHibernateRepository.Delete(deleteProductId);
            }
            _productHibernateRepository.Commit();
            _hibernateRepository.Delete(deleteCategory.Id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            _productHibernateRepository.CloseTransaction();
            _offerHibernateRepository.CloseTransaction();
            return new BaseResponse<CategoryResponse>(success:true);
        }
        catch (Exception e)
        {
            _offerHibernateRepository.Rollback();
            _productHibernateRepository.Rollback();
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            _productHibernateRepository.CloseTransaction();
            _offerHibernateRepository.CloseTransaction();

            return new BaseResponse<CategoryResponse>(e.Message);
        }
        
        
    }



    public BaseResponse<IEnumerable<CategoryResponse>>  GetAllCategories()
    {
        var tempEntity = _hibernateRepository.Entities.ToList();
        var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(tempEntity);


        
        return new BaseResponse<IEnumerable<CategoryResponse>>(result);
    }
}