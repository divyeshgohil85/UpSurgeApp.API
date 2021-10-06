using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpSurgeApp.API.Dtos;

namespace UpSurgeApp.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<OTPDto, MobileOTP>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Country, CountryDto>();
            //CreateMap<testc, testDto>().ReverseMap();
        }
    }
}
