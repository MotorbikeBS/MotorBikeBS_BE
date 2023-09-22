using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class MotorModelRepository : GenericRepository<MotorbikeModel>, IMotorModelService
    {
        public MotorModelRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
