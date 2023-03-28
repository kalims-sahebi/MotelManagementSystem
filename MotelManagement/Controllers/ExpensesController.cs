using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using MotelManagement.ViewModels;
using ShamisDateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(ExpensesModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            model.user = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.Description,
                    model.Date,
                    model.Amount,
                    model.user
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddExpenses", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
                return View(model);
            
        }

        [HttpGet]
        public IActionResult List()
        {
            var ViewModel = new ExpensesListViewModel() { };
            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.ExpensesList = sql.Query<ExpensesModel>("dbo.GetExpensesList", commandType: CommandType.StoredProcedure);
            return View(ViewModel);
        }

        public IActionResult Delete(int id)
        {
            var template = new { ExpensesId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            sql.Query("dbo.DeleteExpenses", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var template = new
            {
                ExpensesId = id
            };

            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var ViewModel = new ExpensesModel() { };

            ViewModel = sql.Query<ExpensesModel>("dbo.SelectExpensesToEdit", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            if (ViewModel != null)
                ViewModel.DateString = new ShamisDateTime.PersianDateTime(ViewModel.Date)
                    .ToShortDateString();

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Edit(ExpensesModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            model.user = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.ExpensesId,
                    model.Description,
                    model.Date,
                    model.Amount,
                    model.user
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.EditExpenses", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
                return View(model);
        }
    }
}
