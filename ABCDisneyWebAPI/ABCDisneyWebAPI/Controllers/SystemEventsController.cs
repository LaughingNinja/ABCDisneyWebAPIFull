using System;
using ABCDisneyWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using System.Net.Http;
using System.Configuration;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ABCDisneyWebAPI.Controllers
{

    interface ISystemEventRepository
    {
        IEnumerable<SystemEvent> GetSystemEvents();
        IHttpActionResult GetSystemEvent(int id);
        IHttpActionResult PostSystemEvent(SystemEvent systemEvent);
        IHttpActionResult DeleteSystemEvent(int id);
        IHttpActionResult PutSystemEvent(int id, SystemEvent systemEvent);
    }

    public class SystemEventsController : ApiController, ISystemEventRepository
    {

        const string CONSTRINGNAME = "ConnectionString";
        static Configuration rootWebConfig = WebConfigurationManager.OpenWebConfiguration("/ABCDisneyWebAPI");
        static ConnectionStringSettings conString = null;
        static SqlConnection con = null;


        public SystemEventsController()
        {
            conString = rootWebConfig.ConnectionStrings.ConnectionStrings[CONSTRINGNAME];
            con = new SqlConnection(conString.ConnectionString);
        }

        //GET: api/SystemEvents
        public IEnumerable<SystemEvent> GetSystemEvents()
        {
            List<SystemEvent> systemEvents = new List<SystemEvent>();

            using (SqlConnection con = new SqlConnection(conString.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSystemEvents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", 0);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemEvent systemEventAdd = new SystemEvent();
                            //systemEventAdd.Id = Convert.ToInt16(reader.GetString(0));
                            systemEventAdd.Description = reader.GetString(1);
                            systemEvents.Add(systemEventAdd);
                        }
                    }
                }
            }
            return systemEvents;
        }

        //GET api/SystemEvents/1
        [ResponseType(typeof(SystemEvent))]
        public IHttpActionResult GetSystemEvent(int id)
        {
            SystemEvent systemEvent = null;
            //IEnumerable<SystemEvent> systemEvents = GetSQLSystemEvents(id);
            List<SystemEvent> systemEvents = new List<SystemEvent>();

            using (SqlConnection con = new SqlConnection(conString.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetSystemEvents", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", 0);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemEvent systemEventAdd = new SystemEvent();
                            //systemEventAdd.Id = Convert.ToInt16(reader.GetString(0));
                            systemEventAdd.Description = reader.GetString(1);
                            systemEvents.Add(systemEventAdd);
                        }
                    }
                }
            }
            systemEvent = systemEvents.FirstOrDefault();

            if (systemEvent == null)
            {
                return NotFound();
            }
            return Ok(systemEvent);
        }

        //POST - api/SystemEvents
        [ResponseType(typeof(SystemEvent))]
        public IHttpActionResult PostSystemEvent(SystemEvent systemEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection con = new SqlConnection(conString.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddSystemEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Description", systemEvent.Description);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = systemEvent.Id }, systemEvent);
        }

        //DELETE - api/SystemEvents
        [ResponseType(typeof(SystemEvent))]
        public IHttpActionResult DeleteSystemEvent(int id)
        {
            SystemEvent systemEvent = null;
            using (SqlConnection con = new SqlConnection(conString.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteSystemEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok(systemEvent);
        }

        //PUT: api/SystemEvent/1
        [ResponseType(typeof(SystemEvent))]
        public IHttpActionResult PutSystemEvent(int id, SystemEvent systemEvent)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != systemEvent.Id)
            {
                return BadRequest();
            }
            using (SqlConnection con = new SqlConnection(conString.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateSystemEvent", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", systemEvent.Id);
                    cmd.Parameters.AddWithValue("@Description", systemEvent.Description);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

    //    public IEnumerable<SystemEvent> GetSystemEvents(int id)
    //    {
    //        return systemEvents;
    //    }
    }
}