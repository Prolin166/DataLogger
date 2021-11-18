using AutoMapper;
using Business.Services.Contracts;
using Common;
using Common.Models;
using Common.OSHelper;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;

namespace Business.Services
{
    public class ManagementService : IManagementService
    {

        DataLoggerDbContext _dBContext;

        IMapper _mapper;

        public ManagementService(DataLoggerDbContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public string GetHostName()
        {
            try
            {

                var data = _dBContext.DeviceProperties.FirstOrDefault();
                var model = _mapper.Map<DeviceModel>(data);


                return model.HostName;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return null;
            }
        }

        public string GetIPAdress()
        {
            try
            {

                var data = _dBContext.DeviceProperties.FirstOrDefault();
                var model = _mapper.Map<DeviceModel>(data);


                return model.IPAdress;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return null;
            }
        }

        public string GetDeviceId()
        {
            try
            {

                var data = _dBContext.DeviceProperties.FirstOrDefault();
                var model = _mapper.Map<DeviceModel>(data);


                return model.DeviceId;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return String.Empty;
            }
        }

        public string GetMacAddr()
        {
            try
            {

                var data = _dBContext.DeviceProperties.FirstOrDefault();
                var model = _mapper.Map<DeviceModel>(data);


                return model.MacAdress;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return String.Empty;
            }
        }

        public DeviceModel GetDeviceProperties()
        {
            try
            {

                var data = _dBContext.DeviceProperties.FirstOrDefault();
                var model = _mapper.Map<DeviceModel>(data);


                return model;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return new DeviceModel();
            }
        }

        public bool SetDeviceId(string id)
        {
            try
            {

                var data = _dBContext.DeviceProperties.FirstOrDefault();
                data.DeviceId = id;
                _dBContext.DeviceProperties.Update(data);
                _dBContext.SaveChanges();

                return true;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public bool InitDeviceProperties()
        {
            var data = _dBContext.DeviceProperties.FirstOrDefault();
            try
            {
                if (data == null)
                {
                    data = new Device();
                    data.MacAdress = NetworkHelper.MacAdress;
                    data.DeviceId = NetworkHelper.MacAdress;
                    data.IPAdress = NetworkHelper.IPAdress;
                    data.HostName = NetworkHelper.HostName;
                    data.DaysToSave = 200;
                    _dBContext.DeviceProperties.Add(data);
                }
                else
                {
                    data.MacAdress = NetworkHelper.MacAdress;
                    data.IPAdress = NetworkHelper.IPAdress;
                    data.HostName = NetworkHelper.HostName;
                    _dBContext.DeviceProperties.Update(data);
                }

                _dBContext.SaveChanges();

                return true;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public bool OperationTimerExists()
        {
            try
            {
                var optModel = _dBContext.Operationtimer.Any();

                return optModel;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public OperationTimerModel GetOperationTimerValue()
        {
            try
            {
                var optModel = _mapper.Map<OperationTimerModel>(_dBContext.Operationtimer.FirstOrDefault());
                return optModel;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return new OperationTimerModel();
            }
        }

        public bool UpdateOperationTimerFromSensors(OperationTimerModel optModel)
        {
            try
            {
                var opt = _mapper.Map<OperationTimer>(optModel);
                if(OperationTimerExists())
                {
                    var value = _dBContext.Operationtimer.FirstOrDefault();
                    value.SessionId = opt.SessionId;
                    value.LastTime = opt.LastTime;
                    value.CumulatedCount = opt.CumulatedCount;

                    _dBContext.Operationtimer.Update(value);
                }
                else
                {
                    _dBContext.Operationtimer.Add(opt);
                }
                _dBContext.SaveChanges();

                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }

        }

        public bool ResetOperationTimer()
        {
            try
            {
                if (OperationTimerExists())
                {
                    var opt = _dBContext.Operationtimer.FirstOrDefault();
                    _dBContext.Operationtimer.Remove(opt);
                }
                _dBContext.SaveChanges();

                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public int? GetDaysToSave()
        {
            try
            {
                var data = _dBContext.DeviceProperties.FirstOrDefault();
                var model = _mapper.Map<DeviceModel>(data);

                return model.DaysToSave;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return null;
            }
        }

        public bool SetDaysToSave(int days)
        {
            try
            {
                var data = _dBContext.DeviceProperties.FirstOrDefault();
                data.DaysToSave = days;
                _dBContext.DeviceProperties.Update(data);
                _dBContext.SaveChanges();

                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return true;
            }
        }
    }
}
