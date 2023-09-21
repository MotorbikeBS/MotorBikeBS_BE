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
        public IMotorImageService MotorImageService { get; }
        public IMotorStatusService MotorStatusService { get; }
        public IMotorTypeService MotorTypeService { get; }
        public IFacilityService FacilityService { get; }
        public IUserService UserService { get; }
		public IRoleService RoleService { get; }
		public IStoreDescriptionService StoreDescriptionService { get; }
	}
}
