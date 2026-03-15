using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeVault.Data
{
    // record type to represent login data
    public class UserAuthRecord
    {
        public string Username {get; set;} = string.Empty;
        public string PasswordHash {get; set;} = string.Empty;
        public string Role {get; set;} = string.Empty;

    public UserAuthRecord(string username, string passwordHash, string role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }

    }
}