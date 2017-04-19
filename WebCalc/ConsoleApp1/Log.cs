
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("Logs")]
    public partial class Log
    {
        public int Id { get; set; }

        public DateTime LogTime { get; set; }

        public int? OperationId { get; set; }

        public double Result { get; set; }

        public virtual Operation Operation { get; set; }
    }

