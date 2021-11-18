using AutoMapper;
using Business.Services.Contracts;
using Common.Models;
using Common.OSHelper;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class MeasurementService : IMeasurementService
    {
        DataLoggerDbContext _dBContext;

        IMapper _mapper;

        public MeasurementService(DataLoggerDbContext dBContext, IMapper mapper)
        {
            _dBContext = dBContext;
            _mapper = mapper;
        }

        public bool AddMeasurementFromSensors(ICollection<MeasurementModel> measurement, bool detachFromAddedElementsAutomatically = true)
        {
            try
            {
                var map = _mapper.Map<ICollection<Measurement>>(measurement);
                _dBContext.Measurements.AddRange(map);
                _dBContext.SaveChanges();

                if (detachFromAddedElementsAutomatically)
                {
                    var changedEntries = _dBContext.ChangeTracker.Entries<Measurement>().ToList();
                    changedEntries.ForEach(ce => ce.State = EntityState.Detached);
                }

                return true;
            }
            catch
            {
                //logging...
                return false;
            }
            
        }

        public bool DeleteMeasurementById(int id)
        {

            try
            {

                var existing = _dBContext.Measurements.FirstOrDefault(i => i.Id == id);
                _dBContext.Measurements.Remove(existing);
                _dBContext.SaveChanges();

                return true;
            }
            catch
            { 
                //logging...
                return false;
            }

        }

        public List<MeasurementModel> GetAllMeasurements()
        {

            try
            {
                var list = _mapper.Map<List<MeasurementModel>>(_dBContext.Measurements.ToList());

                return list;
            }
            catch
            {
                //logging...
                return new List<MeasurementModel>();
            }

        }

        public MeasurementModel GetLastMeasurement()
        {

            try
            {
                var measurementModel = _mapper.Map<MeasurementModel>(_dBContext.Measurements.LastOrDefault());

                return measurementModel;
            }
            catch
            {
                //logging...
                return new MeasurementModel();
            }

        }

        public MeasurementModel GetMeasurementById(int id)
        {

            try
            {
                var measurementModel = _mapper.Map<MeasurementModel>(_dBContext.Measurements.FirstOrDefault(m => m.Id == id));

                return measurementModel;
            }
            catch
            {
                //logging...
                return new MeasurementModel();
            }

        }

        public void WriteMeasurementsToFile(string name)
        {
            var measurements = GetAllMeasurements();
            TableWriter.WriteToFile(measurements, name);
        }

        public byte[] SendMeasurmentsToClient()
        {
            return TableWriter.WriteToMemory(GetAllMeasurements());
        }

        public bool DeleteMeasurementsOlderThanDays(int days)
        {
            try
            {
                foreach (var measurement in GetAllMeasurements())
                {
                    if (measurement.Timestamp <= DateTime.Now.AddDays(-days))
                    {
                        DeleteMeasurementById(measurement.Id);
                    }
                }
                return true;
            }
            catch
            {
                //logging...
                return false;
            }
            
        }
    }
}
