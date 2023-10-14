using Core.Models;
using Service.Repository;
using Service.Service;

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
        public IRequestService RequestService { get; private set; } = null!;
        public IBillService BillService { get; private set; } = null!;
        public IRequestTypeService RequestTypeService { get; private set; } = null!;
        public IUserService UserService { get; private set; } = null!;
        public IRoleService RoleService { get; private set; } = null!;
        public IStoreDescriptionService StoreDescriptionService { get; private set; } = null!;
		public IStoreImageService StoreImageService { get; private set; } = null!;
		public IWishListService WishListService { get; private set; } = null!;
		public IBookingService BookingService { get; private set; } = null!;
        public INegotiationService NegotiationService { get; private set; } = null!;


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
            RequestService = new RequestRepository(_context, this);
            RequestTypeService = new RequestTypeRepository(_context, this);
            BillService = new BillRepository(_context, this);
            UserService = new UserRepository(_context, this);
            RoleService = new RoleRepository(_context, this);
            StoreDescriptionService = new StoreDescriptionRepository(_context, this);
			StoreImageService = new StoreImageRepository(_context, this);
            WishListService = new WishListRepository(_context, this);
            BookingService = new BookingRepository(_context, this);
            NegotiationService = new NegotiationRepository(_context, this);
		}
    }
}
