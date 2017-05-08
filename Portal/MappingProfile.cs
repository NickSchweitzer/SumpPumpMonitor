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

            CreateMap<DataPointEntity, SumpPump>() // For current water level and pump status
                .ForMember(dest => dest.CurrentWaterLevel, opt => opt.MapFrom(src => src.WaterLevel))
                .ForMember(dest => dest.LastDataRecorded, opt => opt.MapFrom(src => src.RowKey.FromRowKey()));

            CreateMap<DeviceTwinQueryEntity, SumpPump>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Properties.Desired.DeviceName))
                .ForMember(dest => dest.MaxWaterLevel, opt => opt.MapFrom(src => src.Properties.Desired.MaxWaterLevel))
                .ForMember(dest => dest.MaxRunTimeNoChange, opt => opt.MapFrom(src => src.Properties.Desired.MaxRunTimeNoChange));

            CreateMap<SumpPump, DeviceTwinUpdateEntity>()
                .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.PumpId))
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => new SumpPumpSettingUpdate
                {
                    Desired = new SumpPumpSetting
                    {
                        DeviceName = src.Name,
                        MaxWaterLevel = src.MaxWaterLevel,
                        MaxRunTimeNoChange = src.MaxRunTimeNoChange
                    }
                }));
        }
    }
}
