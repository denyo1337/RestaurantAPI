using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(m=>m.City, c=>c.MapFrom(s=>s.Address.City))
                .ForMember(m=>m.Street, c=>c.MapFrom(s=>s.Address.Street))
                .ForMember(m=>m.PostalCode, c=>c.MapFrom(s=>s.Address.PostalCode));

            CreateMap<Dish, DishDTO>().ReverseMap()
                .ForMember(x=>x.RestaurantId, s=>s.MapFrom(o=>o.RestaurantId));

            //mapped specifically each property
            CreateMap<CreateRestaurantDTO, Restaurant>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => new Address() { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
            CreateMap<Dish, CreateDishDTO>().ReverseMap();

                
        }
    }
}
