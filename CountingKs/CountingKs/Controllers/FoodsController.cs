using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace CountingKs.Controllers
{
    public class FoodsController : BaseController
    {

        const int PAGE_SIZE = 50;

        public FoodsController(ICountingKsRepository repo)
            : base(repo)
        {
        }


        public object Get(bool includeMeasures = true, int page = 0)
        {
            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = TheRepository.GetAllFoodsWithMeasures();
            }
            else
            {
                query = TheRepository.GetAllFoods();
            }

            var baseQuery = query
                .OrderBy(x => x.Description);

            var totalCount = query.Count();
            var totalPages = Math.Ceiling((double)totalCount / PAGE_SIZE);

            var helper = new UrlHelper(this.Request);
            var prevUrl = helper.Link("Food", new { page = page - 1 });
            var nextPage = helper.Link("Food", new { page = page + 1 });

            var result = query.ToList().Skip(PAGE_SIZE * page)
                .Take(PAGE_SIZE)
                .ToList()
                .Select(x => TheModelFactory.Create(x));

            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                NextUrl = nextPage,
                PrevUrl = prevUrl,
                Results = result,
            };
        }

        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheRepository.GetFood(foodid));
        }

    }
}
