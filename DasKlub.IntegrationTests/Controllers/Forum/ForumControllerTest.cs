using System;
using System.Linq;
using DasKlub.Models;
using DasKlub.Models.Forum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlub.IntegrationTests.Controllers.Forum
{
    [TestClass]
    public class ForumControllerTest
    {
        [TestMethod]
        public void ForumCategory_CanBeCreated_ReturnsID()
        {
            // arrange
            var uniqueKey = Guid.NewGuid().ToString();

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
            var uniqueKeyForum = Guid.NewGuid().ToString();
            var uniqueKeySubCat = Guid.NewGuid().ToString();

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
                var forumSubCatID = context.ForumCategory.FirstOrDefault(x => x.Key == uniqueKeyForum).ForumCategoryID;

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
            var uniqueKeyForum = Guid.NewGuid().ToString();
            var uniqueKeySubCat = Guid.NewGuid().ToString();
            var uniqueKeyPost = Guid.NewGuid().ToString();

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
                var forumID = context.ForumCategory.FirstOrDefault(x => x.Key == uniqueKeyForum).ForumCategoryID;

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
                var forumSubCategoryID = context.ForumSubCategory.FirstOrDefault(x => x.Key == uniqueKeySubCat).ForumSubCategoryID;

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