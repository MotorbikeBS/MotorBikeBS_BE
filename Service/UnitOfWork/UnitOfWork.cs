using Core.Models;
using Service.Repository;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public IMotorBikeService MotorBikeService { get; private set; } = null!;
        public IMotorModelService MotorModelService { get; private set; } = null!;
        public IMotorBrandService MotorBrandService { get; private set; } = null!;
        public IMotorImageService MotorImageService { get; private set; } = null!;
        public IMotorStatusService MotorStatusService { get; private set; } = null!;
        public IMotorTypeService MotorTypeService { get; private set; } = null!;
        public IUserService UserService { get; private set; } = null!;
		public IRoleService RoleService { get; private set; } = null!;
		public IStoreDescriptionService StoreDescriptionService { get; private set; } = null!;


        private readonly MotorbikeDBContext _context;

		public UnitOfWork()
		{
			_context = new MotorbikeDBContext();
			InitRepositories();
		}

		private void InitRepositories()
		{
			MotorBikeService = new MotorBikeRepository(_context, this);
            MotorBrandService = new MotorBrandRepository(_context, this);
            MotorModelService = new MotorModelRepository(_context, this);
            MotorImageService = new MotorImageRepository(_context, this);
            MotorStatusService = new MotorStatusRepository(_context, this);
            MotorTypeService = new MotorTypeRepository(_context, this);
            UserService = new UserRepository(_context, this);
			RoleService = new RoleRepository(_context, this);
			StoreDescriptionService = new StoreDescriptionRepository(_context, this);
		}
	}
}
