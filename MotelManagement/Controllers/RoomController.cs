using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using MotelManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static MotelManagement.DataAccess.MyTables;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            var model = new RoomModel() 
            {
                RoomTypeList = RoomType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.RoomTypeId.ToString(),
                    Text = a.RoomType
                })
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(RoomModel model)
        {
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.RoomNumber,
                    model.IsEmpty,
                    model.RoomLocation,
                    model.BedCount,
                    model.RentPerNight,
                    model.RoomTypeId,

                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddRoom", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
            {
                model.RoomTypeList = RoomType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.RoomTypeId.ToString(),
                    Text = a.RoomType
                });
                return View(model);
            }
        }

        public IActionResult List()
        {
            var ViewModel = new RoomListViewModel() { };
            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.RoomList = sql.Query<RoomModel>("dbo.GetRoomList", commandType: CommandType.StoredProcedure);
            return View(ViewModel);
        }

        public IActionResult Delete(int id)
        {
            var template = new { RoomId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            sql.Query("dbo.DeleteRoom", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var template = new
            {
                RoomId = id
            };

            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var ViewModel = new RoomModel() { };

            ViewModel = sql.Query<RoomModel>("dbo.SelectRoomToEdit", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

            ViewModel.RoomTypeList = RoomType.GetAll().Select(a => new SelectListItem
            {
                Value = a.RoomTypeId.ToString(),
                Text = a.RoomType,
                Selected = a.RoomTypeId == ViewModel.RoomTypeId
            });
            
            return View(ViewModel);
           
        }

        [HttpPost]
        public IActionResult Edit(RoomModel model)
        {
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.RoomId,
                    model.RoomNumber,
                    model.IsEmpty,
                    model.RoomLocation,
                    model.BedCount,
                    model.RentPerNight,
                    model.RoomTypeId,

                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.EditRoom", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
            {
                model.RoomTypeList = RoomType.GetAll().Select(a => new SelectListItem
                {
                    Value = a.RoomTypeId.ToString(),
                    Text = a.RoomType
                });
                return View(model);
            }
        }

    }
}
