namespace ConsoleApp1
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CalcContext : DbContext
    {
        public CalcContext()
            : base("name=CalcContext1")
        {
        }

        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}