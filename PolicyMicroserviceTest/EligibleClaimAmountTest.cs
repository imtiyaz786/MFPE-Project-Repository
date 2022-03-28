using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PolicyMicroservice.Controllers;
using PolicyMicroservice.Models;
using PolicyMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyMicroserviceTest
{
    /// <summary>
    /// Contributed by Team 2
    /// Test for GetEligibleClaimAmount of Policy Service
    /// </summary>
    class EligibleClaimAmountTest
    {
        List<MemberPolicy> memberPolicies = new List<MemberPolicy>();
        List<Benefits> benefitLists = new List<Benefits>();
        Double d;


        [SetUp]
        public void Setup()
        {

            memberPolicies = new List<MemberPolicy>
        {
           new MemberPolicy{MemberId=1,PolicyId=1,PolicyNo=101,BenefitId=1,Tenure=3,SubscriptionDate=new DateTime(2020, 03, 15),CapAmountBenefits=100000.00},
           new MemberPolicy{MemberId=2,PolicyId=1,PolicyNo=101,BenefitId=1,Tenure=3,SubscriptionDate=new DateTime(2019, 04, 18),CapAmountBenefits=120000.00},
           new MemberPolicy{MemberId=3,PolicyId=2,PolicyNo=102,BenefitId=1,Tenure=5,SubscriptionDate=new DateTime(2019, 05, 10),CapAmountBenefits=80000.00}
        };
            benefitLists = new List<Benefits>
        {
            new Benefits{BenefitId=1,BenefitName="MedicalCheckup"},
            new Benefits{BenefitId=2,BenefitName="Accidental"}
        };
           d = 0.0;





        }
        /// <summary>
        /// Pass Test for GetEligibleClaimAmount Repository
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        /// <param name="benefitId"></param>

        [TestCase(1,1,1)]
        public void GetEligbleClaimAmount_Repo_ValidInput_ReturnsRightValue(int policyId,int memberId,int benefitId)
        {
            double d = 0.0;
            Mock<IPolicyRepo> policyContextMock = new Mock<IPolicyRepo>();
            var policyRepo = new PolicyRepo();
            policyContextMock.Setup(x => x.GetEligibleClaimAmount(policyId, memberId, benefitId)).Returns(d);
            var result = policyRepo.GetEligibleClaimAmount(policyId, memberId, benefitId);
            Assert.IsNotNull(result);
            Assert.AreEqual(100000.00, result);

        }
        /// <summary>
        /// Fail Test for GetEligibleClaimAmount Repository
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        /// <param name="benefitId"></param>
        [TestCase(11, 12, 11)]
        [TestCase(1, 12, 71)]
        public void GetEligbleClaimAmount_Repo_InvalidInput_ReturnsWrongValue(int policyId, int memberId, int benefitId)
        {
            //double d = 0.0;
            Mock<IPolicyRepo> policyContextMock = new Mock<IPolicyRepo>();
            var policyRepo = new PolicyRepo();
            policyContextMock.Setup(x => x.GetEligibleClaimAmount(policyId, memberId, benefitId)).Returns(d);
            var result = policyRepo.GetEligibleClaimAmount(policyId, memberId, benefitId);
            Assert.AreEqual(-1, result);
            Assert.AreNotEqual(100000.00, result);

        }
        /// <summary>
        /// Pass Test for GetEligibleClaimAmount Controller
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        /// <param name="benefitId"></param>
        [TestCase(1, 1,1)]
        [TestCase(1, 2, 1)]
        public void GetEligbleClaimAmount_Controller_ValidInput_ReturnsOkResponseStatus(int policyId, int memberId,int benefitId)
        {
           // double d=0.0;
           // IActionResult i;
            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetEligibleClaimAmount(policyId, memberId,benefitId)).Returns(d);
            PolicyController pc = new PolicyController(mock.Object);
            OkObjectResult result = pc.GetEligibleClaimAmount(policyId, memberId,benefitId) as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);

        }
        /// <summary>
        /// Fail Test for GetEligibleClaimAmount Repository
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        /// <param name="benefitId"></param>
        [TestCase(-11, -1, 1)]
        [TestCase(-1, -1, 1)]
        public void GetEligbleClaimAmount_Controller_InvalidInput_ReturnsBadRequestStatus(int policyId, int memberId, int benefitId)
        {
            
            
                Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
                mock.Setup(p => p.GetEligibleClaimAmount(policyId, memberId, benefitId)).Returns(d);
                PolicyController pc = new PolicyController(mock.Object);
                var result =(BadRequestResult) pc.GetEligibleClaimAmount(policyId, memberId, benefitId);
                Assert.AreEqual(400, result.StatusCode);



        }
    }
}
