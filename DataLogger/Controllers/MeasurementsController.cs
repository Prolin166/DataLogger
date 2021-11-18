using Business.Services.Contracts;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataLogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementsController : ControllerBase
    {
        private IMeasurementService _measurementService;

        public MeasurementsController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }
        
        [HttpGet("[action]")]
        public IActionResult GetMeasurements()
        {
           var data = JsonConvert.SerializeObject(_measurementService.GetAllMeasurements(), Formatting.Indented,
           new JsonSerializerSettings
           {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
           });
            //All measurements from database
            return Content(data) ;
        }

        [HttpGet("[action]/{sensorId}")]
        public IActionResult GetMeasurementsById(int sensorId)
        {
            //All measurements of one sensorid from database
            var result = _measurementService.GetMeasurementById(sensorId);

            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult WriteMeasurements(string name)
        {

            _measurementService.WriteMeasurementsToFile($"{name}_{DateTime.Now}.csv");
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult DownloadMeasurements(string name)
        {
            var csvBytes = _measurementService.SendMeasurmentsToClient();
            return File(csvBytes, "application/octet-stream", $"{name}_{DateTime.Now}.csv");
        }

        [HttpGet("[action]")]
        public IActionResult DeleteMeasurement(int id)
        {
            _measurementService.DeleteMeasurementById(id);
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult DeleteMeasurementsOlderThanDays(int days)
        {
            _measurementService.DeleteMeasurementsOlderThanDays(days);
            return Ok();
        }
    }
}