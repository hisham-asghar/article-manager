namespace LayerDb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserTag
    {
        public long Id { get; set; }

        [Required]
        [StringLength(256)]
        public string UserId { get; set; }

        public long TagId { get; set; }
    }
}
