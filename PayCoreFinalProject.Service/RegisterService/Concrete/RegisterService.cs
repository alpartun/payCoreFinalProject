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
    
        //injections
        public RegisterService(ISession session, IMapper mapper )
        {
            _session = session;
            _mapper = mapper;
            _hibernateRepository = new HibernateRepository<User>(session);
    
        }
        // Create passwordHash and passwordSalt method
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //PasswordHash and PasswordSalt
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        // Register
        public BaseResponse<UserResponse> Register(UserRegisterDto userRegisterDto)
        {
            // e mail has taken or not
            var isEmailExists = _hibernateRepository.Entities.Any(x=> x.Email ==userRegisterDto.Email);
            //if its taken then register failed. send error message to user.
            if (isEmailExists)
            {
                return new BaseResponse<UserResponse>("Duplicated e mail error. Please change your e mail.");
            }
            byte[] passwordHash, passwordSalt;
            // call CreatePasswordHash method for hash and salt operations
            CreatePasswordHash(userRegisterDto.Password, out passwordHash, out passwordSalt);
            // test
            // after the operation password is encrypted and we can store in db.
            // new user create
            var user = new User
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                Email = userRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            
            try
            {
                // open transaction and save user
                _hibernateRepository.BeginTransaction();
                _hibernateRepository.Save(user);
                _hibernateRepository.Commit();
                _hibernateRepository.CloseTransaction();
                
                var result = _mapper.Map<UserResponse>(user);
                
                return new BaseResponse<UserResponse>(result);
            }
            catch (Exception e)
            {
                // if some error has occurs then rollback and send failed message
                _hibernateRepository.Rollback();
                _hibernateRepository.CloseTransaction();
                return new BaseResponse<UserResponse>("Register operation is failed.");
            }
        }
    
}