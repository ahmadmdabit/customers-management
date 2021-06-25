using DAL.Bases;
using DAL.DataContext;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class CustomerTableRepository : BaseRepository<CustomerTable, DatabaseContext>
    {
        public CustomerTableRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
