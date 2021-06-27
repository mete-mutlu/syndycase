using AutoMapper;
using Product.API.Models;
using Product.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities = Product.Core.Entities;

namespace Product.API.Infrastructure.AutoMapper
{
    public class MapperProfile :Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateProductModel, Entities.Product>()
                .ForMember(dest => dest.Id,  p => p.Ignore())
                .ForMember(dest => dest.Status, p => p.Ignore())
                .ForMember(dest => dest.CreatedAt, p => p.Ignore())
                .ForMember(dest => dest.ModifiedAt, p => p.Ignore())
                .ForMember(dest => dest.User,p=>p.Ignore())
                .ForMember(dest => dest.Brand, p => p.Ignore());

            CreateMap<UpdateProductModel, Entities.Product>()
                .ForMember(dest => dest.Status, p => p.Ignore())
                .ForMember(dest => dest.CreatedAt, p => p.Ignore())
                .ForMember(dest => dest.ModifiedAt, p => p.Ignore())
                .ForMember(dest => dest.User, p => p.Ignore())
                .ForMember(dest => dest.Brand, p => p.Ignore());

            CreateMap<Entities.Product, ProductDto>()
               .ForMember(dest => dest.Username, src => src.MapFrom(p => p.User.Username))
               .ForMember(dest => dest.BrandName, src => src.MapFrom(p => p.Brand.Name));
        }
    }
}
