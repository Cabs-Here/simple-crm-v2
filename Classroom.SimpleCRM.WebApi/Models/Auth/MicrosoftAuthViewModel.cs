﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classroom.SimpleCRM.WebApi.Models.Auth
{
    public class MicrosoftAuthViewModel
    {
        public string AccessToken { get; set; }
        public string State { get; set; }
        public string BaseHref { get; set; }
    }
}