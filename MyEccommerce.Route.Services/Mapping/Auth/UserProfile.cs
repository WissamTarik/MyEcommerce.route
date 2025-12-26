using AutoMapper;
using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Mapping.Auth
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
