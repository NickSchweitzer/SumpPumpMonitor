using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

using CodingMonkeyNet.SumpPumpMonitor.Data.Utilities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public abstract class TableRepository<T> : ITableRepository<T>
        where T : TableEntity, new()
    {
        protected string TableName { get; private set; }
        protected CloudTable StorageTable { get; private set; }
        public TableRepository(string connectionString, string tableName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = account.CreateCloudTableClient();
            StorageTable = tableClient.GetTableReference(tableName);
        }

        protected async Task<IEnumerable<T>> ExecuteQuery(TableQuery<T> query)
        {
            TableQuerySegment<T> segment = null;
            var returnList = new List<T>();
            while (segment == null || segment.ContinuationToken != null)
            {
                segment = await StorageTable.ExecuteQuerySegmentedAsync(query, segment?.ContinuationToken);
                returnList.AddRange(segment);
            }
            return returnList;
        }

        public async Task<IEnumerable<T>> All(string partitionKey = null)
        {
            TableQuery<T> query = new TableQuery<T>();
            if (!string.IsNullOrEmpty(partitionKey))
            {
                query = query.Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            }
            return await ExecuteQuery(query);
        }

        public async Task<IEnumerable<T>> Range(string partitionKey, DateTime startTime, DateTime? endTime)
        {
            TableQuery<T> query;

            // Note - The GreaterThan/LessThan Operators are reverse of normal, because the ToRowKey is reverse ordered
            if (endTime != null)
            {
                query = new TableQuery<T>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, endTime.Value.ToRowKey())),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, startTime.ToRowKey())));
            }
            else
            {
                query = new TableQuery<T>().Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, startTime.ToRowKey())));
            }
            return await ExecuteQuery(query);
        }

        public async Task<IEnumerable<T>> Top(string partitionKey, int top)
        {
            var query = new TableQuery<T>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey)).Take(top);

            return await ExecuteQuery(query);
        }

        public async Task Insert(T item)
        {
            var insertItem = TableOperation.Insert(item);
            await StorageTable.ExecuteAsync(insertItem);
        }

        public async Task Upsert(T item)
        {
            var upsertItem = TableOperation.InsertOrReplace(item);
            await StorageTable.ExecuteAsync(upsertItem);
        }
    }
}
