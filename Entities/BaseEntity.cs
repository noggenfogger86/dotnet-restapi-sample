using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSkeleton.Entities
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedDate { get; set; }
    }
}