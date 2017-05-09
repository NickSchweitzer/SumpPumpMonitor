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
        private readonly IIoTHubSender<SumpPumpSettings> IoTHub;
        private readonly TwinRepository TwinRepository;
        private readonly IMapper Mapper;

        public PumpsController(ITableRepository<DataPointEntity> dataPointRepo, IMapper mapper, IIoTHubSender<SumpPumpSettings> iotHub, TwinRepository twinRepo)
        {
            DataPointRepository = dataPointRepo;
            TwinRepository = twinRepo;
            Mapper = mapper;
            IoTHub = iotHub;
        }

        public async Task<IEnumerable<SumpPump>> Pumps()
        {
            IEnumerable<DeviceTwinEntity> entities = await TwinRepository.All();
            var pumpList = Mapper.Map<IEnumerable<DeviceTwinEntity>, IEnumerable<SumpPump>>(entities);
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
            DeviceTwinEntity entity = await TwinRepository.ById(pumpId);
            var topDataPointQuery = await DataPointRepository.Top(pumpId, 1);
            DataPointEntity currentData = topDataPointQuery.FirstOrDefault();

            var partialPump = Mapper.Map<DeviceTwinEntity, SumpPump>(entity);
            return Mapper.Map<DataPointEntity, SumpPump>(currentData, partialPump);
        }

        [HttpPut("{pumpId}")]
        public async Task<IActionResult> UpdatePump(string pumpId, [FromBody] SumpPump pump)
        {
            if (pump == null || pump.PumpId != pumpId)
                return BadRequest();

            DeviceTwinEntity twinPump = await TwinRepository.ById(pumpId);
            if (twinPump == null)
                return NotFound();

            var updatePump = Mapper.Map<SumpPump, DeviceTwinEntity>(pump);
            TwinRepository.Update(updatePump);
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
