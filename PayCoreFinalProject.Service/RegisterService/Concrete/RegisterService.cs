using AutoMapper;
using NHibernate;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Base.User;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Dto;
using PayCoreFinalProject.Service.RegisterService.Abstract;

namespace PayCoreFinalProject.Service.RegisterService.Concrete;

    public class RegisterService: IRegisterService
    {
        protected readonly ISession _session;
        protected readonly IMapper _mapper;
        protected readonly IHibernateRepository<User> _hibernateRepository;
    
    
        public RegisterService(ISession session, IMapper mapper )
        {
            _session = session;
            _mapper = mapper;
            _hibernateRepository = new HibernateRepository<User>(session);
    
        }
        // it can user return i will decide later.
    
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //PasswordHash and Salt
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    
            }
        }
    
        public BaseResponse<UserResponse> Register(UserRegisterDto userRegisterDto)
        {
            var isEmailUnique = _hibernateRepository.Entities.Any(x=> x.Email ==userRegisterDto.Email);

            if (isEmailUnique)
            {
                return new BaseResponse<UserResponse>("Duplicated e mail error. Please change your e mail.");
            }
            byte[] passwordHash, passwordSalt;
    
            CreatePasswordHash(userRegisterDto.Password, out passwordHash, out passwordSalt);
    
            var user = new User
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                Email = userRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            
            if (user is null)
            {
                return null;
            }
    
            try
            {
                _hibernateRepository.BeginTransaction();
                _hibernateRepository.Save(user);
                _hibernateRepository.Commit();
                _hibernateRepository.CloseTransaction();

                var result = _mapper.Map<UserResponse>(user);
    
                return new BaseResponse<UserResponse>(result);
            }
            catch (Exception e)
            {
                _hibernateRepository.Rollback();
                _hibernateRepository.CloseTransaction();
                return new BaseResponse<UserResponse>("Fault");
            }
        }
    
}