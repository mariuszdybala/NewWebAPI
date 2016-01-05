using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;

namespace CountingKs.Controllers
{
    public class MeasuresController : BaseController
    {
        public MeasuresController(ICountingKsRepository repo) :base(repo)
        {
        }

        public IEnumerable<MeasureModel> Get(int foodid)
        {
            var result = TheRepository.GetMeasuresForFood(foodid)
                .ToList()
                .Select(x => TheModelFactory.Create(x));
            return result;
        }

        public MeasureModel Get(int foodid, int id)
        {
            var result = TheRepository.GetMeasure(id);
            if (result.Food.Id == foodid)
                return TheModelFactory.Create(result);
            return null;
        }
    }
}
