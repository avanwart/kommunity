using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DasKlub.Models.Shopping;

namespace DasKlub.Web.Controllers
{
    public class ItemsController : ApiController
    {
        private readonly IItemRepository _repo;

        public ItemsController(IItemRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Item> Get()
        {
            return _repo.GetTopics().OrderByDescending(x => x.CreateDate);
        }
    }

    public interface IItemRepository
    {
        IEnumerable<Item> GetTopics();
    }
}
