using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UnitOfWork
{
	public interface IUnitOfWork
	{
		public IMotorBikeService MotorBikeService { get; }
		public IUserService UserService { get; }
		public IRoleService RoleService { get; }
		public IStoreDescriptionService StoreDescriptionService { get; }
	}
}
