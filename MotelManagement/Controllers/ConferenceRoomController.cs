using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using MotelManagement.ViewModels;
using ShamisDateTime;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class ConferenceRoomController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(ConferenceRoomModel model)
        {
            model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            model.User = User.Identity.Name;
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.OrganizationName,
                    model.Address,
                    model.contact,
                    model.Date,
                    model.StartTime,
                    model.EndTime,
                    model.RefreshmentCost,
                    model.RentPerHour,
                    model.User
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.AddConference", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
                return View(model);
        }

        [HttpGet]
        public IActionResult List()
        {
            var ViewModel = new ConferenceListViewModel() { };

            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.ConferenceList = sql.Query<ConferenceRoomModel>("dbo.GetConferenceList", commandType: CommandType.StoredProcedure);
            return View(ViewModel);
        }

        [HttpGet]
        public IActionResult ListExit()
        {
            var ViewModel = new ConferenceListViewModel() { };

            using SqlConnection sql = new(Startup.ConnectionString);
            ViewModel.ConferenceList = sql.Query<ConferenceRoomModel>("dbo.GetConferenceListExit", commandType: CommandType.StoredProcedure);
            return View("List", ViewModel);
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var template = new { ConferenceRoomId = id };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            sql.Query("dbo.DeleteConference", parameters, commandType: CommandType.StoredProcedure);
            TempData["Message"] = parameters.Get<string>("Msg");
            return RedirectToAction("ListExit");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var template = new {ConferenceRoomId = id};

            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            DynamicParameters parameters = new DynamicParameters(template);
            parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            var ViewModel = new ConferenceRoomModel() { };

            ViewModel = sql.Query<ConferenceRoomModel>("dbo.SelectConferenceToEdit", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            if (ViewModel != null)
                ViewModel.DateString = new ShamisDateTime.PersianDateTime(ViewModel.Date)
                    .ToShortDateString();

            return View(ViewModel);
           
        }
        [HttpPost]
        public IActionResult Edit(ConferenceRoomModel model)
        {
            if (!string.IsNullOrEmpty(model.DateString))
                model.Date = PersianDateTime.Parse(model.DateString).ToDateTime();
            if (ModelState.IsValid)
            {
                var template = new
                {
                    model.ConferenceRoomId,
                    model.OrganizationName,
                    model.Address,
                    model.contact,
                    model.Date,
                    model.StartTime,
                    model.EndTime,
                    model.RefreshmentCost,
                    model.RentPerHour,
                    
                };
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(template);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.EditConference", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("List");
            }
            else
            {
                return View(model);
            }
        }
    
        [HttpGet]
        public IActionResult OrderList(int id)
        {
            var template = new
            {
                ConferenceRoomId = id
            };
            var parameters = new DynamicParameters(template);


            var model = new ConferenceBillViewModel() { };
            parameters.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            model =  sql.Query<ConferenceBillViewModel>("dbo.GetConferenceBill", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
           
            double duration = model.Duration;
            double RentPerHour = model.RentPerHour;
            double RefreshmentCost = model.RefreshmentCost;
            TempData["duration"] = (duration / 60);
            model.total = ((duration / 60) * RentPerHour)+RefreshmentCost;

            TempData["Message"] = parameters.Get<string>("FullName");
            return View(model);
        }

        [HttpGet]
        public IActionResult PrintBill(int id)
        {
            var template = new
            {
                ConferenceRoomId = id
            };
            var parameters = new DynamicParameters(template);


            var model = new ConferenceBillViewModel() { };
            parameters.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            parameters.Add("@msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            model = sql.Query<ConferenceBillViewModel>("dbo.PrintConferenceBill", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

            double duration = model.Duration;
            double RentPerHour = model.RentPerHour;
            double RefreshmentCost = model.RefreshmentCost;
            TempData["duration"] = (duration / 60);
            model.total = ((duration / 60) * RentPerHour) + RefreshmentCost;

            TempData["Message"] = parameters.Get<string>("FullName");
            TempData["msg"] = parameters.Get<string>("msg");
            return View(model);
        }


        [HttpGet]
        public IActionResult PrintByNumber()
        {
            return View();
        }


        [HttpPost]
        public IActionResult PrintByNumber(PrintConferenceByNumberModel modell)
        {
            var template = new
            {
                Contact = modell.Contact
            };
            //Checking Phone  number if exist
            ConferencePhoneConditionModel temp;
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            temp = sql.Query<ConferencePhoneConditionModel>("dbo.ConferencePhoneCondition", new { Contact = modell.Contact }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            //Checking Phone  number if exist
            if (temp == null)
            {
                TempData["Message"] = " این شماره وجود ندارد";
                return View("PrintByNumber");
            }
            else
            {
                var parameters = new DynamicParameters(template);


                var model = new ConferenceBillViewModel() { };
                parameters.Add("@FullName", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                parameters.Add("@msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                model = sql.Query<ConferenceBillViewModel>("dbo.PrintConferenceBillByNumber", parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();

                double duration = model.Duration;
                double RentPerHour = model.RentPerHour;
                double RefreshmentCost = model.RefreshmentCost;
                TempData["duration"] = (duration / 60);
                model.total = ((duration / 60) * RentPerHour) + RefreshmentCost;

                TempData["Message"] = parameters.Get<string>("FullName");
                TempData["msg"] = parameters.Get<string>("msg");
                return View("PrintBill",model);
            }

        }

    }

}
