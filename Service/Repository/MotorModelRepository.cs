﻿using Core.Models;
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
	public class MotorModelRepository : GenericRepository<MotorbikeModel>, IMotorModelService
    {
		public MotorModelRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
		{
		}
	}
}
