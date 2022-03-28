using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PolicyMicroservice;
using PolicyMicroservice.Controllers;
using PolicyMicroservice.Models;
using PolicyMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolicyMicroserviceTest
{
    /// <summary>
    /// Contributed by Team 2
    /// </summary>
    class PolicyGetProviderListTest
    {
        List<ProviderPolicy> providerPolicies = new List<ProviderPolicy>();
        

        [SetUp]
        public void Setup()
        {

            providerPolicies = new List<ProviderPolicy>()
            {  new ProviderPolicy{HospitalId=1,HospitalName="Apollo Hospital",HospitalAddress="Beleghata Road",Location="Kolkata",PolicyId=1},
               new ProviderPolicy{HospitalId=2,HospitalName="Mission Hospital",HospitalAddress="Bidhan Nagar",Location="Durgapur",PolicyId=1},
               new ProviderPolicy{HospitalId=3,HospitalName="AMRI Hospital",HospitalAddress="Salt Lake",Location="Kolkata",PolicyId=2}
            };
           




        }
        /// <summary>
        /// Pass Test For GetChainOfProviders Repository
        /// </summary>
        /// <param name="policyId"></param>

        [TestCase(1)]
        public void PolicyGetProviderList_Repo_ValidInput_ReturnsRightValue(int policyId)
        {
            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetChainOfProviders(policyId)).Returns(providerPolicies);
            PolicyRepo pr = new PolicyRepo();
            var result = pr.GetChainOfProviders(policyId);
            Assert.AreEqual(2, result.Count());

        }
        /// <summary>
        /// Fail Test For GetChainOfProviders Repository
        /// </summary>
        /// <param name="policyId"></param>
        [TestCase(9)]
        [TestCase(14)]
        public void PolicyGetProviderList_Repo_InvalidInput_ReturnsWrongValue(int policyId)
        {
            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetChainOfProviders(policyId)).Returns(providerPolicies);
            PolicyRepo pr = new PolicyRepo();
            var result = pr.GetChainOfProviders(policyId);
            Assert.AreEqual(0, result.Count());
            
           


        }
        /// <summary>
        /// Pass Test For GetChainOfProviders Controller
        /// </summary>
        /// <param name="policyId"></param>
        [TestCase(1)]
        [TestCase(2)]
        public void PolicyGetProviderList_Controller_ValidInput_ReturnsOkResponseStatus(int policyId)
        {
            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetChainOfProviders(policyId)).Returns(providerPolicies);
            PolicyController pc = new PolicyController(mock.Object);
            var result =pc.GetChainOfProviders(policyId) as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);

        }
        /// <summary>
        /// Fail Test For GetChainOfProviders Controller
        /// </summary>
        /// <param name="policyId"></param>
        [TestCase(-1)]
        [TestCase(-14)]
        public void PolicyGetProviderList_Controller_InvalidInput_ReturnsBadRequestStatus(int policyId)
        {
            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetChainOfProviders(policyId)).Returns(providerPolicies);
            PolicyController pc = new PolicyController(mock.Object);
            var result =(BadRequestResult) pc.GetChainOfProviders(policyId);
          


            Assert.AreEqual(400, result.StatusCode);


        }
    }
}
