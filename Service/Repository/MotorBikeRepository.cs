using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class MotorBikeRepository : GenericRepository<Motorbike>, IMotorBikeService
    {
        public MotorBikeRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
