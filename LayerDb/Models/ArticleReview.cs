namespace LayerDb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ArticleReview
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [StringLength(128)]
        public string ReviewerId { get; set; }

        public long ArticleId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime OnCreated { get; set; }

        public DateTime OnModified { get; set; }

        [Required]
        [StringLength(128)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public virtual Article Article { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
