using Dapper;
using partneriOD.Interfaces;
using partneriOD.Models;
using System.Data.SqlClient;
using System.Text;

namespace partneriOD.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly string _connectionString;

        public PolicyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            var sql = GetPolicySql(false);
            var policies = await QueryPoliciesAsync(sql);
            return policies;
        }

        private string GetPolicySql(bool withWhereClause)
        {
            var sql = @"
                    SELECT * FROM policies";

            if (withWhereClause)
                sql += " WHERE Id = @Id";

            return sql;
        }

        private async Task<IEnumerable<Policy>> QueryPoliciesAsync(
            string sql, object? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var policyDictionary = new Dictionary<int, Policy>();

                var policies = await connection.QueryAsync<Policy>(
                    sql,
                    parameters);

                return policies;
            }
        }


    }
}
