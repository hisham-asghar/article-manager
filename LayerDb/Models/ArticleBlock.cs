namespace LayerDb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ArticleBlock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArticleBlock()
        {
            ArticleBlocksComments = new HashSet<ArticleBlocksComment>();
        }

        public long Id { get; set; }

        public long ArticleId { get; set; }

        public long BlockTypeId { get; set; }

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

        public virtual BlockType BlockType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticleBlocksComment> ArticleBlocksComments { get; set; }
    }
}
