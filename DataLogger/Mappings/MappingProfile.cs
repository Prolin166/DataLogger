using Common.Enums;
using Domain.Enums;
using AutoMapper;
using Common.Models;
using Domain.Entities;

namespace DataLogger.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Measurement, MeasurementModel>().ForPath(d => d.SensorId, o => o.MapFrom(s => s.Sensor.Id));
            CreateMap<MeasurementModel, Measurement>().ForMember((src) => src.Sensor, opts => opts.Condition((src, dst, srcmember) => srcmember != null));

            CreateMap<Sensor, SensorModel>();
            CreateMap<SensorModel, Sensor>();

            CreateMap<OperationTimer, OperationTimerModel>();
            CreateMap<OperationTimerModel, OperationTimer>();

            CreateMap<Device, DeviceModel>();
            CreateMap<DeviceModel, Device>();

            CreateMap<ConnectionPortModel, ConnectionPort>();
            CreateMap<ConnectionPort, ConnectionPortModel>();

            CreateMap<SensorType, SensorTypeModel>();
            CreateMap<SensorTypeModel, SensorType>();

            CreateMap<Unit, UnitModel>();
            CreateMap<UnitModel, Unit>();

            CreateMap<EnableType, EnableTypeModel>();
            CreateMap<EnableTypeModel, EnableType>();
        }

    }
}

