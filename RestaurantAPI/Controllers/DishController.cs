using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }
        [HttpPost]
       public async Task<ActionResult> CreateDish([FromRoute] int restaurantId, [FromBody]CreateDishDTO dto)
        {
            var newDishId = await _service.CreateDish(restaurantId, dto);
            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}",null);
       }  
        [HttpGet("{dishId}")]
       public async Task<ActionResult<DishDTO>> Get([FromRoute]int restaurantId ,[FromRoute] int dishId)
        {
            DishDTO dish = await _service.GetById(restaurantId, dishId);
            return Ok(dish);
        }
        [HttpGet]
        public async Task< ActionResult<List<DishDTO>>> GetDishes([FromRoute] int restaurantId)
        {
            var dishes = await _service.GetAll(restaurantId);
            return Ok(dishes);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAllDishes([FromRoute]int restaurantId)
        {
            await _service.RemoveAll(restaurantId);
            return NoContent();
        }
        [HttpDelete("{dishId}")]
        public async Task<ActionResult> DeleteDishById([FromRoute] int restaurantId,[FromRoute]int dishId)
        {
            await _service.RemoveById(restaurantId, dishId);
            return NoContent();
        }
    }

}
