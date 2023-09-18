using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
	public class StoreDescriptionRepository : GenericRepository<StoreDesciption>, IStoreDescriptionService
	{
		public StoreDescriptionRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
		{
		}
	}
}
