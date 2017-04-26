using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moon.OData;

using CodingMonkeyNet.SumpPumpMonitor.Portal.Models;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Repositories;
using System.Threading.Tasks;
using AutoMapper;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Controllers
{
    [Route("api/[controller]")]
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

        public async Task<IEnumerable<SumpPump>> Pumps()
        {
            IEnumerable<SumpPumpMetaEntity> entities = await MetaDataRepository.All();
            var pumpList = Mapper.Map<IEnumerable<SumpPumpMetaEntity>, IEnumerable<SumpPump>>(entities);
            foreach (var pump in pumpList)
            {
                var topDataPointQuery = await DataPointRepository.Top(pump.PumpId, 1);
                DataPointEntity currentData = topDataPointQuery.FirstOrDefault();
                Mapper.Map<DataPointEntity, SumpPump>(currentData, pump);
            }
            return pumpList;
        }

        [HttpGet("{pumpId}")]
        public async Task<SumpPump> Pumps(string pumpId)
        {
            IEnumerable<SumpPumpMetaEntity> entities = await MetaDataRepository.Top(pumpId, 1);

            var topDataPointQuery = await DataPointRepository.Top(pumpId, 1);
            DataPointEntity currentData = topDataPointQuery.FirstOrDefault();

            var partialPump = Mapper.Map<SumpPumpMetaEntity, SumpPump>(entities.FirstOrDefault());
            return Mapper.Map<DataPointEntity, SumpPump>(currentData, partialPump);
        }


        [HttpGet("[action]/{pumpId}")]
        public async Task<IEnumerable<DataPoint>> Data(string pumpId, DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null && endDate != null)
                startDate = DateTime.Now;

            IEnumerable<DataPointEntity> entities;

            if (startDate == null && endDate == null)
                entities = await DataPointRepository.All(pumpId);
            else
                entities = await DataPointRepository.Range(pumpId, startDate.Value, endDate);

            return Mapper.Map<IEnumerable<DataPointEntity>, IEnumerable<DataPoint>>(entities);
        }
    }
}
