using keyclock_Authentication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace keyclock_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IConfiguration _Configuration;

        public UserController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        List<cmsclass> cms = new List<cmsclass>();
        List<TblpreferanceClass> pref = new List<TblpreferanceClass>();
        List<TblSubPreferance> aa = new List<TblSubPreferance>();

        [HttpGet]
        [Route("getid")]
        [Authorize]

        public ActionResult Get(int id)
        {
            string sqlDataSource = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = $"select * from cms_getdata_user({id})";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var list = new cmsclass();
                list.id = reader.GetInt32("a_id");
                list.title = reader.GetString("title");
                list.description = reader.GetString("description");
                list.prefname = reader.GetString("preferencename");
                list.image = reader.GetString("image");
                list.subPreferenceId =reader.GetInt32("subpreference_id");
                /*list.subId = reader.GetInt16("subId");*/
                cms.Add(list);
            }
            conn.Close();
            return Ok(cms);
        }

        [HttpDelete]
        [Authorize]

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
                conn.Close();
            }
            else
            {
                return Ok(new { success = "done" });
                conn.Close();
            }
        }

        [HttpGet]
        [Route("preference")]

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
                /*list.image = reader.GetStream("image");*/
                pref.Add(list);
            }
            conn.Close();
            return Ok(pref);
        }


        [HttpGet]
        [Route("subpref")]
        [Authorize]

        public ActionResult get_s()
        {
            string sqlDataSource = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = "select * from TblSubPreferance";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var list = new TblpreferanceClass();
                list.id = reader.GetInt16("subpref_id");
                list.name = reader.GetString("subpref_name");
                list.sub_id = reader.GetInt16("preferenceid");
                pref.Add(list);
            }
            conn.Close();
            return Ok(pref);
        }

        [HttpGet]
        [Route("getinsindedatadetalils")]
        [Authorize]


        public ActionResult Getdetails(int id, int id1)
        {
            string sqlDataSource = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = $"select * from cms_get_detaildata({id},{id1})";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var list = new cmsclass();
                list.id = reader.GetInt32("a_id");
                list.title = reader.GetString("title");
                list.description = reader.GetString("description");
                list.image =reader.GetString("image");

                list.image = reader.GetString("image");
                // list.subId = reader.GetInt16("subId");
                cms.Add(list);
            }
            conn.Close();
            return Ok(cms);
        }

        [HttpGet]
        [Route("getsubs")]

        public ActionResult Get_sub_data(int id)
        {
            string sqlDataSource = _Configuration.GetConnectionString("conn");
            NpgsqlConnection conn = new NpgsqlConnection(sqlDataSource);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            command.CommandType = CommandType.Text;
            command.CommandText = $"select * from cms_get_subdatatypes({id})";
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var list = new TblSubPreferance();
                list.id = reader.GetInt32("a_id");
                list.name = reader.GetString("title");
                list.pref_id = reader.GetInt16("p_id");

                aa.Add(list);
            }
            conn.Close();
            return Ok(aa);
        }
    }
}
