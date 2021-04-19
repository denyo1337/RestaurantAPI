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
      //[Authorize(Policy = "CreatedAtleast2Restaurants")] works fine 
      public ActionResult<RestaurantDTO> GetAll([FromQuery] RestaurantQuery query)
        {
            var result =  restaurantService.GetAll(query);
            return Ok(result);
        } 

       [HttpGet("{id}")]
      public ActionResult<RestaurantDTO> Get([FromRoute]int id)
        {
            var result =  restaurantService.GetById(id).Result; // bez await i taska - po prostu Result
            if (result == null)
                throw new NotFoundExpection("Restauracja nie istnieje");

            return Ok(result);
            
        }
        [HttpPost]
        [Authorize(Roles ="Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDTO dto)
        {
          

           var id =  restaurantService.CreateAsync(dto).Result;
            
            return Created($"/api/restaurant/{id}",null);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            var result = await restaurantService.Delete(id);
            if (result == null)
                throw new NotFoundExpection("Restauracja, którą chcesz usunać nie istnieje");

            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> UpdateAsync([FromRoute] int id,[FromBody]UpdateRestaurantDTO dto)
        {
           
          await restaurantService.UpdateAsync(id, dto);
           

            return NoContent();
        }
    }
}
