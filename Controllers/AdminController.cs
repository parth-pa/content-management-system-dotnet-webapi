

using cmsApi;
using keyclock_Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Reflection.PortableExecutable;

namespace cmsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly IConfiguration _Configuration;

        private readonly IWebHostEnvironment _environment;

      
        public AdminController(IConfiguration configuration , IWebHostEnvironment environment)
        {
            _Configuration = configuration;
            this._environment = environment;
        }

        static string imgUrl = string.Empty;

        List<cmsclass> cms = new List<cmsclass>();


        // [HttpGet]
        // [Route("getid")]
        // public ActionResult Get(int id)
        // {
        //     string sqlDataSource = _Configuration.GetConnectionString("conn");
        //     NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
        //     conn.Open();
        //     NpgsqlCommand command = new NpgsqlCommand();
        //     command.Connection = conn;
        //     command.CommandType = CommandType.Text;
        //     command.CommandText = $"select * from cms_getdata({id})";
        //     NpgsqlDataReader reader = command.ExecuteReader();
        //     while (reader.Read())
        //     {

        //         var list = new cmsclass();
        //         list.id = reader.GetInt32("id");
        //         list.title = reader.GetString("title");
        //         list.description = reader.GetString("description");
        //         list.prefId = reader.GetInt32("prefid");
        //         list.subPreferenceId = reader.GetInt32("subpreference");
        //         list.image = reader.GetString("image");
        //         cms.Add(list);
        //     }


        //     return Ok(cms);
        // }
        
        [HttpDelete]
       [Authorize(Roles = Roles.ADMIN)]
        public ActionResult Delete(int id, int id1)
        {
            string data = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(data);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = $"select * from cms_deletedata({id},{id1});";
            int a = command.ExecuteNonQuery();
            if (a == 0)
            {
                return BadRequest(new { fail = " failed" });
            }
            else
            {
                return Ok(new { success = "Deleted" });
            }
        }


        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]

        public ActionResult post(cmsclass data)
        {
            string sqlConnection = _Configuration.GetConnectionString("conn");
            NpgsqlConnection con = new NpgsqlConnection(sqlConnection);
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"Select insert_data('{data.title}','{data.description}','{data.image}',{data.prefId},{data.subPreferenceId})", con);
            cmd.ExecuteNonQuery();
            //con.Close();

            return Ok(new { success = "Inserted" });

        }

        [HttpPut]
       [Authorize(Roles = Roles.ADMIN)]

        public ActionResult put(cmsclass data)
        {
            string sqlConnection = _Configuration.GetConnectionString("conn");
            NpgsqlConnection con = new NpgsqlConnection(sqlConnection);
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand($"select update_data({data.id},'{data.title}','{data.description}','{data.image}',{data.prefId},{data.subPreferenceId})", con);
            cmd.ExecuteNonQuery();
            con.Close();


            return Ok(new { success = "Updated" });
        }



     

        //***************Image APi (Link)***********************

      /*  [HttpPost("UploadImages")]
        public async Task<ActionResult> UploadImage()
        {
            bool Results = false;
            try
            {
                var _uploadedfiles = Request.Form.Files;
                foreach (IFormFile source in _uploadedfiles)
                {
                    string Filename = source.FileName;
                    string Filepath = GetFilePath(Filename);
                    GetImage(Filename);
                    string imagepath = Filepath + "\\image.png";

                    using (FileStream stream = System.IO.File.Create(Filepath))
                    {
                        await source.CopyToAsync(stream);
                        Results = true;
                    }
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return Ok(Results);
        }




        [HttpGet("image")]
        public string GetImage(string productcode)
        {
           
            //string productcode = "Screenshot (2).png";
            string ImageUrl = string.Empty;
            string HostUrl = "https://localhost:7106/";
            string FilePath = GetFilePath(productcode);
            string ImagePath = FilePath + "\\image.png";


            ImageUrl = HostUrl + "/uploads/Product/" + productcode;
            //ImageUrl = HostUrl + "/uploads/Product/Screenshot (2).png";
             imgUrl = ImageUrl;
            return ImageUrl;
        }


        [NonAction]
        private string GetFilePath(string ProductCode)
        {
            return this._environment.WebRootPath + "\\Uploads\\Product\\" + ProductCode;

        }
      

         [HttpGet]
          [Route("abc")]
          public ActionResult get()
          {
              string sqlDataSource = _Configuration.GetConnectionString("conn");
              NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
              conn.Open();
              NpgsqlCommand command = new NpgsqlCommand();
              command.Connection = conn;
              command.CommandType = CommandType.Text;
              command.CommandText = "select * from tblpreferencemaster";
              NpgsqlDataReader reader = command.ExecuteReader();
              while (reader.Read())
              {
                  var list = new TblpreferanceClass();
                  list.id = reader.GetInt16("pref_id");
                  list.name = reader.GetString("preference");
                  / list.image = reader.GetStream("image");/
                  pref.Add(list);
              }
              return Ok(pref);
          }*/

        [HttpGet ("deleted_data")]
        [Authorize(Roles = Roles.ADMIN)]
        public ActionResult Get_deleted_data(int id)
        {
            string sqlDataSource = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = $"select * from cms_get_deletedata({id})";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var list = new cmsclass();
                list.id = reader.GetInt32("a_id");
                list.title = reader.GetString("title");
                list.image =reader.GetString("image");
                list.description=reader.GetString("description");
                list.prefId= reader.GetInt32("preference");
                list.subPreferenceId = reader.GetInt32("subpreference");
                // list.pref_id = reader.GetInt16("p_id");

                cms.Add(list);
            }
            conn.Close();
            return Ok(cms);
        }


        [HttpPut]
        [Route("restoredata")]
       [Authorize(Roles = Roles.ADMIN)]

        public ActionResult restore(int id, int id1)
        {
            string data = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(data);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = $"select * from cms_deletedata_true({id},{id1});";
            int a = command.ExecuteNonQuery();
            if (a == 0)
            {
                return BadRequest(new { fail = " failed" });
                conn.Close();
            }
            else
            {
                return Ok(new { success = "done" });
                conn.Close();
            }
        }
    }
}







