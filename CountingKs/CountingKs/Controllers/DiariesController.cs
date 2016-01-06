using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Models;
using System.Threading;
using CountingKs.Services;


namespace CountingKs.Controllers
{
    public class DiariesController : BaseController
    {
        private ICountingKsIdentityService _identityService;
        public DiariesController(ICountingKsRepository repo, ICountingKsIdentityService identityService)
            : base(repo)
        {
            _identityService = identityService;
        }

        public object Get()
        {
            //var userName = Thread.CurrentPrincipal.Identity.Name;
            var userName = _identityService.CurrentUser;
            var result = TheRepository.GetDiaries(userName)
                .OrderByDescending(x => x.CurrentDate)
                .Take(10)
                .ToList()
                .Select(x => TheModelFactory.Create(x));
            return result;
        }

        public HttpResponseMessage Get(DateTime diaryId)
        {
            //var userName = Thread.CurrentPrincipal.Identity.Name;
            var userName = _identityService.CurrentUser;
            var result = TheRepository.GetDiary(userName, diaryId);

            if(result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(result));
        }

    }
}
