using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class MotorImageRepository : GenericRepository<MotorbikeImage>, IMotorImageService
    {
        public MotorImageRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
