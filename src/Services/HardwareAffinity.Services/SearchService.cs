namespace HardwareAffinity.Services
{
    using System;
    using System.Threading.Tasks;

    using Nest;

    public class SearchService : ISearchService
    {
        private const string ElasticSearch = "Elasticsearch: ";
        private const string ModelElasticSearch = "Elasticsearch model is null.";

        private readonly IElasticClient client;

        public SearchService(IElasticClient client)
            => this.client = client;

        public async Task<Result> CreateIndexAsync<T>(T model)
            where T : class
        {
            if (model == null)
            {
                throw new ArgumentException(ModelElasticSearch);
            }

            var result = await this.client.IndexDocumentAsync(model);

            if (!result.IsValid)
            {
                throw new ArgumentException(ElasticSearch + result.OriginalException);
            }

            return result.Result;
        }

        public async Task<Result> DeleteIndexAsync<T>(T model)
            where T : class
        {
            if (model == null)
            {
                throw new ArgumentException(ModelElasticSearch);
            }

            var result = await this.client.DeleteAsync<T>(model);

            if (!result.IsValid)
            {
                throw new ArgumentException(ElasticSearch + result.OriginalException);
            }

            return result.Result;
        }

        public async Task<Result> UpdateIndexAsync<T>(T model)
            where T : class
        {
            if (model == null)
            {
                throw new ArgumentException(ModelElasticSearch);
            }

            var result = await this.client.UpdateAsync<T>(model, u => u.Doc(model));

            if (!result.IsValid)
            {
                throw new ArgumentException(ElasticSearch + result.OriginalException);
            }

            return result.Result;
        }
    }
}
