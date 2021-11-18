using AutoMapper;
using Business.Services.Contracts;
using Common.Enums;
using Common.Models;
using Domain;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Services
{
    public class SensorService : ISensorService
    {
        DataLoggerDbContext _dBContext;

        IMapper _mapper;

        public SensorService(DataLoggerDbContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }
        public bool AddSensorFromSensors(SensorModel sensormodel)
        {
            try
            {
                var sensor = _mapper.Map<Sensor>(sensormodel);
                _dBContext.Sensors.Add(sensor);
                _dBContext.SaveChanges();

                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public bool DeleteSensor(int id)
        {
            try
            {
                SensorModel sensorModel = new SensorModel(id) { Id = id };
                var sensor = _mapper.Map<Sensor>(sensorModel);
                _dBContext.Sensors.Remove(sensor);
                _dBContext.SaveChanges();

                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public bool EditSensorProperties(SensorModel sensorModel)
        {
            try
            {
                var sensor = _mapper.Map<Sensor>(sensorModel);
                var entity = _dBContext.Sensors.FirstOrDefault(e => e.Id == sensorModel.Id);
                entity.Name = sensorModel.Name;
                entity.Port = (Domain.Enums.ConnectionPort)sensorModel.Port;
                entity.Type = (Domain.Enums.SensorType)sensorModel.Type;
                entity.Unit = (Domain.Enums.Unit)sensorModel.Unit;
                entity.Enabled = (Domain.Enums.EnableType)sensorModel.Enabled;

                _dBContext.Sensors.Update(entity);
                _dBContext.SaveChanges();

                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return false;
            }
        }

        public List<SensorModel> GetAllSensors()
        {
            try
            {

                var sensorList = new List<Sensor>();
                sensorList = _dBContext.Sensors.ToList();
                var list = _mapper.Map<List<SensorModel>>(sensorList);

                return list;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return new List<SensorModel>();
            }
        }

        public SensorModel GetSensorById(int id)
        {
            try
            {

                var sensor = _dBContext.Sensors.FirstOrDefault(s => s.Id == id);
                var model = _mapper.Map<SensorModel>(sensor);

                return model;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return new SensorModel();
            }
        }

        public List<SensorModel> GetAllSensors(bool isActive)
        {
            var sensors = _mapper.Map<List<SensorModel>>(_dBContext.Sensors.ToList());
            List<SensorModel> choosedSensors = new List<SensorModel>();
            try
            {
                if (isActive == true)
                {
                    foreach (var sensor in sensors)
                    {
                        if (sensor.Enabled == EnableTypeModel.True)
                            choosedSensors.Add(sensor);
                    }
                }
                else if (isActive == false)
                {
                    foreach (var sensor in sensors)
                    {
                        if (sensor.Enabled == EnableTypeModel.False)
                            choosedSensors.Add(sensor);
                    }
                }

                
                return choosedSensors;

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                return new List<SensorModel>();
            }
        }
    }
}
