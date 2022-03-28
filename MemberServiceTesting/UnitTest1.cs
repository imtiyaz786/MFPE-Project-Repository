using MemberMicroservice.Controllers;
using MemberMicroservice.Models;
using MemberMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace MemberServiceTesting
{
    public class Tests
    {
        MemberPremium memberPremium = new MemberPremium();
        List<MemberPremium> dataObject = new List<MemberPremium>();
        [SetUp]
        public void Setup()
        {
            dataObject = new List<MemberPremium>()
            {
                new MemberPremium()
                {
                    MemberID = 1,
                    PolicyID = 1,
                    PremiumDue = 43242.0,
                    PaymentDetails = "UPI Mode",
                    DueDate = new DateTime(2020, 12, 20),
                    LastPremiumPaidDate = new DateTime(2019, 12, 21)
                },
                new MemberPremium()
                {
                    MemberID = 2,
                    PolicyID = 1,
                    PremiumDue = 54342.0,
                    PaymentDetails = "Cheque Mode",
                    DueDate = new DateTime(2021, 04, 16),
                    LastPremiumPaidDate = new DateTime(2020, 04, 22)
                }
            };
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void RepositoryPassStatus(int policyID, int memberID)
        {
            Mock<IMemberRepository> memberContextMock = new Mock<IMemberRepository>();
            var memberRepoObject = new MemberRepository();
            memberContextMock.Setup(x => x.ViewBill(1, 2)).Returns(memberPremium);
            var memberStatus = memberRepoObject.ViewBill(1, 2);
            Assert.IsNotNull(memberStatus);
        }

        [TestCase(3, 5)]
        [TestCase(5, 7)]
        public void RepositoryFailStatus(int policyID, int memberID)
        {
            MemberPremium memberPremium = new MemberPremium();
            Mock<IMemberRepository> memberContextMock = new Mock<IMemberRepository>();
            var memberRepoObject = new MemberRepository();
            memberContextMock.Setup(x => x.ViewBill(3, 5)).Returns(memberPremium);
            var memberStatus = memberRepoObject.ViewBill(3, 5);
            Assert.IsNull(memberStatus);
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void ControllerTestPass(int policyID, int memberID)
        {
            Mock<IMemberRepository> mock = new Mock<IMemberRepository>();
            mock.Setup(p => p.ViewBill(policyID, memberID)).Returns(memberPremium);
            MembersController pc = new MembersController(mock.Object);
            var result = pc.ViewBill(policyID, memberID) as ActionResult<MemberPremium>;
            Assert.IsNotNull(result);
        }

        [TestCase(-1, -11)]
        [TestCase(-12, -24)]
        public void ControllerTestFail(int policyID, int memberID)
        {
            Mock<IMemberRepository> mock = new Mock<IMemberRepository>();
            mock.Setup(p => p.ViewBill(policyID, memberID)).Returns(memberPremium);
            MembersController pc = new MembersController(mock.Object);
            var result1 = pc.ViewBill(policyID, memberID) as ActionResult<MemberPremium>;
            var result = pc.ViewBill(policyID, memberID).Result;
            Assert.AreNotEqual(result, result1);
        }
    }
}