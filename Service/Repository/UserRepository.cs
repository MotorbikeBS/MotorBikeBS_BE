using Core.Models;
using Microsoft.EntityFrameworkCore;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
	public class UserRepository : GenericRepository<User>, IUserService
	{
		public readonly MotorbikeDBContext _context;
		public UserRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
		{
			_context = context;
		}

		public async Task<User> Login(User user)
		{
			User userObj = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email &&  x.Password == user.Password);
			if(userObj != null)
			{
				return userObj;
			}
			return null;
		}

		public async Task<User> Register(User user)
		{
			try
			{
				await _unitOfWork.UserService.Add(user);
				return user;
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
