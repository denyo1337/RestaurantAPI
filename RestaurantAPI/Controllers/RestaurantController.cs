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
        public async Task<ActionResult<RestaurantDTO>> GetAll([FromQuery] RestaurantQuery query)
        {
            var result =  await restaurantService.GetAll(query);
            return Ok(result);
        } 

       [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDTO>> Get([FromRoute]int id)
        {
            var result = await restaurantService.GetById(id); // bez await i taska - po prostu Result
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles ="Admin,Manager")]
        public async Task<ActionResult> CreateRestaurant([FromBody]CreateRestaurantDTO dto)
        {
            var id = await restaurantService.CreateAsync(dto);   
            return Created($"/api/restaurant/{id}",null);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
           var result = await restaurantService.Delete(id);
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
