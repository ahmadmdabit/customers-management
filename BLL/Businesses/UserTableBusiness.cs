using BLL.Bases;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Businesses
{
    public class UserTableBusiness : BaseBusiness<UserTable>
    {
        public UserTableBusiness(IRepository<UserTable> repository
            //, ILogger<ColumnsBusiness> logger
            )
            : base(repository
                  //, logger
                  )
        {
        }
    }
}
