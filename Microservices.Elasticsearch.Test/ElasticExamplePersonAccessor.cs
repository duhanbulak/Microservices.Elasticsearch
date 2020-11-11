using System;
using System.Collections.Generic;
using System.Text;
using Microservices.Elasticsearch.Helper;
using Nest;

namespace Microservices.Elasticsearch.Test
{
    public static class ElasticExamplePersonAccessor
    {
        public static IndexResponse AddModelToElastic(ExamplePersonModel personModel, string indexName = null)
        {
            return ElasticHelper<ExamplePersonModel>.Instance.AddIndex(personModel, indexName?.ToLower() ?? typeof(ExamplePersonModel).Name.ToLower());
        }

        public static BulkResponse AddModelListToElastic(List<ExamplePersonModel> personModels, string indexName = null)
        {
            return ElasticHelper<ExamplePersonModel>.Instance.BulkIndexList(personModels, indexName?.ToLower() ?? typeof(ExamplePersonModel).Name.ToLower());
        }

        public static DeleteResponse DeleteModelToElastic(string indexName, string id)
        {
            return ElasticHelper<ExamplePersonModel>.Instance.DeleteIndex(indexName.ToLower(), id);
        }
    }
}
