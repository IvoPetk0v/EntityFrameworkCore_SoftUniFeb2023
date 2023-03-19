namespace ProductShop
{
    using AutoMapper;

    using DTOs.Import;
    using Models;
    using ProductShop.DTOs.Export;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Users
            this.CreateMap<ImportUserDto, User>();

            this.CreateMap<User, UserSoldProductDto>()
                .ForMember(d => d.FirstName,
                 opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName,
                opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.SoldProducts,
                opt => opt.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            //Products
            this.CreateMap<ImportProductDto, Product>();

            this.CreateMap<Product, ProductInRangeDto>()
                .ForMember(d => d.ProductName,
                    opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ProductPrice,
                    opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.SellerName,
                    opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            this.CreateMap<Product, SoldProductDto>()
                .ForMember(d => d.ProductName,
                    opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Price,
                    opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.BuyerFirstName,
                    opt => opt.MapFrom(s => s.Buyer!.FirstName))
                .ForMember(d => d.BuyerLastName,
                    opt => opt.MapFrom(s => s.Buyer!.LastName));



            //Categories
            this.CreateMap<ImportCategoryDto, Category>();

            //CategoryProduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
