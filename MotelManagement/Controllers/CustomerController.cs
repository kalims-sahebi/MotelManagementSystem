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
    [Authorize]
    public class CustomerController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            var model = new CustomerModel()
            {
                RoomList = Room.GetAll().Select(a => new SelectListItem
                {
                    Value = a.RoomId.ToString(),
                    Text = a.RoomNumber
                })
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Add(CustomerModel model)
        {
            if (!string.IsNullOrEmpty(model.EntryDateString))
                model.EntryDate = PersianDateTime.Parse(model.EntryDateString).ToDateTime();
            model.user = User.Identity.Name;
            //By default a customer is not out while adding him to the list
            //So I set the is out value to false it means customer is not out
            model.IsOut = 0;
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.FullName,
                    model.Phone,
                    model.Address,
                    model.EntryDate,
                    model.ExitDate,
                    model.RoomId,
                    model.IsOut,
                    model.user

                };

                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddCustomer", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
            {
                model.RoomList = Room.GetAll().Select(a => new SelectListItem
                {
                    Value = a.RoomId.ToString(),
                    Text = a.RoomNumber
                });
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult List()
        {
            var ViewModel = new CustomerViewModel()
            {
                DrinkList = Drink.GetAll().Select(f => new SelectListItem
                {
                    Value = f.DrinkId.ToString(),
                    Text = f.Drink
                }),
                FoodList = Food.GetAll().Select(s => new SelectListItem
                {
                    Value = s.FoodId.ToString(),
                    Text = s.Food
                })
            };

            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.CustomerList = sql.Query<CustomerModel>("dbo.GetCustomerList", commandType: CommandType.StoredProcedure);
            return View(ViewModel);
        }

        [HttpGet]
        public IActionResult ListExit()
        {
            var ViewModel = new CustomerViewModel() { };
            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.CustomerList = sql.Query<CustomerModel>("dbo.GetExitCustomerList", commandType: CommandType.StoredProcedure);

            return View(ViewModel);
        }

        //Ajax POst ORder Here
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AjaxOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.DrinkId,
                    model.DrinkCount,
                    model.FoodId,
                    model.FoodCount,
                    model.CustomerId
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                await sql.QueryAsync("AddOrder", parameters, commandType: CommandType.StoredProcedure);
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }

        } 

      /* [HttpGet]
        public  IActionResult OrderList(int id)
        {
            var template = new 
            {
                CustomerId = id
            };
            var ViewModel = new OrderRemoveViewModel() { };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            ViewModel.RemoveOrder = sql.Query<OrderRemoveModel>("dbo.GetOrderToRemove", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("FullName");

            return View(ViewModel);
        }*/

        [HttpGet]
        public async  Task<IActionResult> OrderList(int id)
        {

            var template = new
            {
                CustomerId = id
            };
            var parameters = new DynamicParameters(template);
            

            var model = new BillViewModel() { };

            parameters.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            model.DrinkBill = await sql.QueryAsync<BillModel>("dbo.GetDrinkBill", new { CustomerId = id }, commandType: CommandType.StoredProcedure);
            model.FoodBill = await sql.QueryAsync<BillModel>("dbo.GetFoodBill", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("FullName");
            
            return View(model);
        }

        public IActionResult DeleteOrder(int id)
        {
            var template = new { OrderId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
             sql.Query("dbo.DeleteOrder", parameters, commandType: CommandType.StoredProcedure);
            TempData["fullname"] = parameters.Get<string>("Msg");
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var template = new { CustomerId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            await sql.QueryAsync("dbo.DeleteCustomer", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("ListExit");
        }

        //Ajax POst New Food Here
        [HttpPost]
        public async Task<IActionResult> AddNewFood([FromBody] FoodModel model)
        {
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.Food,
                    model.Price
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                await sql.QueryAsync("AddNewFood", parameters, commandType: CommandType.StoredProcedure);
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }

        //Ajax POst New Drink Here
        [HttpPost]
        public async Task<IActionResult> AddNewDrink([FromBody] AjaxDrinkModel model)
        {
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.Drink,
                    model.DPrice
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                await sql.QueryAsync("dbo.AddNewDrink", parameters, commandType: CommandType.StoredProcedure);
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }


        //Ajax collects this data to populate dropdown list(refresh list)
        [HttpPost]
        public IActionResult GetFoodList()
        {
            return Json(Food.GetAll()
               .Select(f => new SelectListItem
               {
                   Value = f.FoodId.ToString(),
                   Text = f.Food
               }));
        }

        //Ajax collects this data to populate dropdown list(refresh list)
        [HttpPost]
        public IActionResult GetDrinkList()
        {
            return Json(Drink.GetAll()
               .Select(d => new SelectListItem
               {
                   Value = d.DrinkId.ToString(),
                   Text = d.Drink
               }));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var template = new
            {
                CustomerId = id
            };

            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var ViewModel = new CustomerModel() { };

            ViewModel = sql.Query<CustomerModel>("dbo.SelectCustomerToEdit", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            if (ViewModel != null)
                ViewModel.EntryDateString = new ShamisDateTime.PersianDateTime(ViewModel.EntryDate)
                    .ToShortDateString();

            ViewModel.RoomList = Room.GetAll().Select(a => new SelectListItem
            {
                Value = a.RoomId.ToString(),
                Text = a.RoomNumber,
                Selected = a.RoomId== ViewModel.RoomId
            });
            
            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Edit(CustomerModel model)
        {
            if (!string.IsNullOrEmpty(model.EntryDateString))
                model.EntryDate = PersianDateTime.Parse(model.EntryDateString).ToDateTime();
            model.user = User.Identity.Name;
            
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.CustomerId,
                    model.FullName,
                    model.Phone,
                    model.Address,
                    model.EntryDate,
                    model.RoomId,
                    model.user

                };

                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.EditCustomer", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
            {
                model.RoomList = Room.GetAll().Select(a => new SelectListItem
                {
                    Value = a.RoomId.ToString(),
                    Text = a.RoomNumber
                });
                return View(model);
            }
        }


        //Ajax POst Method
        [HttpPost]
        public void SetRoomFull([FromBody] roomID model)
        {
            var temp = new
            {
                model.RoomId
            };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            sql.Query("dbo.SetRoomFull", temp, commandType: CommandType.StoredProcedure);
        }
    
    
        public async Task<IActionResult> PrintBill(int id)
        {
            var template = new
            {
                CustomerId = id
            };
            var parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var parameters2 = new DynamicParameters(template);
            parameters2.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            
            var model = new BillViewModel() { };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            model.DrinkBill = await sql.QueryAsync<BillModel>("dbo.GetDrinkBill", new { CustomerId = id }, commandType: CommandType.StoredProcedure);
            model.FoodBill = await sql.QueryAsync<BillModel>("dbo.GetFoodBill", parameters2, commandType: CommandType.StoredProcedure);
            model.Room = sql.Query<RoomBill>("dbo.GetRoomBill", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

            TempData["Message"] = parameters2.Get<string>("FullName");



            return View(model);
            
        }
    
        [HttpGet]
        public IActionResult PrintByNumber()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PrintByNumber(PrintByNumberModel modell)
        {
            var template = new
            {
                Phone = modell.Phone
            };
            //Checking Phone  number if exist
            PhoneConditionModel temp;
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            temp = sql.Query<PhoneConditionModel>("dbo.PhoneCondition", new { Phone = modell.Phone }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            //Checking Phone  number if exist
            if (temp == null)
            {
                TempData["Message"] = " این شماره وجود ندارد";
                return View("PrintByNumber");
            }
            else
            {
                var parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var parameters2 = new DynamicParameters(template);
                parameters2.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var model = new BillViewModel() { };
                model.DrinkBill = await sql.QueryAsync<BillModel>("dbo.GetDrinkBillByNumber", new { Phone = modell.Phone }, commandType: CommandType.StoredProcedure);
                model.FoodBill = await sql.QueryAsync<BillModel>("dbo.GetFoodBillByNumber", parameters2, commandType: CommandType.StoredProcedure);
                model.Room = sql.Query<RoomBill>("dbo.GetRoomBillByNumber", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

                TempData["Message"] = parameters2.Get<string>("FullName");

                return View("PrintBill", model);
            }
            
        }


        [HttpGet]
        public IActionResult PrintByRoom()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PrintByRoom(PrintByRoomModel modell)
        {
            //print by room only when the given room is full/taken not empty
            var template = new
            {
                Room = modell.Room
            };
            //Checking room condition full/empty
            RoomConditionModel temp;
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            temp = sql.Query<RoomConditionModel>("dbo.RoomCondition", new { Room = modell.Room }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            //Checking room condition full/empty
            if (temp == null || temp.IsEmpty == 0)
            {
                TempData["Message"] = " این اتاق خالی است یا وجود ندارد";
                return View("PrintByRoom");
            }
            else
            {
                var parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var parameters2 = new DynamicParameters(template);
                parameters2.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var model = new BillViewModel() { };
                model.DrinkBill = await sql.QueryAsync<BillModel>("dbo.GetDrinkBillByRoom", new { Room = modell.Room }, commandType: CommandType.StoredProcedure);
                model.FoodBill = await sql.QueryAsync<BillModel>("dbo.GetFoodBillByRoom", parameters2, commandType: CommandType.StoredProcedure);
                model.Room = sql.Query<RoomBill>("dbo.GetRoomBillByRoom", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

                TempData["Message"] = parameters2.Get<string>("FullName");

                return View("PrintBill", model);
            }
        }

    }
}
