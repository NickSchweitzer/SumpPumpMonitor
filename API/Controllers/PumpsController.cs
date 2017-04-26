using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using CodingMonkeyNet.SumpPumpMonitor.API.Models;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Repositories;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace CodingMonkeyNet.SumpPumpMonitor.API.Controllers
{
    [/*Authorize, */Route("api/[controller]")]
    public class PumpsController : Controller
    {
        private readonly ITableRepository<DataPointEntity> DataPointRepository;
        private readonly ITableRepository<SumpPumpMetaEntity> MetaDataRepository;
        private readonly IMapper Mapper;

        public PumpsController(ITableRepository<DataPointEntity> dataPointRepo, ITableRepository<SumpPumpMetaEntity> metaRepo, IMapper mapper)
        {
            DataPointRepository = dataPointRepo;
            MetaDataRepository = metaRepo;
            Mapper = mapper;
        }

        [HttpGet()]
        public async Task<IEnumerable<SumpPump>> Pumps()
        {
            IEnumerable<SumpPumpMetaEntity> entities = await MetaDataRepository.All();
            return Mapper.Map<IEnumerable<SumpPumpMetaEntity>, IEnumerable<SumpPump>>(entities);
        }

        [HttpGet("[action]/{pumpId}")]
        public async Task<IEnumerable<DataPoint>> Data(string pumpId)
        {
            IEnumerable<DataPointEntity> entities = await DataPointRepository.All(pumpId);
            return Mapper.Map<IEnumerable<DataPointEntity>, IEnumerable<DataPoint>>(entities);
        }
    }
}