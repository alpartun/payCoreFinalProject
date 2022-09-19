using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Base.User;
using PayCoreFinalProject.Dto;

namespace PayCoreFinalProject.Service.RegisterService.Abstract;

public interface IRegisterService
{
    BaseResponse<UserResponse> Register(UserRegisterDto userRegisterDto);

    
}