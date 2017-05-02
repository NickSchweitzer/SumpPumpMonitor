using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;

using CodingMonkeyNet.SumpPumpMonitor.Portal.Models;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Repositories;
using CodingMonkeyNet.SumpPumpMonitor.IoT.Messages;
using CodingMonkeyNet.SumpPumpMonitor.Portal.Services;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Controllers
{
    [Route("api/[controller]")]
    public class PumpsController : Controller
    {
        private readonly ITableRepository<DataPointEntity> DataPointRepository;
        private readonly ITableRepository<SumpPumpMetaEntity> MetaDataRepository;
        private readonly IIoTHubSender<SumpPumpSettings> IoTHub;
        private readonly IMapper Mapper;

        public PumpsController(ITableRepository<DataPointEntity> dataPointRepo, ITableRepository<SumpPumpMetaEntity> metaRepo, IMapper mapper, 
            IIoTHubSender<SumpPumpSettings> iotHub)
        {
            DataPointRepository = dataPointRepo;
            MetaDataRepository = metaRepo;
            Mapper = mapper;
            IoTHub = iotHub;
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

        [HttpPut("{pumpId}")]
        public async Task<IActionResult> UpdatePump(string pumpId, [FromBody] SumpPump pump)
        {
            if (pump == null || pump.PumpId != pumpId)
                return BadRequest();

            IEnumerable<SumpPumpMetaEntity> entities = await MetaDataRepository.Top(pumpId, 1);
            var repoPump = entities.FirstOrDefault();
            if (repoPump == null)
                return NotFound();

            repoPump.Name = pump.Name;
            repoPump.MaxWaterLevel = pump.MaxWaterLevel;
            repoPump.MaxRunTimeNoChange = pump.MaxRunTimeNoChange;

            MetaDataRepository.Upsert(repoPump);
            IoTHub.SendMessage(pumpId, new SumpPumpSettings
            {
                MaxWaterLevel = pump.MaxWaterLevel,
                MaxRunTimeNoChange = pump.MaxRunTimeNoChange
            });
            return new NoContentResult();
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
