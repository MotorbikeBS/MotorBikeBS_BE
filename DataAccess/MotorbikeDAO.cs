using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
	public class MotorbikeDAO
	{
		private static MotorbikeDAO instance = null;
		private static readonly object instanceLock = new object();
		MotorbikeDBContext _dbContext = new MotorbikeDBContext();
		public MotorbikeDAO()
		{

		}
		public static MotorbikeDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new MotorbikeDAO();
					}
					return instance;
				}
			}
		}
		public async Task<List<Motorbike>> Get()
		{
			try
			{
				var obj = await _dbContext.Motorbikes.ToListAsync();
				if (obj != null)
				{
					return obj;
				}
				return null;
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}


		public async Task Delete(int id)
		{
			try
			{
				var obj = await _dbContext.Motorbikes.FirstOrDefaultAsync(x => x.MotorId == id);
					_dbContext.Motorbikes.Remove(obj);
					_dbContext.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
		public async Task<Motorbike> GetById(int id)
		{
			try
			{
				var ca = await _dbContext.Motorbikes.FirstOrDefaultAsync(x => x.MotorId == id);
				if (ca != null)
				{
					return ca;
				}
				return null;
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

		public async Task Update(Motorbike obj)
		{
			try
			{
				_dbContext.ChangeTracker.Clear();
				_dbContext.Entry<Motorbike>(obj).State = EntityState.Modified;
				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
		public async Task Create(Motorbike obj)
		{
			try
			{
				_dbContext.Motorbikes.Add(obj);
				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}
	}
}
