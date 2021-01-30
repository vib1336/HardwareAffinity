namespace HardwareAffinity.Web.Extensions
{
    using System;

    using Elasticsearch.Net;
    using HardwareAffinity.Data.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Nest;

    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var connectionPool = new SingleNodeConnectionPool(new Uri(url));

            var settings = new ConnectionSettings(connectionPool)
                .DefaultIndex(defaultIndex)
            .DefaultMappingFor<Product>(m => m);

            settings.ThrowExceptions(alwaysThrow: true);
            settings.PrettyJson();
            settings.DisablePing();

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
