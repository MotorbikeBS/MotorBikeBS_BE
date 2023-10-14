using Service.Service;

namespace Service.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IMotorBikeService MotorBikeService { get; }
        public IMotorModelService MotorModelService { get; }
        public IMotorBrandService MotorBrandService { get; }
        public IMotorImageService MotorImageService { get; }
        public IMotorStatusService MotorStatusService { get; }
        public IMotorTypeService MotorTypeService { get; }
        public IRequestService RequestService { get; }
        public IRequestTypeService RequestTypeService { get; }
        public IBillService BillService { get; }
        public IUserService UserService { get; }
        public IRoleService RoleService { get; }
        public IStoreDescriptionService StoreDescriptionService { get; }
		public IStoreImageService StoreImageService { get; }
		public IWishListService WishListService { get; }
		public IBookingService BookingService { get; }
	}
}
