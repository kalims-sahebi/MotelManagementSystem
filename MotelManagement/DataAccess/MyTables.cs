using Dapper;
using Microsoft.Data.SqlClient;
using MotelManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotelManagement.DataAccess
{
    public static class MyTables
    {
        public static class DutyType
        {
            public static IEnumerable<DutyModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<DutyModel>("dbo.GetDuty");
            }
        }
        public static class Drink
        {
            public static IEnumerable<DrinkModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<DrinkModel>("dbo.GetDrink");
            }
        }
        public static class Food
        {
            public static IEnumerable<FoodModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<FoodModel>("dbo.GetFood");
            }
        }
        public static class EmployeeType
        {
            public static IEnumerable<EmployeeTypeModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<EmployeeTypeModel>("dbo.GetEmployeeType");
            }
        }
        public static class RoomType
        {
            public static IEnumerable<RoomTypeModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<RoomTypeModel>("dbo.GetRoomType");
            }
        }

        public static class Employee
        {
            public static IEnumerable<EmployeeModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<EmployeeModel>("dbo.GetEmployee");
            }
        }
        
        public static class Room
        {
            public static IEnumerable<RoomModel> GetAll()
            {
                using SqlConnection sql = new SqlConnection(Startup.ConnectionString);
                return sql.Query<RoomModel>("dbo.GetRoom");
            }
        } 
        
    }
}
