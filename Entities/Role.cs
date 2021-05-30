using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSkeleton.Entities
{
    [Table(name: "Roles")]
    public class Role : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}