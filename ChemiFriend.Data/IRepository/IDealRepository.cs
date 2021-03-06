﻿using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChemiFriend.Data.IRepository
{
    public interface IDealRepository : IGenericRepository<Deal>
    {
        IQueryable<GetDealModel> GetDealList();
        IQueryable<GetSchemeModel> GetSchemeList();
        IQueryable<GetDealModel> GetDealDetailsById(Int64 dealId);
        IQueryable<GetSchemeWithDealModel> GetSchemeWithDeal();
        IQueryable<OrderDetailModel> GetOrderDetail();
    }
}
