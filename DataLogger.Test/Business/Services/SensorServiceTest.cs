using AutoMapper;
using Business.Services;
using Business.Services.Contracts;
using Common.Enums;
using Common.Models;
using DataLogger.Mappings;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DataLogger.Test.Business.Services
{
    [TestClass]
    public class SensorServiceTest
    {
        private DbContextOptions<DataLoggerDbContext> _options;
        private ISensorService _sensorService;
        private DataLoggerDbContext _dbContext;
        private IMapper _mapper;

        [TestInitialize]
        public void SensorServiceTestInit()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _options = new DbContextOptionsBuilder<DataLoggerDbContext>()
            .UseInMemoryDatabase(databaseName: "DataLoggerTestDb")
            .Options;
            _dbContext = new DataLoggerDbContext(_options);
            _sensorService = new SensorService(_dbContext, _mapper);

            _dbContext.Sensors.Add(new Sensor(1)
            {
                Unit = Unit.A,
                Port = ConnectionPort.SENSE1,
                Name = "Strom1",
                Enabled = EnableType.False,
                Type = SensorType.Current
            });

            _dbContext.Sensors.Add(new Sensor(2)
            {
                Unit = Unit.V,
                Port = ConnectionPort.DIFF1,
                Name = "Spannung1",
                Enabled = EnableType.True,
                Type = SensorType.Voltage
            });

            _dbContext.Sensors.Add(new Sensor(3)
            {
                Unit = Unit.C,
                Port = ConnectionPort.I2C1,
                Name = "Temperatur",
                Enabled = EnableType.True,
                Type = SensorType.Temperature
            });
            _dbContext.SaveChanges();

            Assert.AreEqual(3, _sensorService.GetAllSensors().Count);
        }

        [TestMethod]
        public void GetAllActiveSensors_IsActiveTrue()
        {
            var list = _sensorService.GetAllSensors(true);

            Assert.AreEqual("Spannung1", list.FirstOrDefault().Name);
            Assert.AreEqual(2, list.Count);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllActiveSensors_IsActiveFalse()
        {
            var list = _sensorService.GetAllSensors(false);

            Assert.AreEqual("Strom1", list.FirstOrDefault().Name);
            Assert.AreEqual(1, list.Count);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetAllSensors()
        {
            var list = _sensorService.GetAllSensors();

            Assert.AreEqual("Strom1", list.FirstOrDefault().Name);
            Assert.AreEqual(3, list.Count);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetSensorById()
        {
            var sensor = _sensorService.GetSensorById(2);

            Assert.AreEqual("Spannung1", sensor.Name);
            Assert.AreEqual(UnitModel.V, sensor.Unit);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void AddSensorFromSensors()
        {
            _sensorService.AddSensorFromSensors(new SensorModel(4)
            {
                Unit = UnitModel.Prozent,
                Port = ConnectionPortModel.I2C1,
                Name = "Humidity",
                Enabled = EnableTypeModel.True,
                Type = SensorTypeModel.Humidity
            });

            Assert.AreEqual("Humidity", _dbContext.Sensors.FirstOrDefault(id => id.Id == 4).Name);
            Assert.AreEqual(4, _dbContext.Sensors.ToList().Count);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void EditSensorProperties()
        {
            _sensorService.EditSensorProperties(new SensorModel(2)
            {
                Unit = UnitModel.bar,
                Port = ConnectionPortModel.I2C1,
                Name = "Pressure",
                Enabled = EnableTypeModel.True,
                Type = SensorTypeModel.Pressure
            });

            Assert.AreEqual("Pressure", _dbContext.Sensors.FirstOrDefault(id => id.Id == 2).Name);
            Assert.AreEqual(3, _dbContext.Sensors.ToList().Count);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void DeleteSensor()
        {
            _sensorService.DeleteSensor(2);

            Assert.AreEqual(3, _dbContext.Sensors.ToList().Count);
            _dbContext.Database.EnsureDeleted();
        }

    }
}



