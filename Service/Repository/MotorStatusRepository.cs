using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class MotorStatusRepository : GenericRepository<MotorbikeStatus>, IMotorStatusService
    {
        public MotorStatusRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
