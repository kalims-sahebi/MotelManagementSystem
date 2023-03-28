using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using MotelManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public IActionResult Weekly()
        {
            var model = new ReportViewModel() { };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            model.Report =  sql.Query<ReportModel>("dbo.GetWeeklyReport", commandType: CommandType.StoredProcedure);
            return View(model);
        }
        public IActionResult Monthly()
        {
            var model = new ReportViewModel() { };
            using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
            model.Report = sql.Query<ReportModel>("dbo.GetMonthlylyReport", commandType: CommandType.StoredProcedure);
            return View(model);
        }
    }
}
