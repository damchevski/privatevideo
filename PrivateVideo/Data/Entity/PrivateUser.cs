using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateVideo.Data.Entity
{
    [Table("PrivateUsers")]
    public class PrivateUser : IdentityUser
    {
        public bool HasPaid { get; set; }
    }
}
