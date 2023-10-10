using Core.Models;
using Service.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
	public interface IBookingService : IGenericRepository<Booking>
	{
	}
}
