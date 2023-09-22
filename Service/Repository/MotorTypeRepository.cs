using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;

namespace Service.Repository
{
    public class MotorTypeRepository : GenericRepository<MotorbikeType>, IMotorTypeService
    {
        public MotorTypeRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
