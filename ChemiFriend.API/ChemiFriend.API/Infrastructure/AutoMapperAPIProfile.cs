using ChemiFriend.API.Models;
using ChemiFriend.API.Models.InputModel;
using ChemiFriend.Entity;
using ChemiFriend.Entity.JsonModel;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Infrastructure
{
    public class AutoMapperAPIProfile : AutoMapper.Profile
    {
        public AutoMapperAPIProfile()
        {
            CreateMap<UsermasterModel, Usermaster>();
            CreateMap<GetUsermasterModel, Usermaster>();
            CreateMap<Usermaster, GetUsermasterModel>();
            CreateMap<RegistrationModel, Registration>();
            CreateMap<ProductCategoryModel, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryModel>();
            CreateMap<ProductSubCategoryModel, ProductSubCategory>();
            CreateMap<GetProductSubCategoryModel, ProductSubCategory>();
            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>();
            CreateMap<Product, GetProductModel>();
            CreateMap<OrderModel, Order>();
            CreateMap<OrderItemModel, OrderItem>();
            CreateMap<DealModel, Deal>();
            CreateMap<SchemeModel, Scheme>();
        }

        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperAPIProfile>();
            });
        }
    }
}