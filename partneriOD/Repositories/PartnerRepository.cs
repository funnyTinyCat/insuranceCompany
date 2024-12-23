using Dapper;
using partneriOD.Interfaces;
using partneriOD.Models;
using System.Data.SqlClient;

namespace partneriOD.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly string _connectionString;

        public PartnerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public Task<int> CreatePartnerAsync(Partner partner)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            var sql = GetPartnerSql(false);
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
                        //int publisherId = await GetOrCreatePublisherAsync(connection,
                        //    transaction, videoGame.Publisher.Name);

                        //int developerId = await GetOrCreateDeveloperAsync(connection,
                        //    transaction, videoGame.Developer.Name);

                        int partnerTypeId = await GetOrCreatePartnerTypeAsync(connection,
                            transaction, partner.PartnerType.Type);

                        string sql = @"
                         INSERT INTO partners (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId,
                            CreatedAtUtc, CreateByUser, IsForeign, ExternalCode, Gender)
                         VALUES (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId,
                            SELECT GETUTCDATE(), @CreateByUser, @IsForeign, @ExternalCode, @Gender);
                         SELECT CAST(SCOPE_IDENTITY() as int);";

                        var id = await connection.QuerySingleAsync<int>(sql, new
                        {
                            partner.FirstName,
                            partner.LastName,
                            partner.Address,
                            partner.PartnerNumber,
                            partner.CroatianPIN,
                            PartnerTypeId = partnerTypeId,
                            partner.CreateByUser,
                            partner.IsForeign,
                            partner.ExternalCode,
                            partner.Gender
                        }, transaction);

                        partner.Id = id;

                        //if (videoGame.GameDetail != null)
                        //{
                        //    videoGame.GameDetail.VideoGameId = id;
                        //    await CreateGameDetailAsync(connection, videoGame.GameDetail, transaction);
                        //}

                        //if (videoGame.Reviews != null)
                        //{
                        //    foreach (var review in videoGame.Reviews)
                        //    {
                        //        review.VideoGameId = id;
                        //        await CreateReviewAsync(connection, review, transaction);
                        //    }
                        //}

                        if (partner.Policies != null)
                        {
                            foreach (var policy in partner.Policies)
                            {
                                await CreatePartnerPolicyAsync(connection, new PartnerPolicy
                                {
                                    VideoGameId = id,
                                    PlatformId = platform.Id
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
            string checkSql = "SELECT Id FROM partnerType WHERE Type = @Type";
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

        private string GetPartnerSql(bool withWhereClause)
        {
            var sql = @"
                    SELECT p.*, pt.*, pp.*, po.*
                    FROM partners p
                    LEFT JOIN partnerTypes pt ON p.PartnerTypeId = pt.Id
                    LEFT JOIN partnerPolicy pp ON p.Id = pp.PartnerId 
                    LEFT JOIN policies po ON pp.PolicyId = po.Id ";

            if (withWhereClause)
                sql += " WHERE p.Id = @Id";

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

    }
}
