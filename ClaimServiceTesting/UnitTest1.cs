using ClaimsMicroservice.Controllers;
using ClaimsMicroservice.Models;
using ClaimsMicroservice.Repository;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ClaimServiceTesting
{
    public class Tests
    {
        string s = "";
        List<Claim> dataObject = new List<Claim>();
        [SetUp]
        public void Setup()
        {
            dataObject = new List<Claim>()
            {
                new Claim()
                {
                    ClaimID = 1,
                    ClaimStatus = "Pending",
                    PolicyID = 1,
                    AmountClaimed = 45300,
                    BenefitsAvailed = "Medicine & Checkup",
                    HospitalID = 1,
                    Remarks = "Verified",
                    Settled = "False"
                },
                new Claim()
                {
                    ClaimID = 2,
                    ClaimStatus = "Rejected",
                    PolicyID = 1,
                    AmountClaimed = 54340,
                    BenefitsAvailed = "Medicine & Checkup",
                    HospitalID = 1,
                    Remarks = "Verified",
                    Settled = "False"
                }
            };
        }

        [TestCase(2, 1, 2)]
        [TestCase(1, 1, 1)]
        public void RepositoryGetStatusPass(int claimID, int policyID, int memberID)
        {
            string p = "";
            Mock<IClaimRepository> claimContextMock = new Mock<IClaimRepository>();
            var claimRepoObject = new ClaimRepository();
            claimContextMock.Setup(x => x.GetClaimStatus(claimID, policyID, memberID)).Returns(p);
            var claimStatus = claimRepoObject.GetClaimStatus(claimID, policyID, memberID);
            Assert.IsNotNull(claimStatus);
            if(claimID == 2)
                Assert.AreEqual("Rejected", claimStatus);
            else
                Assert.AreEqual("Pending", claimStatus);
        }


        [TestCase(2, 2, 2)]
        [TestCase(22, 12, 24)]
        public void RepositoryGetStatusFail(int claimID, int policyID, int memberID)
        {
            string p = "";
            Mock<IClaimRepository> claimContextMock = new Mock<IClaimRepository>();
            var claimRepoObject = new ClaimRepository();
            claimContextMock.Setup(x => x.GetClaimStatus(claimID, policyID, memberID)).Returns(p);
            var claimStatus = claimRepoObject.GetClaimStatus(claimID, policyID, memberID);
            Assert.IsNull(claimStatus);
            Assert.AreNotEqual("Pending", claimStatus);
        }

        [TestCase(2, 1, 1)]
        [TestCase(1, 1, 1)]
        public void ControllerGetStatusPass(int claimID, int policyID, int memberID)
        {
            Mock<IClaimRepository> mock = new Mock<IClaimRepository>();
            mock.Setup(p => p.GetClaimStatus(claimID, policyID, memberID)).Returns(s);
            ClaimsController pc = new ClaimsController(mock.Object);
            var result = pc.GetClaimStatus(claimID, policyID, memberID).Result;
            Assert.IsNotNull(result);
        }

        [TestCase(-2, -1, -3)]
        [TestCase(40, -82, -90)]
        public void ControllerGetStatusFail(int claimID, int policyID, int memberID)
        {
            Mock<IClaimRepository> mock = new Mock<IClaimRepository>();
            mock.Setup(p => p.GetClaimStatus(claimID, policyID, memberID)).Returns(s);
            ClaimsController pc = new ClaimsController(mock.Object);
            var result = pc.GetClaimStatus(claimID, policyID, memberID).Result;
            Assert.IsNotNull(result);
        }
    }
}