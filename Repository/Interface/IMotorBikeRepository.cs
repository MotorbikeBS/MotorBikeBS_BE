using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
	public interface IMotorBikeRepository
	{
		Task<List<Motorbike>> Get();
		Task Delete(int id);
		Task<Motorbike> GetById(int id);
		Task Update(Motorbike obj);
		Task Create(Motorbike obj);
	}
}
