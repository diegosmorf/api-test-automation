using ApiTestAutomation.Response;
using FluentRest;
using FluentRest.Fake;
using NUnit.Framework;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace ApiTestAutomation.Steps
{
    [Binding]
    public class ApiGetExampleSteps
    {
        private string apiUrl = "";
        private FluentClient client;
        private FluentResponse response;
        private Post result;

        [Given(@"a collection of posts exposed as api")]
        public void GivenACollectionOfPostsExposedAsApi()
        {
            //arrange
            apiUrl = "https://jsonplaceholder.typicode.com";
            client = new FluentClient();
            client.BaseUri = new Uri(apiUrl, UriKind.Absolute);
        }

        [When(@"I request post with id (.*)")]
        public void WhenIRequestPostWithId(int id)
        {
            //act
            response = client.GetAsync(b => b
                .AppendPath("posts")
                .AppendPath(id.ToString())).Result;
        }

        [Then(@"response StatusCode is Ok")]
        public void ThenResponseStatusCodeIsOk()
        {
            //assert
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            result = response.DeserializeAsync<Post>().Result;
            Assert.NotNull(result);
        }


        [Then(@"response content is composed by title with value ""(.*)""")]
        public void ThenResponseContentIsComposedByTitleWithValue(string title)
        {
            //assert
            Assert.AreEqual(title, result.Title);
        }

        [Then(@"response content is composed by id with value (.*)")]
        public void ThenResponseContentIsComposedByIdWithValue(int id)
        {
            //assert
            Assert.AreEqual(id, result.Id);
        }

        [Then(@"response content is composed by userId with value ""(.*)""")]
        public void ThenResponseContentIsComposedByUserIdWithValue(string userId)
        {
            //assert
            Assert.AreEqual(userId, result.UserId);
        }

        [Then(@"response content is composed by body with value ""(.*)""")]
        public void ThenResponseContentIsComposedByBodyWithValue(string body)
        {
            //assert
            Assert.AreEqual(body, result.Body.Replace("\n", " "));
        }
        
        [Then(@"response content is composed by all fields with value (.*)")]
        public void ThenResponseContentIsComposedByAllFieldsWithValue(int id)
        {
            //arrange
            var responseObject = new Post() { Id = 2, Body = "2", Title = "2", UserId = "2" };            

            MemoryMessageStore.Current.Register(b => b
                    .Url($"{apiUrl}/posts/{id}")
                    .StatusCode(HttpStatusCode.OK)
                    .ReasonPhrase("OK")
                    .Content(c => c
                    .Header("Content-Type", "application/json; charset=utf-8")
                    .Data(responseObject) // object to be JSON serialized
                )
            );

            // use memory store by default
            var serializer = new JsonContentSerializer();
            var fakeHttp = new FakeMessageHandler();
            var client = new FluentClient(serializer, fakeHttp);
            client.BaseUri = new Uri(apiUrl, UriKind.Absolute);

            //act            
            response = client.GetAsync(b => b
                .AppendPath("posts")
                .AppendPath(id.ToString())).Result;

            //assert
            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            result = response.DeserializeAsync<Post>().Result;
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(id.ToString(), result.Title);
            Assert.AreEqual(id.ToString(), result.UserId);
            Assert.AreEqual(id.ToString(), result.Body);
        }

    }
}