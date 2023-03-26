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

        }
    }
}
