using BusinessObject.Models;
using DataAccess;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
	public class MotorBikeRepository : IMotorBikeRepository
	{
		public async Task Create(Motorbike obj) => await MotorbikeDAO.Instance.Create(obj);

		public async Task Delete(int id) => await MotorbikeDAO.Instance.Delete(id);

		public async Task<Motorbike> GetById(int id) => await MotorbikeDAO.Instance.GetById(id);

		public async Task<List<Motorbike>> Get() => await MotorbikeDAO.Instance.Get();

		public async Task Update(Motorbike obj) => await MotorbikeDAO.Instance.Update(obj);
	}
}
