﻿using CountingKs.Data;
using CountingKs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CountingKs.Controllers
{
    public abstract class BaseController : ApiController
    {
        private readonly ICountingKsRepository _repo;
        private ModelFactory _modelFactory;

        public BaseController(ICountingKsRepository repo)
        {
            _repo = repo;
            
        }

        protected ICountingKsRepository TheRepository
        {
            get
            {
                return _repo;
            }
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if(_modelFactory ==null)
                {
                    _modelFactory = new ModelFactory(this.Request);
                }
                return _modelFactory;
            }
        }
    }
}