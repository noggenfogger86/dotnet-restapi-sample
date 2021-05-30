using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiSkeleton.Entities
{
    [Table(name: "Users")]
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public bool IsActive { get; set; }

        // many to many
        public ICollection<Role> Roles { get; set; }
        public List<UserRole> UserRoles { get; set; }

    }
}