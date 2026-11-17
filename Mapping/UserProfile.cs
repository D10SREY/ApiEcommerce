using System;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using Mapster;

namespace ApiEcommerce.Mapping;

public class UserProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(destination => destination.Id, source => source.Id.ToString());
        config.NewConfig<UserDto, User>()
            .Map(destination => destination.Id, source => int.Parse(source.Id));

        config.NewConfig<User, CreateUserDto>()
            .Map(destination => destination.Username, source => source.UserName);
        config.NewConfig<CreateUserDto, User>()
            .Map(destination => destination.UserName, source => source.Username);

        config.NewConfig<User, UserLoginDto>()
            .Map(destination => destination.Username, source => source.UserName);
        config.NewConfig<UserLoginDto, User>()
            .Map(destination => destination.UserName, source => source.Username);

        config.NewConfig<User, UserLoginResponseDto>();
        config.NewConfig<UserLoginResponseDto, User>();

        config.NewConfig<ApplicationUser, UserDataDto>();
        config.NewConfig<UserDataDto, ApplicationUser>();

        config.NewConfig<ApplicationUser, UserDto>();
        config.NewConfig<UserDto, ApplicationUser>();
    }
}
