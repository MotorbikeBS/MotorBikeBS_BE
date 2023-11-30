using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class StoreDescriptionRepository : GenericRepository<StoreDescription>, IStoreDescriptionService
    {
        public StoreDescriptionRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
