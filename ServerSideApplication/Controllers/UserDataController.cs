using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Incoming = ServerSideApplication.Models.Incoming;
using Outgoing = ServerSideApplication.Models.Outgoing;

namespace ServerSideApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly CsvHandler _csvHandler;
        private readonly LocationHandler _locationHandler;

        public UserDataController()
        {
            _csvHandler = new CsvHandler();
            _locationHandler = new LocationHandler();
        }

        [HttpGet]
        public IActionResult GetUserData()
        {
            var records = _csvHandler.GetRecords();

            var result = MapOutgoingRecords(records);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserData(Guid id)
        {
            // Get from CSV and load into list of domain objects
            // Query list of objects for item with ID 
            // Return item with matching ID
            var record = _csvHandler.GetRecord(id);

            if (record is null)
                return NotFound($"No record found with ID: {id}");

            var result = MapOutgoingRecords(new List<Domain.UserData> { record });

            return Ok(result);
        }

        [HttpPost]
        public IActionResult PostUserData(Incoming.UserData userData)
        {
            var req = HttpContext.Request;

            var id = Guid.NewGuid();

            var userDetails = new Domain.UserData
            {
                CreatedOnUtc = DateTime.UtcNow,
                IP = GetUserIP(req),
                Device = GetUserDevice(req),
                Id = id,
                Name = userData.Name,
                Interests = userData.Interests,
                Username = userData.Username
            };

            string location = GetLocation(userDetails.IP);

            userDetails.Location = location;

            var records = _csvHandler.AppendRecord(userDetails);

            var result = MapOutgoingRecord(records.FirstOrDefault(x => x.Id == id));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserRecord(Guid id)
        {
            var records = _csvHandler.DeleteRecord(id);

            return Ok(records);
        }

        private string GetUserIP(HttpRequest req)
        {
            var ip = req.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(ip)) ip = ip.Split(',')[0];

            if (string.IsNullOrWhiteSpace(ip)) ip = Convert.ToString(req.HttpContext.Connection.RemoteIpAddress);

            if (string.IsNullOrWhiteSpace(ip)) ip = req.Headers["REMOTE_ADDR"].FirstOrDefault();

            return ip;
        }

        private string GetUserDevice(HttpRequest req)
        {
            if (req.Headers.Any(x => x.Key == "User-Agent"))
            {
                return req.Headers.FirstOrDefault(x => x.Key == "User-Agent").Value;
            }

            return null;
        }

        private List<Outgoing.UserData> MapOutgoingRecords(List<Domain.UserData> records)
        {
            var result = new List<Outgoing.UserData>();

            foreach (var record in records)
            {
                result.Add(MapOutgoingRecord(record));
            }

            return result;
        }

        private Outgoing.UserData MapOutgoingRecord(Domain.UserData record)
        {
            return new Outgoing.UserData
            {
                Id = record.Id,
                Interests = record.Interests,
                Name = record.Name,
                Username = record.Username,
                CreatedOnUtc = record.CreatedOnUtc,
                Device = record.Device,
                IP = record.IP,
                Location = record.Location
            };
        }

        private string GetLocation(string ip)
        {
            string location = string.Empty;

            try
            {
                location = _locationHandler.GetLocationByIP(ip);

            }
            catch (Exception)
            {
                Console.WriteLine($"Error - location could not be retrieved");
            }

            if (location == string.Empty)
                location = "Not available";

            return location;
        }
    }
}
