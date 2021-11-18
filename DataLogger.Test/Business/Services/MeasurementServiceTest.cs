using AutoMapper;
using Business.Services;
using Business.Services.Contracts;
using Common.Models;
using DataLogger.Mappings;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataLogger.Test.Business.Services
{
    [TestClass]
    public class MeasurementServiceTest
    {
        private DbContextOptions<DataLoggerDbContext> _options;
        private IMeasurementService _measurementService;
        private DataLoggerDbContext _dbContext;
        private IMapper _mapper;

        [TestInitialize]
        public void MeasurementServiceTestInit()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _options = new DbContextOptionsBuilder<DataLoggerDbContext>()
            .UseInMemoryDatabase(databaseName: "DataLoggerTestDb")
            .Options;
            _dbContext = new DataLoggerDbContext(_options);
            _measurementService = new MeasurementService(_dbContext, _mapper);

            _dbContext.Measurements.Add(new Measurement
            {
                Id = 1,
                SensorId = 1,
                Timestamp = DateTime.Now,
                Value = 11.1111,
            });

            _dbContext.Measurements.Add(new Measurement
            {
                Id = 2,
                SensorId = 2,
                Timestamp = DateTime.Now,
                Value = 22.2222,
            });

            _dbContext.Measurements.Add(new Measurement
            {
                Id = 3,
                SensorId = 3,
                Timestamp = DateTime.Now,
                Value = 33.3333,
            });
            _dbContext.SaveChanges();

            Assert.AreEqual(3, _measurementService.GetAllMeasurements().Count);
        }

        [TestMethod]
        public void GetAllMeasurements()
        {
            var list = _measurementService.GetAllMeasurements();

            Assert.AreEqual(11.1111, list.FirstOrDefault().Value);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetLastMeasurement()
        {
            var measurement = _measurementService.GetLastMeasurement();

            Assert.AreEqual(33.3333, measurement.Value);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetMeasurementById()
        {
            var measurement = _measurementService.GetMeasurementById(2);

            Assert.AreEqual(22.2222, measurement.Value);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void WriteMeasurementsToFile()
        {
            _measurementService.WriteMeasurementsToFile("../../../../Testfiles/testfile.csv");

            Assert.IsTrue(File.Exists("../../../../Testfiles/testfile.csv"));
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void AddMeasurementFromSensors()
        {
            var data = new ControllerNotificationModel();
            List<MeasurementModel> list = new List<MeasurementModel>();
            list.Add(new MeasurementModel
            {
                Id = 4,
                SensorId = 4,
                Timestamp = DateTime.Now,
                Value = 44.4444,
            });
            list.Add(new MeasurementModel
            {
                Id = 5,
                SensorId = 5,
                Timestamp = DateTime.Now,
                Value = 55.5555,
            });
            list.Add(new MeasurementModel
            {
                Id = 6,
                SensorId = 6,
                Timestamp = DateTime.Now,
                Value = 66.6666,
            });

            data.SessionId = 222;
            data.Measurements = list;

            _measurementService.AddMeasurementFromSensors(data.Measurements);

            Assert.AreEqual(55.5555, _measurementService.GetMeasurementById(5).Value);
            Assert.AreEqual(6, _measurementService.GetAllMeasurements().Count);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void DeleteMeasurementById()
        {
            _measurementService.DeleteMeasurementById(2);

            Assert.AreEqual(2, _measurementService.GetAllMeasurements().Count);
            _dbContext.Database.EnsureDeleted();
        }
    }
}



