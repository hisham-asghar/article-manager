namespace LayerDb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ArticleManagerView")]
    public partial class ArticleManagerView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string SubTitle { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime OnCreated { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime OnModified { get; set; }

        [Key]
        [Column(Order = 4)]
        public string CreatedBy { get; set; }

        [Key]
        [Column(Order = 5)]
        public string ModifiedBy { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool IsReadyToPublish { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool IsReadyToReview { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool IsDeleted { get; set; }

        [StringLength(256)]
        public string Guid { get; set; }

        public int? ReviewCount { get; set; }

        [StringLength(1024)]
        public string CreatedByName { get; set; }

        [StringLength(256)]
        public string CreatedByEmail { get; set; }

        public long? ArticleId { get; set; }

        public long? TagId { get; set; }
    }
}
