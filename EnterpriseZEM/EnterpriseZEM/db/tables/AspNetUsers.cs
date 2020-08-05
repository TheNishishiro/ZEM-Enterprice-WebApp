using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM.db.tables
{
    public class AspNetUsers
    {
        [Key]
        [Column(TypeName = "nvarchar(450)")]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string UserName { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string NormalizedUserName { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(256)")]
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
    }
}
