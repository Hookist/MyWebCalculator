namespace Calc.DataLayer.DBLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("Logs")]
    public partial class History
    {
        public int Id { get; set; }

        public DateTime LogTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
  
    }
}
