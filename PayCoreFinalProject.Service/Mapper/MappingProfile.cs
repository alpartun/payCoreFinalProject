using AutoMapper;
using PayCoreFinalProject.Base.Category;
using PayCoreFinalProject.Base.Offer;
using PayCoreFinalProject.Base.Product;
using PayCoreFinalProject.Base.User;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Dto;

namespace PayCoreFinalProject.Service.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UserRegisterDto, User>().ReverseMap();
        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<UserRegisterDto, UserResponse>().ReverseMap();
        
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CategoryResponse>().ReverseMap();
        CreateMap<Category, CategoryRequest>().ReverseMap(); 
        CreateMap<CategoryDto, CategoryResponse>().ReverseMap();
        
        
        CreateMap<Offer, OfferDto>().ReverseMap();
        CreateMap<OfferResponse, Offer>().ReverseMap();
        CreateMap<Offer, OfferRequest>().ReverseMap();

        CreateMap<Product, ProductResponse>();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<ProductDto, ProductResponse>().ReverseMap();
        CreateMap<ProductDto, ProductRequest>().ReverseMap();
        CreateMap<Product, ProductRequest>().ReverseMap();
        CreateMap<Product, ProductSpecialRequest>().ReverseMap();



    }
    
}