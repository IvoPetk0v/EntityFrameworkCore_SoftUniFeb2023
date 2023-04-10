namespace Boardgames
{
    using AutoMapper;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.DataProcessor.ImportDto;
    using System.Diagnostics;
    using System.Security.AccessControl;

    public class BoardgamesProfile : Profile
    {
        // DO NOT CHANGE OR RENAME THIS CLASS!
        public BoardgamesProfile()
        {

            this.CreateMap<ImportCreatorDto, Creator>()
                .ForSourceMember(d => d.Boardgames, opt => opt.DoNotValidate());

            this.CreateMap<ImportBoardGameDto, Boardgame>()
                .ForMember(d => d.CategoryType, opt => opt.MapFrom(s => (CategoryType)s.CategoryType));

            this.CreateMap<ImportSellerDto, Seller>()
                .ForSourceMember(d => d.Boardgames, opt => opt.DoNotValidate());

            this.CreateMap<Creator, ExportCreatorDto>()
                    .ForMember(d => d.CreatorName, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
                    .ForMember(d => d.BoardgamesCount, opt => opt.MapFrom(s => s.Boardgames.Count))
                    .ForMember(d => d.Boardgames, opt => opt.MapFrom(s => s.Boardgames));

            this.CreateMap<Boardgame, ExportBoardGameDto>()
                   .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                   .ForMember(d => d.YearPublished, opt => opt.MapFrom(s => s.YearPublished));
        }
    }
}