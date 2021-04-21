using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Migrations.Data
{
   public interface IRepository<T> where T : class,IEntity
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
        Task<List<T>> DeleteAll(int id);
        Task<T> DeleteById(int id, int dishId);
        Task<List<T>> GetAllDishesFromRestaurant(int restaurantId);
    }
}
