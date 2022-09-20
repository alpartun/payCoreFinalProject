using AutoMapper;
using NHibernate;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.Base.Concrete;
using PayCoreFinalProject.Service.UserService.Abstract;

namespace PayCoreFinalProject.Service.UserService.Concrete;

public class UserService : BaseService<UserDto,User>, IUserService
{
    protected readonly ISession _session;
    protected readonly IMapper _mapper;
    protected readonly IHibernateRepository<User> _hibernateRepository;
    //Injections
    public UserService(ISession session, IMapper mapper) : base(session, mapper)
    {
        _session = session;
        _mapper = mapper;

        _hibernateRepository = new HibernateRepository<User>(session);
    }
    // this service seems empty but if there is an admin role then we can fill this service with crud operations.
    
    
}