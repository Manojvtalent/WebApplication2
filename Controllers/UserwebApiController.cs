using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserwebApiController : Controller
    {
        private IConfiguration configuration;
        public UserwebApiController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        DataTable dtd;
        [Route("GetUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userList = new List<UsersD>();
            var connectionString = this.configuration.GetConnectionString("Constr");
            using (var conn = new SqlConnection(connectionString))
            {
               
                try
                {
                    await conn.OpenAsync();
                    var cmd = new SqlCommand("select * from UsersD", conn);
                    using (var ad = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);


                        foreach (DataRow row in dt.Rows)
                        {
                            var user = new UsersD
                            {
                                UserId = Convert.ToInt32(row["userId"]),
                                UserName = row["userName"].ToString(),
                                email = row["email"].ToString(),
                                password = row["password"].ToString(),
                                phoneno = row["phoneno"].ToString(),
                            };
                            userList.Add(user);
                        }
                    }
                    return Json(userList);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            return Json(userList);
        }
        //[Route("GetUsers")]
        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var userList = new List<UsersD>();
        //    var connectionString = this.configuration.GetConnectionString("ConStr");

        //    try
        //    {
        //        using (var conn = new SqlConnection(connectionString))
        //        {
        //            await conn.OpenAsync();
        //            var sql = "SELECT * FROM UsersD";

        //            using (var cmd = new SqlCommand(sql, conn))
        //            {
        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var user = new UsersD
        //                        {
        //                            UserId = Convert.ToInt32(reader["userId"]),
        //                            UserName = reader["userName"].ToString(),
        //                            email = reader["email"].ToString(),
        //                            password = reader["password"].ToString(),
        //                            phoneno = reader["phoneno"].ToString()
        //                        };

        //                        userList.Add(user);
        //                    }
        //                }
        //            }
        //        }

        //        return Json(userList);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.Message);
        //    }
        //}

        [Route("Getemailandpassword")]
        [HttpGet]
        public JsonResult FinallyPassage(string email, string password)
        {
            var connectionString = new SqlConnection(this.configuration.GetConnectionString("ConStr"));
            var dt = new DataTable();
            try
            {
                connectionString.Open();
                var sqlData = new SqlDataAdapter($"SELECT * FROM UsersD WHERE Email='{email}' AND Password='{password}'", connectionString);
                sqlData.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    return Json(dt);
                  
                }
                else
                {
                    return Json(new { Message = "Not Found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message });
            }
            finally
            {
                connectionString.Close();
            }
        }
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(UsersD Ud)
        {
            var connString = new SqlConnection(this.configuration.GetConnectionString("ConStr"));
            //var dt = new DataTable();
            try
            {

                //var cmd = new SqlCommand("INSERT INTO UsersD (userName, email, password, phoneno) VALUES (@UserName, @Email, @Password, @PhoneNo)", connString);

                //cmd.Parameters.AddWithValue("@UserName", Ud.UserName);
                //cmd.Parameters.AddWithValue("@Email", Ud.email);
                //cmd.Parameters.AddWithValue("@Password", Ud.password);
                //cmd.Parameters.AddWithValue("@PhoneNo", Ud.phoneno);
                var cmd = new SqlCommand("insert into UsersD(userName,email,password,phoneno) values('" + Ud.UserName + "','" + Ud.email + "','" + Ud.password + "','" + Ud.phoneno + "')", connString);
                connString.Open();
                cmd.ExecuteNonQuery();
                connString.Close();
                return Ok(new { Message = "Record Added" });
                //connString.Open();
                //int rowsAffected = cmd.ExecuteNonQuery();



                //if (dt.Rows.Count > 0)
                //{
                //    return Ok(dt + "Record Added");
                //}
                //else
                //{
                //    return BadRequest("No rows inserted.");
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                connString.Close();
            }
        }



    }

}