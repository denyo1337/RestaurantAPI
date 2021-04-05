using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
      
        private readonly IRestaurantService restaurantService;
        
        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;

        }
        [HttpGet]
       // [Authorize(Policy = "Atleast20")] just for testing 
      [Authorize(Policy = "CreatedAtleast2Restaurants")]
      public ActionResult<RestaurantDTO> GetAll()
        {
            var result =  restaurantService.GetAll();
            return Ok(result);
        } 

       [HttpGet("{id}")]
      public ActionResult<RestaurantDTO> Get([FromRoute]int id)
        {
            var result = restaurantService.GetById(id);
            

            return Ok(result);
            
        }
        [HttpPost]
        [Authorize(Roles ="Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDTO dto)
        {
          

           var id =  restaurantService.Create(dto);
            
            return Created($"/api/restaurant/{id}",null);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete([FromRoute] int id)
        {
            restaurantService.Delete(id);
           

            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id,[FromBody]UpdateRestaurantDTO dto)
        {
           
           restaurantService.Update(id, dto);
           

            return NoContent();
        }
    }
}
