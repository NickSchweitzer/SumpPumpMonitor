using System;
using AutoMapper;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.Portal.Models;
using CodingMonkeyNet.SumpPumpMonitor.Data.Utilities;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DataPointEntity, DataPoint>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.RowKey.FromRowKey()));

            CreateMap<AlertEntity, Alert>();

            CreateMap<SumpPumpSettingEntity, PumpConfiguration>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DeviceName));

            CreateMap<DeviceTwinEntity<SumpPumpSettingEntity>, SumpPump>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.Desired, opt => opt.MapFrom(src => src.Properties.Desired))
                .ForMember(dest => dest.Reported, opt => opt.MapFrom(src => src.Properties.Reported));

            CreateMap<SumpPump, DeviceTwinEntity<SumpPumpSettingEntity>>()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.PumpId))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => new TwinSettingPair<SumpPumpSettingEntity>
                {
                    Desired = new SumpPumpSettingEntity
                    {
                        DeviceName = src.Desired.Name,
                        MaxWaterLevel = src.Desired.MaxWaterLevel,
                        MaxRunTimeNoChange = src.Desired.MaxRunTimeNoChange
                    }
                }));
        }
    }
}