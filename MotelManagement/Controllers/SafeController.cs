using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MotelManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class SafeController : Controller
    {
        [HttpGet]
        public IActionResult Status()
        {
            var model = new SafeBalanceViewModel(){ };
            using SqlConnection sql = new(Startup.ConnectionString);
            model = sql.Query<SafeBalanceViewModel>("dbo.GetSafeTotal", commandType: CommandType.StoredProcedure).SingleOrDefault();

            return View(model);
        }

        [HttpPost]
        public IActionResult Status(SafeBalanceViewModel model)
        {

                var temp = new
                {
                    model.MoneyIn,
                    model.MoneyOut
                };

                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                DynamicParameters parameters = new DynamicParameters(temp);
                parameters.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                sql.Query("dbo.SafeinoutBalance", parameters, commandType: CommandType.StoredProcedure);
                TempData["Message"] = parameters.Get<string>("Msg");
                return RedirectToAction("Status");
   
        }
    }
}
