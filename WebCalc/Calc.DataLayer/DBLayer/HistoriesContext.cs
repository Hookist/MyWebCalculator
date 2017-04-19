namespace Calc.DataLayer.DBLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HistoriesContext : DbContext
    {
        public HistoriesContext()
            : base("name=HistoriesContext")
        {
        }

        public virtual DbSet<History> histories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
