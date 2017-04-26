using System;
using AutoMapper;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.API.Models;
using CodingMonkeyNet.SumpPumpMonitor.Data.Utilities;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DataPointEntity, DataPoint>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.RowKey.ToDateTime()));

            CreateMap<SumpPumpMetaEntity, SumpPump>()
                .ForMember(dest => dest.PumpId, opt => opt.MapFrom(src => src.PartitionKey));
        }
    }
}
