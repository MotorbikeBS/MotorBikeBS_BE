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
        public INotificationService NotificationService { get; }
        public INotificationTypeService NotificationTypeService { get; }
        public IRequestService RequestService { get; }
        public IRequestTypeService RequestTypeService { get; }
        public IBillService BillService { get; }
        public IUserService UserService { get; }
        public IRoleService RoleService { get; }
        public ICommentService CommentService { get; }
        public IStoreDescriptionService StoreDescriptionService { get; }
		public IStoreImageService StoreImageService { get; }
		public IWishListService WishListService { get; }
        public INegotiationService NegotiationService { get; }
        public IBuyerBookingService BuyerBookingService { get; }
        public IPaymentService PaymentService { get; }
        public IValuationService ValuationService { get; }
        public IPointHistoryService PointHistoryService { get; }
        public IPostBoostingService PostBoostingService { get; }
        public IReportService ReportService { get; }
        public IReportImageService ReportImageService { get; }
    }
}
