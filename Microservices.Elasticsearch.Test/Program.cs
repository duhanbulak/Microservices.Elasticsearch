using System;
using System.Collections.Generic;
using Microservices.Elasticsearch.Helper;
using Microservices.Elasticsearch.Utils;
using Nest;

namespace Microservices.Elasticsearch.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ElasticHelperTestMethod();

                ElasticLoggerTestMethod();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        private static void ElasticHelperTestMethod()
        {
            IndexResponse indexResponse = ElasticExamplePersonAccessor.AddModelToElastic(new ExamplePersonModel()
            {
                Id = 1,
                Name = "Duhan Taha",
                SurName = "BULAK",
                CreatedDate = DateTime.Now
            }, "person");

            if (indexResponse.IsValid || indexResponse.Result == Result.Created)
            {
                ISearchResponse<ExamplePersonModel> searchResponses = ElasticHelper<ExamplePersonModel>.Instance.Search(p => p
                    .DateRange(r => r
                        .Field(f => f.CreatedDate)
                        .GreaterThanOrEquals(new DateTime(2017, 01, 01))
                        .LessThanOrEquals(new DateTime(2021, 01, 01))
                    ),"person"
                );

                Console.WriteLine("Responses after AddModelToElastic(indexName:person)");
                foreach (var searchResponse in searchResponses.Documents)
                {
                    Console.WriteLine(
                        $"id: {searchResponse.Id}\nname: {searchResponse.Name}\nsurname: {searchResponse.SurName}");
                }
            }

            BulkResponse bulkResponse = ElasticExamplePersonAccessor.AddModelListToElastic(new List<ExamplePersonModel>()
            {
                new ExamplePersonModel()
                {
                    Id = 2,
                    Name = "Serhat",
                    SurName = "BOYRAZ",
                    CreatedDate = DateTime.Now.AddMinutes(-8)
                },
                new ExamplePersonModel()
                {
                    Id = 3,
                    Name = "Emre",
                    SurName = "AKBAŞ",
                    CreatedDate = DateTime.Now.AddMinutes(-5)
                },
                new ExamplePersonModel()
                {
                    Id = 4,
                    Name = "Yunus Emre",
                    SurName = "YILDIRIM",
                    CreatedDate = DateTime.Now.AddMinutes(-3)
                },
                new ExamplePersonModel()
                {
                    Id = 5,
                    Name = "Süha",
                    SurName = "TURGUT",
                    CreatedDate = DateTime.Now.AddMinutes(-2)
                }
            });

            if (bulkResponse.IsValid || bulkResponse.Errors)
            {
                ISearchResponse<ExamplePersonModel> searchResponses = ElasticHelper<ExamplePersonModel>.Instance.Search(p => p
                    .MatchPhrasePrefix(y => y
                        .Field(z => z.Name).Query("Emr")
                    )
                );

                Console.WriteLine("\n\nBegin with 'Emr' Responses after AddModelListToElastic(indexName:default model name(ExamplePersonModel))");
                foreach (var searchResponse in searchResponses.Documents)
                {
                    Console.WriteLine(
                        $"id: {searchResponse.Id}\nname: {searchResponse.Name}\nsurname: {searchResponse.SurName}");
                }
            }


            DeleteResponse deleteResponse = ElasticExamplePersonAccessor.DeleteModelToElastic(typeof(ExamplePersonModel).Name.ToLower(), "5");

            if (deleteResponse.IsValid || deleteResponse.Result == Result.Deleted)
            {
                ISearchResponse<ExamplePersonModel> searchResponses = ElasticHelper<ExamplePersonModel>.Instance.Search(p => p
                    .DateRange(r => r
                        .Field(f => f.CreatedDate)
                        .GreaterThanOrEquals(new DateTime(2017, 01, 01))
                        .LessThanOrEquals(new DateTime(2021, 01, 01))
                    )
                );

                Console.WriteLine("\n\nErased id:5 Responses after DeleteModelToElastic(indexName:default model name(ExamplePersonModel))");
                foreach (var searchResponse in searchResponses.Documents)
                {
                    Console.WriteLine(
                        $"id: {searchResponse.Id}\nname: {searchResponse.Name}\nsurname: {searchResponse.SurName}");
                }
            }
        }

        private static void ElasticLoggerTestMethod()
        {
            try
            {
                int a = 0;
                int b = 10 / a;
            }
            catch (Exception ex)
            {
                ElasticLogger.Instance.Error(ex, ex.Message);
            }
            finally
            {
                ElasticLogger.Instance.Info("ElasticLoggerTestMethod işlemlerini gerçekleştirdi.");
            }
        }
    }
}
