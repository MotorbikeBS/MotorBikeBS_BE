using Core.Models;
using Service.Generic;

namespace Service.Service
{
    public interface IUserService : IGenericRepository<User>
    {
        Task<User> Register(User user);
    }
}
