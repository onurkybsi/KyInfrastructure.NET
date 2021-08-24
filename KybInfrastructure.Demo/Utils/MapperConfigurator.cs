using AutoMapper;

namespace KybInfrastructure.Demo.Utils
{
    public static class MapperConfigurator
    {
        private static Mapper Mapper = new(new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<Data.User, Business.User>().ReverseMap();
                cfg.CreateMap<Data.Product, Business.Product>().ReverseMap();
            }
        ));

        public static TDest MapTo<TDest>(this object src)
            => (TDest)Mapper.Map<TDest>(src);
    }
}
