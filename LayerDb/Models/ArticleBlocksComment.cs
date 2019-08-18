namespace LayerDb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ArticleBlocksComment")]
    public partial class ArticleBlocksComment
    {
        public long Id { get; set; }

        public long ArticleBlockId { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }

        public DateTime OnCreated { get; set; }

        public DateTime OnModified { get; set; }

        [Required]
        [StringLength(128)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public virtual ArticleBlock ArticleBlock { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}
