﻿using System;
using System.Linq;
using DasKlub.Models.Forum;
using DasKlub.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlubTests.Interaction
{
    [TestClass]
    public class ForumTest
    {
        [TestMethod]
        public void ForumCategory_CanBeCreated_ReturnsID()
        {
            // arrange
            var uniqueKey = Guid.NewGuid().ToString();

            // act
            using (var context = new DasKlubDBContext())
            {
                context.ForumCategory.Add(new ForumCategory
                    {
                        Description = Guid.NewGuid().ToString(),
                        Title = Guid.NewGuid().ToString(),
                        Key = uniqueKey,
                        CreatedByUserID =  0
                    });

                context.SaveChanges();
            }

            // assert
            using ( var context = new DasKlubDBContext())
            {
                Assert.IsNotNull(context.ForumCategory.FirstOrDefault().Key == uniqueKey);
            }
        }
 
    }
}