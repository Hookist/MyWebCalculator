using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.DataLayer.DBLayer;
using Calc.Repository.Repositories;
using System.Data.Entity;

namespace Calc.Service.Services
{
    public class HistoryRepository : GenericRepository<History>
    {
        public HistoryRepository(DbContext context) : base(context) { }
    }
}
