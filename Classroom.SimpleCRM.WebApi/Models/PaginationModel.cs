using System;
using System.Collections.Generic;
namespace Classroom.SimpleCRM.WebApi.Models
{
    public class PaginationModel
    {
        public string Previous { get; set; }
        public string Next { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
        
    }
}