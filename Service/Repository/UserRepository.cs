using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class UserRepository : GenericRepository<User>, IUserService
    {
        public UserRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<User> Register(User user)
        {
            try
            {
                await _unitOfWork.UserService.Add(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
