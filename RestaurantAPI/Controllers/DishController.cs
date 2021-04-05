﻿using Microsoft.AspNetCore.Mvc;
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
       public ActionResult Post([FromRoute] int restaurantId, [FromBody]CreateDishDTO dto)
        {
            var newDishId = _service.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}",null);
       
       }  
        [HttpGet("{dishId}")]
       public ActionResult<DishDTO> Get([FromRoute]int restaurantId ,[FromRoute] int dishId)
        {
            DishDTO dish = _service.GetById(restaurantId, dishId);

            return Ok(dish);
        }
        [HttpGet]
        public ActionResult<List<DishDTO>> GetDishes([FromRoute] int restaurantId)
        {
            var dishes = _service.GetAll(restaurantId);
            return Ok(dishes);
        }
        [HttpDelete]
        public ActionResult DeleteAllDishes([FromRoute]int restaurantId)
        {
            _service.RemoveAll(restaurantId);

            return NoContent();
        }
        [HttpDelete("{dishId}")]
        public ActionResult DeleteDishById([FromRoute] int restaurantId,[FromRoute]int dishId)
        {
            _service.RemoveById(restaurantId, dishId);

            return NoContent();
        }
    }

}
