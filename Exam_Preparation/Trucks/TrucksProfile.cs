namespace Trucks
{
    using AutoMapper;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class TrucksProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TrucksProfile()
        {
            this.CreateMap<ImportDespatcherDto, Despatcher>()
                .ForSourceMember(d => d.Trucks, opt => opt.DoNotValidate());

            this.CreateMap<ImportTruckSubDto, Truck>()
                .ForMember(d => d.CategoryType, opt => opt.MapFrom(s => (CategoryType)s.CategoryType))
                .ForMember(d => d.MakeType, opt => opt.MapFrom(s => (MakeType)s.MakeType));

            this.CreateMap<ImportClientDto, Client>()
                 .ForSourceMember(d => d.Trucks, opt => opt.DoNotValidate());
        }
    }
}
