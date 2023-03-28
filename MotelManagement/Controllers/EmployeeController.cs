using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using MotelManagement.ViewModels;
using ShamisDateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static MotelManagement.DataAccess.MyTables;

namespace MotelManagement.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class EmployeeController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            EmployeeViewModel model = new()
            {
                Duty = DutyType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.DutyId.ToString(),
                    Text = a.Duty
                }),
                EmployeeType = EmployeeType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.EmployeeTypeId.ToString(),
                    Text = a.EmployeeType
                }),
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(EmployeeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.FullName,
                    model.Phone,
                    model.Address,
                    model.Balance,
                    model.MonthlySalary,
                    model.DutyId,
                    model.EmployeeTypeId,
                    model.Date,
                    model.OtherBenefit,

                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddEmployee", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("Add");
            }
            else
            {
                model.Duty = DutyType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.DutyId.ToString(),
                    Text = a.Duty
                });
                model.EmployeeType = EmployeeType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.EmployeeTypeId.ToString(),
                    Text = a.EmployeeType
                });
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult List()
        {
            var ViewModel = new EmployeeListViewModel() { };
            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.Employee = sql.Query<EmployeeModel>("dbo.GetEmployeeList", commandType: CommandType.StoredProcedure);

            return View(ViewModel);
        }

        public IActionResult Delete(int id)
        {
            var template = new { EmployeeId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            sql.Query("dbo.DeleteEmployee", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("EmployeeT");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var template = new
            {
                EmployeeId = id
            };

            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var ViewModel = new EmployeeViewModel() { };

            ViewModel = sql.Query<EmployeeViewModel>("dbo.SelectEmployeeToEdit", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            if(ViewModel != null)
            ViewModel.DateString = new ShamisDateTime.PersianDateTime(ViewModel.Date)
                .ToShortDateString();

            ViewModel.Duty = DutyType.GetAll().Select(a => new SelectListItem
            {
                Value = a.DutyId.ToString(),
                Text = a.Duty,
                Selected = a.DutyId == ViewModel.DutyId
            });
            ViewModel.EmployeeType = EmployeeType.GetAll().Select(a => new SelectListItem
            {
                Value = a.EmployeeTypeId.ToString(),
                Text = a.EmployeeType,
                Selected = a.EmployeeTypeId == ViewModel.EmployeeTypeId
            });
            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.EmployeeId,
                    model.FullName,
                    model.Phone,
                    model.Address,
                    model.Balance,
                    model.MonthlySalary,
                    model.DutyId,
                    model.EmployeeTypeId,
                    model.Date,
                    model.OtherBenefit,

                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.EditEmployee", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }
        
        [HttpGet]
        public IActionResult EmployeeT()
        {
            var model = new EmployeeTViewModel()
            {
                Employee = Employee.GetAll().Select(a => new SelectListItem
                {
                    Value = a.EmployeeId.ToString(),
                    Text = a.FullName
                })
            };
            using SqlConnection sql = new(Startup.ConnectionString);
            model.TList = sql.Query<EmployeeTViewModel>("dbo.GetTList", commandType: CommandType.StoredProcedure);
            return View(model);
        }

        [HttpPost]
        public IActionResult EmployeeT(EmployeeTViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
                model.user = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.EmployeeId,
                    model.Amount,
                    model.inOut,
                    model.Date,
                    model.Description,
                    model.user
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddTransaction", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("EmployeeT");
            }
            else
            {
                model.Employee = Employee.GetAll().Select(a => new SelectListItem
                {
                    Value = a.EmployeeId.ToString(),
                    Text = a.FullName
                });
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteT(int id)
        {
            var template = new { EmployeeTransactionId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            await sql.QueryAsync("dbo.DeleteT", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("EmployeeT");
        }
        [HttpGet]
        public IActionResult EditT(int id)
        {
            var template = new { EmployeeTransactionId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var model = new EmployeeTViewModel() { };
            model = sql.Query<EmployeeTViewModel>("dbo.selectTransactionToedit", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();


            model.Employee = Employee.GetAll().Select(a => new SelectListItem
            {
                Value = a.EmployeeId.ToString(),
                Text = a.FullName,
                Selected = (a.EmployeeId == model.EmployeeId)
            });
            model.DateString = new ShamisDateTime.PersianDateTime(model.Date).ToShortDateString();
            model.TList = sql.Query<EmployeeTViewModel>("dbo.GetTList", commandType: CommandType.StoredProcedure);
            return View("EmployeeT", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditT(EmployeeTViewModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            model.user = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.EmployeeId,
                    model.EmployeeTransactionId,
                    model.Amount,
                    model.inOut,
                    model.Date,
                    model.Description,
                    model.user
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                await sql.QueryAsync("dbo.EditTransaction", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("EmployeeT");
            }
            else
            {
                model.Employee = Employee.GetAll().Select(a => new SelectListItem
                {
                    Value = a.EmployeeId.ToString(),
                    Text = a.FullName
                });
                return View(model);
            }
        }
    }
}

