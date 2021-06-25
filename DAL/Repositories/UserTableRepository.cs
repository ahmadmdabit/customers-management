using DAL.Bases;
using DAL.DataContext;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class UserTableRepository : BaseRepository<UserTable, DatabaseContext>
    {
        public UserTableRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
