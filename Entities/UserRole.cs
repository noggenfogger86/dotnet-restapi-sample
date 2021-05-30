using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiSkeleton.Entities
{
    [Table(name: "UserRoles")]
    public class UserRole : BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        [JsonIgnore]
        public virtual long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Key]
        [Column(Order = 2)]
        [JsonIgnore]
        public virtual int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}