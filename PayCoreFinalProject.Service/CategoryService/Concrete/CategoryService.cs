using AutoMapper;
using NHibernate;
using PayCoreFinalProject.Base.Category;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Concrete;
using PayCoreFinalProject.Service.CategoryService.Abstract;
using Serilog;

namespace PayCoreFinalProject.Service.CategoryService.Concrete;

public class CategoryService : BaseService<CategoryDto,Category>,ICategoryService
{
    protected readonly IHibernateRepository<Category> _hibernateRepository;
    protected readonly IHibernateRepository<Offer> _offerHibernateRepository;
    protected readonly IHibernateRepository<Product> _productHibernateRepository;
    //injections
    public CategoryService(ISession session, IMapper mapper) : base(session, mapper)
    {
        _hibernateRepository = new HibernateRepository<Category>(session);
        _offerHibernateRepository = new HibernateRepository<Offer>(session);
        _productHibernateRepository = new HibernateRepository<Product>(session);
    }

    // Create Category
    public BaseResponse<Category> Create(CategoryRequest categoryRequest)
    {
        // mapping
        var entity = _mapper.Map<CategoryRequest, Category>(categoryRequest);
        // check entity is null or not
        if (entity == null)
        {
            return new BaseResponse<Category>("Failed");
        }
        try
        {
            // open transaction and save category
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Save(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<Category>(entity);
        }
        catch (Exception e)
        {
            Log.Error("CategoryService.Create", e);

            // if something went wrong then rollback
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            
            return new BaseResponse<Category>(e.Message);
        }
        
    }
    // Edit Category
    public BaseResponse<CategoryResponse> Edit(int id, CategoryRequest request)
    {
        try
        {
            // get entity for edit some parts
            var tempEntity = _hibernateRepository.GetById(id);
            
            // automapper operation and assign updated values to category object
            var entity = _mapper.Map<CategoryRequest, Category>(request,tempEntity);
            // then save to db
            _hibernateRepository.BeginTransaction();
            _hibernateRepository.Update(entity);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            var resource = _mapper.Map<Category, CategoryResponse>(entity);
            return new BaseResponse<CategoryResponse>(resource);
        }
        catch (Exception e)
        {
            Log.Error("CategoryService.Edit", e);

            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            return new BaseResponse<CategoryResponse>(e.Message);
        }
            
    }

    // Delete Category
    public BaseResponse<CategoryResponse> Delete(int id, int currentUserId)
    {
        // get category for delete operation
        var deleteCategory = _hibernateRepository.GetById(id);
        // get products which are belongs to that category

        var deleteProducts = _productHibernateRepository.Entities.Where(x => x.Category.Id == deleteCategory.Id)
            .Select(x => x.Id);
        
        // get affers which are belongs to selected products (and also category)
        var deleteOffers = _offerHibernateRepository.Entities.Where(x => x.Product.Category.Id == deleteCategory.Id)
            .Select(x => x.Id);

        try
        {
            // delete offers and products that belongs to categoryi and at the end delete that category
            _offerHibernateRepository.BeginTransaction();
            _productHibernateRepository.BeginTransaction();
            _hibernateRepository.BeginTransaction();
            foreach (var deleteOfferId in deleteOffers)
            {
                _offerHibernateRepository.Delete(deleteOfferId);
            }
            foreach (var deleteProductId in deleteProducts)
            {
                _productHibernateRepository.Delete(deleteProductId);
            }
            _hibernateRepository.Delete(deleteCategory.Id);
            _hibernateRepository.Commit();
            _hibernateRepository.CloseTransaction();
            _productHibernateRepository.CloseTransaction();
            _offerHibernateRepository.CloseTransaction();
            return new BaseResponse<CategoryResponse>(success:true);
        }
        catch (Exception e)
        {
            Log.Error("CategoryService.Delete", e);

            _offerHibernateRepository.Rollback();
            _productHibernateRepository.Rollback();
            _hibernateRepository.Rollback();
            _hibernateRepository.CloseTransaction();
            _productHibernateRepository.CloseTransaction();
            _offerHibernateRepository.CloseTransaction();

            return new BaseResponse<CategoryResponse>(e.Message);
        }
        
        
    }


    // GetAll Categories
    public BaseResponse<IEnumerable<CategoryResponse>>  GetAllCategories()
    {
        //get all categories in db 
        var tempEntity = _hibernateRepository.Entities.ToList();
        // mapping operation for return IEnumerable<CategoryResponse> result
        var result = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(tempEntity);


        
        return new BaseResponse<IEnumerable<CategoryResponse>>(result);
    }
}