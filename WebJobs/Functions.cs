﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure.WebJobs.ServiceBus;

using CodingMonkeyNet.SumpPumpMonitor.IoT.Messages;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Utilities;
using CodingMonkeyNet.SumpPumpMonitor.Data.Repositories;

namespace CodingMonkey.SumpPumpMonitor.WebJobs
{
    public class Functions
    {
        //private static readonly CloudTable DataPointTable;
        private static readonly DataPointRepository DataPointRepository;
        private static readonly DutyCycleRepository DutyCycleRepository;
        private static readonly SumpPumpMetaRepository MetaDataRepository;

        static Functions()
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("IoTStorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            //DataPointTable = tableClient.GetTableReference("SumpPumpMonitorData");
            string tableStorageConnectionString = storageAccount.ToString(true);
            DataPointRepository = new DataPointRepository(tableStorageConnectionString);

            DutyCycleRepository = new DutyCycleRepository(tableStorageConnectionString);
            MetaDataRepository = new SumpPumpMetaRepository(tableStorageConnectionString);
        }

        public async static Task ProcessSumpPumpDataPoint([EventHubTrigger("iothub-ehub-sumppump-i-31562-57e63b098f")] DataPointPayload payload)
        {
            var newDataPoint = new DataPointEntity()
            {
                PartitionKey = payload.DeviceId,
                RowKey = payload.Timestamp.ToRowKey(),
                WaterLevel = payload.WaterLevel,
                PumpRunning = payload.PumpRunning
            };
            DataPointRepository.Insert(newDataPoint);

            // Get the Settings for the pump or create if it doesn't exist
            var metaData =  await GetMetaData(payload);
            /*
            // Get Current Duty Cycle we're in the middle of
            var currentDutyCycle = await GetCurrentDutyCycle(payload);

            // Sump Pump turned on or off - start a new Duty Cycle
            if (currentDutyCycle.IsEmptying != payload.PumpRunning)
                currentDutyCycle = CreateDutyCycle(payload);

            // Update the end time with the current statistics
            currentDutyCycle.EndLevel = payload.WaterLevel;
            currentDutyCycle.EndTime = payload.Timestamp;
            DutyCycleRepository.Upsert(currentDutyCycle);
            */
        }

        private async static Task<DutyCycleEntity> GetCurrentDutyCycle(DataPointPayload payload)
        {
            var dutyCycleList = await DutyCycleRepository.Top(payload.DeviceId, 1);
            var dutyCycle = dutyCycleList.FirstOrDefault();

            if (dutyCycle == null)
                return CreateDutyCycle(payload);

            return dutyCycle;
        }

        private static DutyCycleEntity CreateDutyCycle(DataPointPayload payload)
        {
            return new DutyCycleEntity
            {
                PartitionKey = payload.DeviceId,
                RowKey = payload.Timestamp.ToRowKey(),
                StartLevel = payload.WaterLevel,
                StartTime = payload.Timestamp,
                IsEmptying = payload.PumpRunning
            };
        }

        private async static Task<SumpPumpMetaEntity> GetMetaData(DataPointPayload payload)
        {
            var metaDataList = await MetaDataRepository.Top(payload.DeviceId, 1);
            var metaData = metaDataList.FirstOrDefault();

            // First time seeing this pump, need to create a record for it with defaults
            if (metaData == null)
            {
                metaData = new SumpPumpMetaEntity
                {
                    PartitionKey = payload.DeviceId,
                    RowKey = "MetaData",        // Why not?
                    InError = false,
                    MaxRunTimeNoChange = 60,    // 1 minute default
                    MaxWaterLevel = 10          // 10 inches high default
                };
                MetaDataRepository.Insert(metaData);
            }
            return metaData;
        }
    }
}
