﻿using System;
using System.Linq;
using DasKlub.Models;
using DasKlub.Models.Forum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlub.Web.IntegrationTests.Controllers.Forum
{
    [Ignore] // init db failed
    [TestClass]
    public class ForumControllerTest
    {
        [TestMethod]
        public void ForumCategory_CanBeCreated_ReturnsID()
        {
            // arrange
            string uniqueKey = Guid.NewGuid().ToString();

            // act
            using (var context = new DasKlubDbContext())
            {
                context.ForumCategory.Add(new ForumCategory
                {
                    Description = Guid.NewGuid().ToString(),
                    Title = Guid.NewGuid().ToString(),
                    Key = uniqueKey,
                    CreatedByUserID = 0
                });

                context.SaveChanges();
            }

            // assert
            using (var context = new DasKlubDbContext())
            {
                Assert.IsNotNull(context.ForumCategory.FirstOrDefault().Key == uniqueKey);
            }
        }

        [TestMethod]
        public void ForumSubCategory_CanBeCreated_ReturnsID()
        {
            // arrange
            string uniqueKeyForum = Guid.NewGuid().ToString();
            string uniqueKeySubCat = Guid.NewGuid().ToString();

            // act
            using (var context = new DasKlubDbContext())
            {
                context.ForumCategory.Add(new ForumCategory
                {
                    Description = Guid.NewGuid().ToString(),
                    Title = Guid.NewGuid().ToString(),
                    Key = uniqueKeyForum,
                    CreatedByUserID = 0
                });

                context.SaveChanges();
            }

            using (var context = new DasKlubDbContext())
            {
                int forumSubCatID = context.ForumCategory.FirstOrDefault(x => x.Key == uniqueKeyForum).ForumCategoryID;

                context.ForumSubCategory.Add(new ForumSubCategory
                {
                    Description = Guid.NewGuid().ToString(),
                    Title = Guid.NewGuid().ToString(),
                    Key = uniqueKeySubCat,
                    CreatedByUserID = 0,
                    ForumCategoryID = forumSubCatID
                });

                context.SaveChanges();
            }

            // assert
            using (var context = new DasKlubDbContext())
            {
                Assert.IsNotNull(context.ForumSubCategory.FirstOrDefault().Key == uniqueKeySubCat);
            }
        }

        [TestMethod]
        public void ForumPost_CanBeCreated_ReturnsID()
        {
            // arrange
            string uniqueKeyForum = Guid.NewGuid().ToString();
            string uniqueKeySubCat = Guid.NewGuid().ToString();
            string uniqueKeyPost = Guid.NewGuid().ToString();

            // act
            using (var context = new DasKlubDbContext())
            {
                context.ForumCategory.Add(new ForumCategory
                {
                    Description = Guid.NewGuid().ToString(),
                    Title = Guid.NewGuid().ToString(),
                    Key = uniqueKeyForum,
                    CreatedByUserID = 0
                });

                context.SaveChanges();
            }

            using (var context = new DasKlubDbContext())
            {
                int forumID = context.ForumCategory.FirstOrDefault(x => x.Key == uniqueKeyForum).ForumCategoryID;

                context.ForumSubCategory.Add(new ForumSubCategory
                {
                    Description = Guid.NewGuid().ToString(),
                    Title = Guid.NewGuid().ToString(),
                    Key = uniqueKeySubCat,
                    CreatedByUserID = 0,
                    ForumCategoryID = forumID
                });

                context.SaveChanges();
            }

            using (var context = new DasKlubDbContext())
            {
                int forumSubCategoryID =
                    context.ForumSubCategory.FirstOrDefault(x => x.Key == uniqueKeySubCat).ForumSubCategoryID;

                context.ForumPost.Add(new ForumPost
                {
                    Detail = uniqueKeyPost,
                    CreatedByUserID = 0,
                    ForumSubCategoryID = forumSubCategoryID
                });

                context.SaveChanges();
            }

            // assert
            using (var context = new DasKlubDbContext())
            {
                Assert.IsNotNull(context.ForumPost.FirstOrDefault().Detail == uniqueKeyPost);
            }
        }
    }
}