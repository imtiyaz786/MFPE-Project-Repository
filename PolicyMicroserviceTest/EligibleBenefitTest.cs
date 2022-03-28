using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PolicyMicroservice.Controllers;
using PolicyMicroservice.Models;
using PolicyMicroservice.Repository;
using System;
using System.Collections.Generic;

namespace PolicyMicroserviceTest
{
    /// <summary>
    /// Contributed by Team2
    /// Test for GetEligibleBenefit of PolicyMicroservice
    /// </summary>
    class EligibleBenefitTest
    {
        List<MemberPolicy> memberPolicies;
        List<Benefits> benefitLists;
        string s = "";



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
            new Benefits{BenefitId=1,BenefitName="Medical Checkup"},
            new Benefits{BenefitId=2,BenefitName="Accidental"}
        };





        }
        /// <summary>
        /// Pass Test for GetEligibleBenefit Repository
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>

        [TestCase(1,1)]
        [TestCase(1, 2)]

        public void EligibleBenefitTest_Repo_ValidInput_ReturnsRightValue(int policyId,int memberId)
        {
            string p = "";
            Mock<IPolicyRepo> policyContextMock = new Mock<IPolicyRepo>();
            var policyRepo = new PolicyRepo();
            policyContextMock.Setup(x => x.GetEligibleBenefits(policyId, memberId)).Returns(p);
            var result = policyRepo.GetEligibleBenefits(policyId, memberId);
            Assert.IsNotNull(result);
            Assert.AreEqual("MedicalCheckup", result);

        }
        /// <summary>
        /// Fail Test for GetEligibleBenefit Repository
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        [TestCase(9,1)]
        [TestCase(9, 12)]
        public void EligibleBenefitTest_Repo_InvalidInput_ReturnsWrongValue(int policyId, int memberId)
        {
            string p = "";
            Mock<IPolicyRepo> policyContextMock = new Mock<IPolicyRepo>();
            var policyRepo = new PolicyRepo();
            policyContextMock.Setup(x => x.GetEligibleBenefits(policyId, memberId)).Returns(p);
            var result = policyRepo.GetEligibleBenefits(policyId, memberId);
            Assert.AreNotEqual("MedicalCheckup",result);
            Assert.AreEqual("Invalid Data", result);

        }

        /// <summary>
        /// Pass Test for GetEligibleBenefit Controller
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        [TestCase(1,1)]
        [TestCase(1, 2)]
        public void EligibleBenefitTest_Controller_ValidInput_ReturnsOkResultStatus(int policyId,int memberId)
        {
            

            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetEligibleBenefits(policyId, memberId)).Returns(s);
            PolicyController pc = new PolicyController(mock.Object);
            var result = pc.GetEligibleBenefit(policyId, memberId)as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);

        }
        /// <summary>
        /// Fail Test for GetEligibleBenefit Controller
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="memberId"></param>
        [TestCase(-1, -1)]
        [TestCase(-14, 1)]
        public void EligibleBenefitTest_Controller_InvalidInput_ReturnsBadRequestStatus(int policyId, int memberId)
        {


            Mock<IPolicyRepo> mock = new Mock<IPolicyRepo>();
            mock.Setup(p => p.GetEligibleBenefits(policyId, memberId)).Returns(s);
            PolicyController pc = new PolicyController(mock.Object);
            var result = (BadRequestResult)pc.GetEligibleBenefit(policyId, memberId);
            Assert.AreEqual(400, result.StatusCode);

        }

    }

 }