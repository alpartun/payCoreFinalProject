using PayCoreFinalProject.Base.Response;

namespace PayCoreFinalProject.Service.Base.Abstract;

public interface IBaseService<Dto, Entity>
{
    BaseResponse<Dto> GetById(int id);
    BaseResponse<IEnumerable<Dto>> GetAll();
    BaseResponse<Dto> Insert(Dto InsertResource);
    BaseResponse<Dto> Update(int id, Dto updateResource);
    BaseResponse<Dto> Remove(int id);
}