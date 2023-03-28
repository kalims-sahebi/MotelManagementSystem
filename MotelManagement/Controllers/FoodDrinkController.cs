using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class FoodDrinkController : Controller
    {
        
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(FoodDrinkModel model)
        {
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.Name,
                    model.Price,
                    model.Type
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddFoodDrink", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("Add");
            }
            else
            {
                return View(model);
            }
            
        }


        #region Useless

        // Deleting food or Drink is impossible if they are
        // already taken in an order because of primary key conflict exception
        //therefore the list for both is also not needed

        /*[HttpGet]
        public IActionResult DrinkList()
        {
            var ViewModel = new FoodDrinkModel() { };
            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.List = sql.Query<FoodDrinkModel>("dbo.GetDrinkList", commandType: CommandType.StoredProcedure);
            return View(ViewModel);
        }
        public IActionResult DeleteDrink(int id)
        {
            var template = new { DrinkId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            sql.Query("dbo.DeleteDrink", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("DrinkList");
        }
        [HttpGet]
        public IActionResult FoodList()
        {
            var ViewModel = new FoodDrinkModel() { };
            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.List = sql.Query<FoodDrinkModel>("dbo.GetFoodList", commandType: CommandType.StoredProcedure);
            return View(ViewModel);
        }
        public IActionResult DeleteFood(int id)
        {
            var template = new { FoodId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            sql.Query("dbo.DeleteFood", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("FoodList");
        }*/

        #endregion Useless
    }
}
