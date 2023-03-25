using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supplyer
            this.CreateMap<ImportSupplyerDto, Supplier>();

            //Parts 
            this.CreateMap<ImportPartDto, Part>();

            //Cars
            this.CreateMap<ImportCarDto, Car>();

            //Customers
            this.CreateMap<ImportCustomerDto,Customer>();


        }
    }
}
