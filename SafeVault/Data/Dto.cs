using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SafeVault.Data
{
    public class Dto
    {
        [Required]
        public string Username {get; set;} = string.Empty;
        
        [Required, EmailAddress]
        public string Email {get; set;} = string.Empty;

        public Dto (string username, string email)
        {
            Username = username;
            Email = email;
        }

        // Parameterless constructor
        public Dto(){}
    }
}