using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;
using Product.API.Models;
using Product.API;
using Product.API.IntegrationTests;
using Product.API.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Model = Product.API.Models;
using Product.Core.Dtos;

namespace Product.API.IntegrationTests

{
    public class ProductControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly CustomWebApplicationFactory<Startup>
            factory;

        public ProductControllerTests(
            CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_Should_Return_Ok()
        {

            // Act
            var result = await client.GetAsync($"/api/v1/product/1");


            // Assert
            result.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Should_Return_NotFound()
        {

            // Act
            var result = await client.GetAsync($"/api/product/1123");


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Get_Should_Return_BadRequest()
        {

            // Act
            var result = await client.GetAsync($"/api/v1/product/0");


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task GetUserProducts_Should_Return_Ok()
        {

            // Act
            var response = await client.GetAsync($"/api/v1/product/User/1");
            string json = response.Content.ReadAsStringAsync().Result;
            var items = JsonConvert.DeserializeObject<List<ProductDto>>(json);

            // Assert
            response.EnsureSuccessStatusCode();

        }


        [Fact]
        public async Task GetBrandProducts_Should_Return_Ok()
        {

            // Act
            var response = await client.GetAsync($"/api/v1/product/Brand/1");
            string json = response.Content.ReadAsStringAsync().Result;
            var items = JsonConvert.DeserializeObject<List<ProductDto>>(json);

            // Assert
            response.EnsureSuccessStatusCode();

        }

   

        [Theory]
        [AutoData]
        public async Task Create_Should_Return_Ok(CreateProductModel model)
        {

            // Act
            var result = await client.PostAsync(
            $"/api/v1/product/create",
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            });


            // Assert
            result.EnsureSuccessStatusCode();
        }

        [Theory]
        [MemberData(nameof(InvalidProductData))]
        public async Task Create_Should_Return_BadRequest(CreateProductModel model)
        {
            // Act
            var result = await client.PostAsync(
            $"/api/v1/product/create",
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            });


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }


        [Fact]
        public async Task Update_Should_Return_NoContent()
        {

            var model = new UpdateProductModel { Id = 1, Name = "Updated", Description = "Desc", NormalPrice = 10, DiscountedPrice = 7.5M, ImageUrl = "url", UserId = 2, BrandId = 2 };
            // Act
            var result = await client.PutAsync(
            $"/api/v1/product/update",
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            });


            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }


        [Fact]
        public async Task Update_Should_Return_NotFound()
        {
            // Arrange
            var model = new UpdateProductModel { Id = 123, Name = "Updated", Description = "Desc", NormalPrice = 10, DiscountedPrice = 7.5M, ImageUrl = "url", UserId = 2, BrandId = 2 };

            // Act
            var result = await client.PutAsync(
            $"/api/v1/product/update",
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            });


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Theory]
        [MemberData(nameof(InvalidUpdateProductData))]
        public async Task Update_Should_Return_BadRequest(UpdateProductModel model)
        {
            // Act
            var result = await client.PutAsync(
            $"/api/v1/product/update",
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            });


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Archive_Should_Return_NoContent()
        {
            // Act
            var result = await client.PatchAsync($"/api/v1/product/archive/1", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }


        [Fact]
        public async Task Archive_Should_Return_NotFound()
        {

            // Act
            var result = await client.PatchAsync($"/api/v1/product/archive/100", null);


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Archive_Should_Return_BadRequest()
        {
            // Act
            var result = await client.PatchAsync($"/api/v1/product/archive/0",null);


            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }


      



        public static IEnumerable<object[]> InvalidProductData =>
         new List<object[]>
       {
            new object[] { new CreateProductModel { Name="" }},
            new object[] { new CreateProductModel { Description="" }},
            new object[] {  new CreateProductModel { NormalPrice=0}},
            new object[] {  new CreateProductModel { DiscountedPrice=0 } },
            new object[] {  new CreateProductModel { UserId=0 } },
            new object[] {  new CreateProductModel { BrandId=0 } }
       };
        public static IEnumerable<object[]> InvalidUpdateProductData =>
     new List<object[]>
     {
          new object[] { new UpdateProductModel { Id=0 }},
            new object[] { new UpdateProductModel { Name="" }},
            new object[] { new UpdateProductModel { Description="" }},
            new object[] {  new UpdateProductModel { NormalPrice=0}},
            new object[] {  new UpdateProductModel { DiscountedPrice=0 } },
            new object[] {  new UpdateProductModel { UserId=0 } },
            new object[] {  new UpdateProductModel { BrandId=0 } }
     };











    }
}