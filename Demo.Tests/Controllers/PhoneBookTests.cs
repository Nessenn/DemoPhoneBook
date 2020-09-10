using AutoMapper;
using Demo.APIControllers;
using Demo.AutoMapper;
using Demo.Common.Extensions.Grid;
using Demo.Configs;
using Demo.DbModel;
using Demo.Dto.PhoneBook;
using Demo.Infrastructure.Services;
using Demo.SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Tests
{
    public class PhoneBookTests
    {
        #region Properties
        
        private PhoneBooksController _controller;

        #endregion

        #region setup/down
        [SetUp]
        public void TestUp()
        {

            IUnitOfWorkFactory uowf = new UnitOfWorkFactory(null, true);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PhoneBookProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var service = new PhoneBookService(uowf, mapper);
            _controller = new PhoneBooksController(service, null);

            using (var ouw = uowf.Create())
            {
                var phoneBook = new PhoneBook()
                {
                    Address = "Address1",
                    PhoneNumber = "1234567890",
                    Name = "Jerry Smith"
                };

                var phoneBook1 = new PhoneBook()
                {
                    Address = "Address2",
                    PhoneNumber = "444455551",
                    Name = "Jon Doo"
                };

                var phoneBook2 = new PhoneBook()
                {
                    Address = "Address2",
                    PhoneNumber = "666555",
                    Name = "Beth Doo"
                };

                var phoneBook3 = new PhoneBook()
                {
                    Address = "Address3",
                    PhoneNumber = "22222111111",
                    Name = "Rick Voodoo"
                };


                var phoneBook4 = new PhoneBook()
                {
                    Address = "Address4",
                    PhoneNumber = "89333333",
                    Name = "Morty Doo"
                };

                var phoneBook5 = new PhoneBook()
                {
                    Address = "Address5",
                    PhoneNumber = "9999999",
                    Name = "Summer Doo"
                };

                ouw.Repository.Add(phoneBook);
                ouw.Repository.Add(phoneBook1);
                ouw.Repository.Add(phoneBook2);
                ouw.Repository.Add(phoneBook3);
                ouw.Repository.Add(phoneBook4);
                ouw.Repository.Add(phoneBook5);

                ouw.SaveChanges();
            }

        }


        [TearDown]
        public void TestTearDown()
        {
            IUnitOfWorkFactory uowf = new UnitOfWorkFactory(null, true);
          
            _controller = null;

            using (var ouw = uowf.Create())
            {
                foreach (var item in ouw.Repository.Query<PhoneBook>().ToList())
                {
                    ouw.Repository.Remove<PhoneBook>(item.Id);
                }

                ouw.SaveChanges();
            }

        }
        #endregion

        #region Get Data
        [Test]
        public void GetPhoneBooksOrderByNameDesc()
        {
            //Arrange

            //Act
            var sort = new List<Sort>();
            sort.Add(new Sort() { Dir = "desc", Field = "name" });
            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = new List<Filter>() },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;
            

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(data[0].Name, "Summer Doo");
            Assert.AreEqual(data[0].Address, "Address5");
        }

        [Test]
        public void GetPhoneBooksFilterByName()
        {
            //Arrange

            //Act
            var sort = new List<Sort>();
            var filters = new List<Filter>();
            filters.Add(new Filter() { Field = "name", Logic = "AND", Value = "Doo", Operator = "contains" });
            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;


            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(4, data.Count);
            Assert.AreEqual(4, ((DataSourceResult)((ObjectResult)result).Value).Total);
        }

        [Test]
        public void GetPhoneBooksFilterByAllFields()
        {
            //Arrange

            //Act
            var sort = new List<Sort>();
            var filters = new List<Filter>();
            filters.Add(new Filter() { Field = "name", Logic = "AND", Value = "Doo", Operator = "contains" });
            filters.Add(new Filter() { Field = "phoneNumber", Logic = "AND", Value = "9", Operator = "contains" });
            filters.Add(new Filter() { Field = "address", Logic = "AND", Value = "5", Operator = "contains" });
            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;


            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(1, ((DataSourceResult)((ObjectResult)result).Value).Total);
        }

        [Test]
        public void GetPhoneBooksPagingCount()
        {
            //Arrange

            //Act
            var sort = new List<Sort>();
            var filters = new List<Filter>();
        
            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 5,
                Sort = sort
            }).Result;


            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(6, ((DataSourceResult)((ObjectResult)result).Value).Total);
        }

        [Test]
        public void GetPhoneBooksPagingGetFirstPage()
        {
            //Arrange

            //Act
            var sort = new List<Sort>();
            var filters = new List<Filter>();

            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 5,
                Sort = sort
            }).Result;


            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(5, data.Count);
            Assert.AreEqual(6, ((DataSourceResult)((ObjectResult)result).Value).Total);
        }

        [Test]
        public void GetPhoneBooksPagingGetLastPage()
        {

            //Arrange

            //Act
            var sort = new List<Sort>();
            var filters = new List<Filter>();

            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 5,
                Take = 10,
                Sort = sort
            }).Result;


            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(1, data.Count);
            Assert.AreEqual(6, ((DataSourceResult)((ObjectResult)result).Value).Total);
        }

        [Test]
        public void GetPhoneBooksGetAllData()
        {
            //Arrange
            var sort = new List<Sort>();
            var filters = new List<Filter>();

            //Act
            var result = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;


            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = (((DataSourceResult)((ObjectResult)result).Value).Data) as List<GetPhoneBookDto>;

            Assert.AreEqual(6, data.Count);
            Assert.AreEqual(6, ((DataSourceResult)((ObjectResult)result).Value).Total);
        }

        [Test]
        public void GetPhoneBookGetExistsRow()
        {
            //Arrange
            var sort = new List<Sort>();
            var filters = new List<Filter>();
            //Act
            var resultList = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;

            Assert.AreEqual(resultList.GetType(), typeof(OkObjectResult));
            var dataList = (((DataSourceResult)((ObjectResult)resultList).Value).Data) as List<GetPhoneBookDto>;

            var result = _controller.GetPhoneBook(dataList.FirstOrDefault(v=> 
                v.PhoneNumber == "1234567890" &&
                v.Name == "Jerry Smith" &&
                v.Address == "Address1").Id).Result;

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = ((GetPhoneBookDto)((ObjectResult)result).Value);

            Assert.AreEqual(data.PhoneNumber, "1234567890");
            Assert.AreEqual(data.Name, "Jerry Smith");
            Assert.AreEqual(data.Address, "Address1");

        }

        [Test]
        public void GetPhoneBookGetNonExistsRow()
        {
            //Act
            var result = _controller.GetPhoneBook(9999).Result;

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            Assert.Null(((ObjectResult)result).Value);
        }

        #endregion

        #region Post

        [Test]
        public void PostPhoneBookCreateNew()
        {
            //Arrange
            var phoneBook = new SavePhoneBookDto() {
                Address = "The Death Star",
                Name= "Darth Vader",
                PhoneNumber = "55556666777"
            };

            var sort = new List<Sort>();
            var filters = new List<Filter>();

            //Act
            var result = _controller.PostPhoneBook(phoneBook).Result;
            var resultActual = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;

            
            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = ((DataSourceResult)((ObjectResult)resultActual).Value).Data as List<GetPhoneBookDto>;

            Assert.IsNotNull(data.FirstOrDefault(v =>
                v.Address == "The Death Star" &&
                v.Name == "Darth Vader" &&
                v.PhoneNumber == "55556666777"));
        }

        #endregion

        #region Put
        [Test]
        public void PutPhoneBookUpdateExist()
        {
            //Arrange
            var dto = new SavePhoneBookDto()
            {
                Id = 1,
                Address = "The Death Star",
                Name = "Darth Vader",
                PhoneNumber = "55556666777"
            };
            var sort = new List<Sort>();
            var filters = new List<Filter>();

            //Act
            var resultList = _controller.GetPhoneBooks(new DataSourceRequest()
            {
                Filter = new Filter() { Filters = filters },
                Skip = 0,
                Take = 100,
                Sort = sort
            }).Result;

            Assert.AreEqual(resultList.GetType(), typeof(OkObjectResult));
            var dataList = (((DataSourceResult)((ObjectResult)resultList).Value).Data) as List<GetPhoneBookDto>;
            dto.Id = dataList.FirstOrDefault(v =>
                v.PhoneNumber == "1234567890" &&
                v.Name == "Jerry Smith" &&
                v.Address == "Address1").Id;

            var result = _controller.PutPhoneBook(dto).Result;
            var resultData = _controller.GetPhoneBook(dto.Id.Value).Result;

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = ((GetPhoneBookDto)((ObjectResult)resultData).Value);

            Assert.AreEqual(data.Address, "The Death Star");
            Assert.AreEqual(data.Name, "Darth Vader");
            Assert.AreEqual(data.PhoneNumber,  "55556666777");
        }

        [Test]
        public void PutPhoneBookUpdateNonExist()
        {
            //Arrange
            var dto = new SavePhoneBookDto()
            {
                Id = 9999,
                Address = "The Death Star",
                Name = "Darth Vader",
                PhoneNumber = "Darth Vader"
            };

            //Act
            var result = _controller.PutPhoneBook(dto).Result;
            var resultData = _controller.GetPhoneBook(9999).Result;

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = ((GetPhoneBookDto)((ObjectResult)resultData).Value);

            Assert.IsNull(data);
        }

        #endregion

        #region Delete

        [Test]
        public void DeletePhoneBookDeleteExist()
        {
            //Arrange

            //Act
            var result = _controller.DeletePhoneBook(1).Result;
            var resultData = _controller.GetPhoneBook(1).Result;

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = ((GetPhoneBookDto)((ObjectResult)resultData).Value);

            Assert.IsTrue((bool)((ObjectResult)result).Value);
            Assert.IsNull(data);
        }

        [Test]
        public void DeletePhoneBookDeleteNonExist()
        {
            //Arrange

            //Act
            var result = _controller.DeletePhoneBook(99999).Result;
            var resultData = _controller.GetPhoneBook(99999).Result;

            //Assert
            Assert.AreEqual(result.GetType(), typeof(OkObjectResult));
            var data = ((GetPhoneBookDto)((ObjectResult)resultData).Value);

            Assert.IsFalse((bool)((ObjectResult)result).Value);
            Assert.IsNull(data);
        }
        #endregion
    }
}