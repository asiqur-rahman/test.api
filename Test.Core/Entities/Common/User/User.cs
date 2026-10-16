using System;

namespace Test.Core.Entities.Common.User
{
    public class User
    {
        // Use int Id to match other entities' foreign key types (EntryBy, UpdateBy...)
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        // Store password hash (do not store plain text password)
        public string PasswordHash { get; set; } = string.Empty;

        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginAt { get; set; }
    }
}
