using Nest;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Infrastructure.Services;

public class ElasticsearchService : IElasticsearchService
{
    private readonly IElasticClient _elasticClient;
    public ElasticsearchService(IElasticClient client)
    {
        _elasticClient = client;
    }

    public async Task IndexPermissionAsync(Permission permission)
    {
        await _elasticClient.IndexDocumentAsync(permission);
    }
}