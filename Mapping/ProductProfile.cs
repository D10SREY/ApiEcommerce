using System;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using Mapster;

namespace ApiEcommerce.Mapping;

public class ProductProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductDto>()
            .Map(destination => destination.CategoryName, source => source.Category.Name)
            .Map(destination => destination.Sku, source => source.SKU);
        config.NewConfig<ProductDto, Product>()
            .Map(destination => destination.Category, source => new Category
            {
                Name = source.CategoryName
            })
            .Map(destination => destination.SKU, source => source.Sku);

        config.NewConfig<Product, CreateProductDto>()
            .Map(destination => destination.Sku, source => source.SKU);
        config.NewConfig<CreateProductDto, Product>()
            .Map(destination => destination.SKU, source => source.Sku);

        config.NewConfig<Product, UpdateProductDto>()
            .Map(destination => destination.Sku, source => source.SKU);
        config.NewConfig<UpdateProductDto, Product>()
            .Map(destination => destination.SKU, source => source.Sku);
    }
}
