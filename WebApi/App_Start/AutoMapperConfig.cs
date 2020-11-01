using AutoMapper;
using Domain;
using Domain.Entity;
using WebApi.Infrastructure.Extensions;
using WebApi.Infrastructure.Handlers.Features.Mediation;

#pragma warning disable 618

namespace WebApi
{
    public class AutoMapperConfig
    {
        public static void RegisterMapping()
        {
            Mapper.CreateMap<ResponseModel, ResponseEntity>().Bidirectional();
            Mapper.CreateMap<User, tblUser>().Bidirectional();
            //Mapper.CreateMap<GetUserResponse, User>().Bidirectional();
            //Mapper.CreateMap<UserUpdateModel, User>().Bidirectional();
            //Mapper.CreateMap<UserResponseModel, User>().Bidirectional();
        }
    }
}