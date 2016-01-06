using CountingKs.Data;
using CountingKs.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace CountingKs.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;
        private ICountingKsRepository _repo;

        public ModelFactory(HttpRequestMessage request, ICountingKsRepository repo)
        {
            _urlHelper = new UrlHelper(request);
            _repo = repo;
        }

        public FoodModel Create(Food food)
        {
            return new FoodModel() {
                Url = _urlHelper.Link("Food", new {foodid = food.Id}),
                Description = food.Description,
                Measures = food.Measures.Select(x=>Create(x))
            };
        }

        public MeasureModel Create(Measure measure)
        {
            return new MeasureModel()
                {
                    Url = _urlHelper.Link("Measures", new { foodid = measure.Food.Id, id = measure.Id}),
                    Description = measure.Description,
                    Calories = measure.Calories

                };
        }

        public DiaryModel Create(Diary diary)
        {
            return new DiaryModel()
            {
                Url = _urlHelper.Link("Diaries", new { diaryid = diary.CurrentDate.ToString("yyyy-MM-dd") }),
                CurrentDate = diary.CurrentDate,
                Entries = diary.Entries.Select(x=>Create(x))
            };
        }

        public DiaryEntryModel Create(DiaryEntry entry)
        {
            return new DiaryEntryModel()
            {
                Url = _urlHelper.Link("DiaryEntries", new { diaryid = entry.Diary.CurrentDate.ToString("yyyy-MM-dd"), id = entry.Id }),
                Quantity = entry.Quantity,
                FoodDescription = entry.FoodItem.Description,
                MeasureDescription = entry.Measure.Description,
                MeasureUrl = _urlHelper.Link("Measures", new { foodid = entry.FoodItem.Id, id = entry.Measure.Id })
            };
        }


        public DiaryEntry Parse(DiaryEntryModel model)
        {
            try
            {
                var entry = new DiaryEntry();
                if(model.Quantity != default(double))
                {
                    entry.Quantity = model.Quantity;
                }
                if(!string.IsNullOrWhiteSpace(model.MeasureUrl))
                {
                    var uri = new Uri(model.MeasureUrl);
                    var measureId = int.Parse(uri.Segments.Last());
                    var measure = _repo.GetMeasure(measureId);
                    entry.Measure = measure;
                    entry.FoodItem = measure.Food;
                }

                return entry;
            }
            catch
            {
                return null;
            }
        }

        public DiarySummaryModel CreateSummary(Diary diary)
        {
            return new DiarySummaryModel()
            {
                DiaryDate = diary.CurrentDate,
                TotalCalories = diary.Entries.Sum(x => x.Measure.Calories * x.Quantity)
            };
        }
    }
}