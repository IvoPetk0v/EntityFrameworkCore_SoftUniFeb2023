namespace ProductShop
{
    using AutoMapper;

    using DTOs.Import;
    using Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Users
            this.CreateMap<ImportUserDto, User>();

            //Products
            this.CreateMap<ImportProductDto, Product>();

            //Categories
            this.CreateMap<ImportCategoryDto, Category>();

        }
    }
}
