
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new CalcContext())
            {
                var operation = new Operation() { Name = "Addition" };
                db.Operations.Add(operation);
                db.SaveChanges();
            }
        }
    }
}
