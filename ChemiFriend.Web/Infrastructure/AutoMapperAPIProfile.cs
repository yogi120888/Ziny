using ChemiFriend.Web.Models;
using ChemiFriend.Web.Models.InputModel;
using ChemiFriend.Entity;
using ChemiFriend.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChemiFriend.Entity.JsonModel;

namespace ChemiFriend.Web.Infrastructure
{
    public class AutoMapperAPIProfile : AutoMapper.Profile
    {
        public AutoMapperAPIProfile()
        {
            //CreateMap<UsermasterModel, Usermaster>();
            CreateMap<RegistrationModel, Registration>();
            CreateMap<Registration, RegistrationModel>();
            CreateMap<ProductCategoryModel, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryModel>();
            CreateMap<ProductSubCategoryModel, ProductSubCategory>();
            CreateMap<ProductSubCategory, ProductSubCategoryModel>();
            CreateMap<ProductSubCategory, GetProductSubCategoryModel>();
            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>();
            CreateMap<Product, GetProductModel>();
            CreateMap<DealModel, Deal>();
            CreateMap<Deal, DealModel>();
            CreateMap<SchemeModel, Scheme>();
            CreateMap<Scheme, SchemeModel>();

            //CreateMap<OrderModel, Order>();
            //CreateMap<OrderItemModel, OrderItem>();
            //CreateMap<DealModel, Deal>();
            //CreateMap<SchemeModel, Scheme>();
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