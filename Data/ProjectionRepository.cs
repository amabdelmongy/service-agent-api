using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib.Extensions;
using Domain;
using Domain.ServiceAgent.Events;
using Domain.ServiceAgent.Projection;
using Newtonsoft.Json;

namespace Data
{
    public class ProjectionRepository : IProjectionRepository
    {
        private const string TableName = "[ServiceAgentProjections]";
        private readonly string _connectionString;

        public ProjectionRepository(
            string connectionString
        )
        {
            _connectionString = connectionString;
        }

        public Result<ServiceAgentProjection> Get(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE ServiceAgentId = '{id}';";
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var serviceAgents = connection.QueryFirstOrDefault<ServiceAgentProjection>(sql);
                return Result.Ok(serviceAgents);
            }
            catch (Exception ex)
            {
                return Result.Failed<ServiceAgentProjection>(Error.CreateFrom("ServiceAgentProjection", ex));
            }
        }
        public Result<IEnumerable<ServiceAgentProjection>> Get(bool? isShowFavouriteOnly)
        {
            var sql = $"SELECT * FROM {TableName} ";
            if(isShowFavouriteOnly.HasValue && isShowFavouriteOnly.Value)
                sql += $"WHERE [IsFavourite] = 'true'";

            try
            {
                using var connection = new SqlConnection(_connectionString);
                var serviceAgents = connection.Query<ServiceAgentProjection>(sql);
                return Result.Ok(serviceAgents);
            }
            catch (Exception ex)
            {
                return Result.Failed<IEnumerable<ServiceAgentProjection>>(Error.CreateFrom("ServiceAgentProjection", ex));
            }
        }

        public Result<object> Add(ServiceAgentProjection serviceAgentProjection)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                    connection.Insert(serviceAgentProjection);

                return Result.Ok<object>();
            }
            catch (Exception ex)
            {
                return
                    Result.Failed<object>(
                        Error.CreateFrom("Error when Adding to ServiceAgentProjection", ex)
                    );
            }
        } 
        public Result<object> Update(ServiceAgentCompletedEvent @event)
        {
            var sql =
                $"UPDATE [dbo].[ServiceAgentProjections] " +
                $"SET [Status] = '{@event.Status.Id}', " +
                $"[LastUpdatedDate] = '{@event.TimeStamp}', " +
                $"[ResponseBody] = '{@event.ResponseBody}', " +
                $"[ResponseHeaders] = '{JsonConvert.SerializeObject(@event.ResponseHeaders)}', " +
                $"[ExecutionDate] = '{@event.ExecutionDate}' " +
                $"WHERE ServiceAgentId = '{@event.AggregateId}'";
            return Update(sql);
        }

        public Result<object> Update(ServiceAgentFailedEvent @event)
        {
            var sql =
                "UPDATE [dbo].[ServiceAgentProjections] " +
                $"SET [Status] = '{@event.Status.Id}', " +
                $"[LastUpdatedDate] = '{@event.TimeStamp}', " +
                $"[FailedDetails] = '{@event.Details}', " +
                $"[ExecutionDate] = '{@event.ExecutionDate}' " +
                $"WHERE ServiceAgentId = '{@event.AggregateId}'";

            return Update(sql);
        }
        
        public Result<object> UpdateIsFavourite(Guid id, bool isFavourite)
        {
            var sql =
                "UPDATE [dbo].[ServiceAgentProjections] " +
                $"SET [IsFavourite] = '{isFavourite.ToString().ToLower()}' " +
                $"WHERE ServiceAgentId = '{id}'";

            return Update(sql);
        }
        private Result<object> Update(string sql)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    return Result.Ok<object>();
                }
            }
            catch (Exception ex)
            {
                return
                    Result.Failed<object>(
                        Error.CreateFrom("Projection", ex)
                    );
            }
        }
    }
}
