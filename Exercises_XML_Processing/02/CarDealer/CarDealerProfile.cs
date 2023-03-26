using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supllier
            this.CreateMap<ImportSupplierDto, Supplier>();

            //Parts 
            this.CreateMap<ImportPartDto, Part>();

            //Cars
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());

            // Customer 
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate,
                    opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate)));

        }
    }
}
