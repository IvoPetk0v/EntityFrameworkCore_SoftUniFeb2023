using AutoMapper;
using Boardgames;

namespace Trucks.Utulities
{
    public static class MyMapper
    {

        public static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BoardgamesProfile>()));
        }
    }
}
