using Dapper;
using partneriOD.Interfaces;
using partneriOD.Models;
using System.Data.SqlClient;
using System.Transactions;

namespace partneriOD.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly string _connectionString;

        public PartnerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            var sql = GetPartnerSql(false, true);
            var partners = await QueryPartnersAsync(sql);
            return partners;
        }

        public async Task<Partner> GetPartnerAsync(int id)
        {
            var sql = GetPartnerSql(true);
            var partners = await QueryPartnersAsync(sql, new { Id = id });
            return partners.FirstOrDefault();
        }

        public async Task<int> CreatePartnerAsync(Partner partner)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        //int partnerTypeId = await GetOrCreatePartnerTypeAsync(connection,
                        //    transaction, partner.PartnerType.Type);

                        string sql = @"
                         INSERT INTO partners (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId,
                            CreatedAtUtc, CreateByUser, IsForeign, ExternalCode, Gender)
                         VALUES (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId,
                            GETUTCDATE(), @CreateByUser, @IsForeign, @ExternalCode, @Gender);
                         SELECT CAST(SCOPE_IDENTITY() as int);";

                        var id = await connection.QuerySingleAsync<int>(sql, new
                        {
                            partner.FirstName,
                            partner.LastName,
                            partner.Address,
                            partner.PartnerNumber,
                            partner.CroatianPIN,
                            partner.PartnerTypeId,
                            partner.CreateByUser,
                            partner.IsForeign,
                            partner.ExternalCode,
                            partner.Gender
                        }, transaction);

                        partner.Id = id;
                     
                        if (partner.Policies != null)
                        {
                            foreach (var policy in partner.Policies)
                            {
                                await CreatePartnerPolicyAsync(connection, new PartnerPolicy
                                {
                                    PartnerId = id,
                                    PolicyId = policy.Id
                                }, transaction);
                            }
                        }

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

        private async Task<int> GetOrCreatePartnerTypeAsync(SqlConnection connection,
        SqlTransaction transaction, string partnerType)
        {
            string checkSql = "SELECT Id FROM partnerTypes WHERE Type = @Type";
            var existingPartnerTypeId = await connection
                .QueryFirstOrDefaultAsync<int?>(checkSql, new
                {
                    Type = partnerType
                },
                transaction);

            if (existingPartnerTypeId.HasValue)
            {
                return existingPartnerTypeId.Value;
            }

            string insertSql = @"INSERT INTO partnerType (Type) VALUES (@Type);
                                SELECT CAST(SCOPE_IDENTITY() as int);";
            var newPartnerTypeId = await connection
                .QuerySingleAsync<int>(insertSql, new { Name = partnerType },
                transaction);

            return newPartnerTypeId;
        }

        private async Task CreatePartnerPolicyAsync(SqlConnection connection,
            PartnerPolicy partnerPolicy, SqlTransaction transaction)
        {
            string sql = @"INSERT INTO partnerPolicy (PartnerId, PolicyId)
                            VALUES (@PartnerId, @PolicyId);";
            await connection.ExecuteAsync(sql, partnerPolicy, transaction);
        }

        private string GetPartnerSql(bool withWhereClause, bool withOrderBy = false)
        {
            var sql = @"
                    SELECT p.*, pt.*, pp.*, po.*
                    FROM partners p
                    LEFT JOIN partnerTypes pt ON p.PartnerTypeId = pt.Id
                    LEFT JOIN partnerPolicy pp ON p.Id = pp.PartnerId 
                    LEFT JOIN policies po ON pp.PolicyId = po.Id ";

            if (withWhereClause)
                sql += " WHERE p.Id = @Id";

            if (withOrderBy)
                sql += " order by p.CreatedAtUtc desc ";

            return sql;
        }

        private async Task<IEnumerable<Partner>> QueryPartnersAsync(
            string sql, object? parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var partnerDictionary = new Dictionary<int, Partner>();

                var partners = await connection.QueryAsync<Partner, PartnerType, PartnerPolicy,
                    Policy, Partner>(
                    sql,
                    (partner, partnerType, partnerPolicy, policy) =>
                    {
                        if (!partnerDictionary.TryGetValue(partner.Id, out var currentPartner))
                        {
                            currentPartner = partner;
                            currentPartner.PartnerType = partnerType;
                            currentPartner.Policies = [];

                            partnerDictionary.Add(currentPartner.Id, currentPartner);
                        }

                        if (policy is not null && !currentPartner.Policies.Any(r => r.Id == policy.Id))
                        {
                            currentPartner.Policies.Add(policy);
                        }

                        return currentPartner;
                    },
                    parameters,
                    splitOn: "Id, Id, Id");

                return partnerDictionary.Values;
            }
        }

        public async Task<bool> IsExternalCodeUnique(string externalCode)
        {
            bool isExternalCodeUnique = true;
            var connection = new SqlConnection(_connectionString);

            string checkSql = "SELECT * FROM partners WHERE externalCode = @ExternalCode";
            var existingExternalCode = await connection
                .QueryAsync<Partner>(checkSql, new
                {
                    ExternalCode = externalCode
                });

            if ((existingExternalCode.Count() > 0))
            {
                isExternalCodeUnique = false;
            }

            return isExternalCodeUnique;
        }

    }
}
