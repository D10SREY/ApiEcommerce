using System;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce.Mapping;

public class ProductProfile : Profile
{
public ProductProfile()

    {
        CreateMap<Product,ProductDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(Src => Src.category.Name))
        .ReverseMap();
         CreateMap<Product,CreateProductDto>().ReverseMap();
          CreateMap<Product,UpdateProductDto>().ReverseMap();

    }
}

