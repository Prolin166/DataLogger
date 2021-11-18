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
using System.Linq;

namespace DataLogger.Test.Business.Services
{
    [TestClass]
    public class ManagementServiceTest
    {
        private DbContextOptions<DataLoggerDbContext> _options;
        private IManagementService _managementService;
        private DataLoggerDbContext _dbContext;
        private IMapper _mapper;
        private Device _device;
        private OperationTimer _operationTimer;

        [TestInitialize]
        public void SensorServiceTestInit()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())).CreateMapper();
            _options = new DbContextOptionsBuilder<DataLoggerDbContext>()
            .UseInMemoryDatabase(databaseName: "DataLoggerTestDb")
            .Options;
            _dbContext = new DataLoggerDbContext(_options);
            _managementService = new ManagementService(_dbContext, _mapper);

            _device = new Device
            {
                Id = 1,
                DeviceId = "ID0815",
                IPAdress = "192.168.1.106",
                HostName = "RaspberryPi",
                MacAdress = "15:28:FD:AF:EB:D9",
                DaysToSave = 11
            };

            _operationTimer = new OperationTimer
            {
                Id = 1,
                SessionId = 222,
                LastTime = new DateTime(2020, 3, 15),
                CumulatedCount = new TimeSpan(6, 14, 32, 17, 685),
            };

            _dbContext.Add(_device);
            _dbContext.Add(_operationTimer);
            _dbContext.SaveChanges();
        }

        [TestMethod]
        public void GetHostName()
        {
            var host = _managementService.GetHostName();

            Assert.AreEqual("RaspberryPi", host);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetDeviceId()
        {
            var id = _managementService.GetDeviceId();

            Assert.AreEqual(_device.DeviceId, id);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetIPAdress()
        {
            var ip = _managementService.GetIPAdress();

            Assert.AreEqual(_device.IPAdress, ip);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetMacAddr()
        {
            var ip = _managementService.GetMacAddr();

            Assert.AreEqual(_device.MacAdress, ip);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void SetDeviceId()
        {
            _managementService.SetDeviceId("ID2277");

            Assert.IsNotNull(_device.DeviceId);
            Assert.AreSame("ID2277", _dbContext.DeviceProperties.FirstOrDefault(id => id.Id == 1).DeviceId);
            Assert.AreNotSame("ID0815", _dbContext.DeviceProperties.FirstOrDefault(id => id.Id == 1).DeviceId);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void InitDeviceProperties()
        {
            //has to be implemented
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void OperationTimerExists()
        {
            var optModel = _managementService.OperationTimerExists();

            Assert.IsTrue(optModel);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetOperationTimerValue()
        {
            var optModel = _managementService.GetOperationTimerValue();

            Assert.IsNotNull(optModel);
            Assert.AreEqual(_operationTimer.LastTime, optModel.LastTime);
            Assert.AreEqual(_operationTimer.SessionId, optModel.SessionId);
            Assert.AreEqual(_operationTimer.CumulatedCount, optModel.CumulatedCount);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void UpdateOperationTimerFromSensors_OperationTimerDontExits()
        {
            var optModel = _dbContext.Operationtimer.FirstOrDefault();
            _dbContext.Operationtimer.Remove(optModel);

            var operationTimerModel = new OperationTimerModel
            {
                Id = 1,
                SessionId = 223,
                LastTime = new DateTime(2020, 3, 16),
                CumulatedCount = new TimeSpan(6, 15, 32, 17, 685),
            };

            _managementService.UpdateOperationTimerFromSensors(operationTimerModel);

            Assert.IsNotNull(_dbContext.Operationtimer.FirstOrDefault());
            Assert.AreEqual(operationTimerModel.LastTime, _dbContext.Operationtimer.FirstOrDefault().LastTime);
            Assert.AreEqual(operationTimerModel.SessionId, _dbContext.Operationtimer.FirstOrDefault().SessionId);
            Assert.AreEqual(operationTimerModel.CumulatedCount, _dbContext.Operationtimer.FirstOrDefault().CumulatedCount);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void UpdateOperationTimerFromSensors_OperationTimerExits()
        {
            var operationTimerModel = new OperationTimerModel
            {
                Id = 1,
                SessionId = 223,
                LastTime = new DateTime(2020, 3, 16),
                CumulatedCount = new TimeSpan(6, 15, 32, 17, 685),
            };

            _managementService.UpdateOperationTimerFromSensors(operationTimerModel);

            Assert.IsNotNull(_dbContext.Operationtimer.FirstOrDefault());
            Assert.AreEqual(operationTimerModel.LastTime, _dbContext.Operationtimer.FirstOrDefault().LastTime);
            Assert.AreEqual(operationTimerModel.SessionId, _dbContext.Operationtimer.FirstOrDefault().SessionId);
            Assert.AreEqual(operationTimerModel.CumulatedCount, _dbContext.Operationtimer.FirstOrDefault().CumulatedCount);

            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void ResetOperationTimer()
        {
            _managementService.ResetOperationTimer();

            Assert.IsNull(_dbContext.Operationtimer.FirstOrDefault());

            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetDaysToSave()
        {
            var daysToSave = _managementService.GetDaysToSave();

            Assert.AreEqual(_device.DaysToSave, daysToSave);
            _dbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void SetDaysToSave()
        {
            _managementService.SetDaysToSave(2277);

            Assert.IsNotNull(_device.DaysToSave);
            Assert.AreEqual(2277, _dbContext.DeviceProperties.FirstOrDefault(id => id.Id == 1).DaysToSave);
            Assert.AreNotEqual(0815, _dbContext.DeviceProperties.FirstOrDefault(id => id.Id == 1).DaysToSave);
            _dbContext.Database.EnsureDeleted();
        }

    }
}



