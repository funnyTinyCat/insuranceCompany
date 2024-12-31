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

        public async Task<int> CreatePolicyAsync(Policy policy)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        string sql = @"
                         INSERT INTO policies(PolicyNumber, PolicyAmount)
                         VALUES (@PolicyNumber, @PolicyAmount);
                         SELECT CAST(SCOPE_IDENTITY() as int);";

                        var id = await connection.QuerySingleAsync<int>(sql, new
                        {
                            policy.PolicyNumber,
                            policy.PolicyAmount
                        }, transaction);

                        policy.Id = id;

                        transaction.Commit();

                        return id;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            var sql = GetPolicySql(false);
            var policies = await QueryPoliciesAsync(sql);
            return policies;
        }

        public async Task<Policy> GetPolicyAsync(int id)
        {
            var sql = GetPolicySql(true);
            var policies = await QueryPoliciesAsync(sql, new { Id = id });
            return policies.FirstOrDefault();
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
