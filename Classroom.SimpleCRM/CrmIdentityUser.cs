using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Classroom.SimpleCRM
{
    public class CrmIdentityUser : IdentityUser<Guid>
    {
        [MaxLength(120)]
        public string Name { get; set; }
    }
}
