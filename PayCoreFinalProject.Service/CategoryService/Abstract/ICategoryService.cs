using PayCoreFinalProject.Base.Category;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Abstract;

namespace PayCoreFinalProject.Service.CategoryService.Abstract;

public interface ICategoryService : IBaseService<CategoryDto,Category>
{
    BaseResponse<IEnumerable<CategoryResponse>> GetAllCategories();
    BaseResponse<Category> Create(CategoryRequest categoryRequest);
    BaseResponse<CategoryResponse> Edit(int id, CategoryRequest request);
    BaseResponse<CategoryResponse> Delete(int id, int currentUserId);

}