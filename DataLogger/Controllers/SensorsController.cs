using Business.Services.Contracts;
using Common.Models;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DataLogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private ISensorService _sensorService;


        public SensorsController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllSensors()
        {
            return Ok(_sensorService.GetAllSensors());
        }

        [HttpGet("[action]")]
        public IActionResult GetAllActiveSensors(bool isActive)
        {
            return Ok(_sensorService.GetAllSensors(isActive));
        }

        [HttpGet("[action]")] 
        public IActionResult GetSensor(int id)
        {
            return Ok(_sensorService.GetSensorById(id));
        }

        [HttpPost("[action]")]
        public IActionResult EditSensor([FromBody] SensorModel sensor)
        {
            return Ok(_sensorService.EditSensorProperties(sensor));
        }

        [HttpDelete("[action]")]
        public IActionResult DeleteSensor(int id)
        {
            return Ok(_sensorService.DeleteSensor(id));
        }


    }
}