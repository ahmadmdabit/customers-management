using BLL.Bases;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Businesses
{
    public class CustomerTableBusiness : BaseBusiness<CustomerTable>
    {
        public CustomerTableBusiness(IRepository<CustomerTable> repository
            //, ILogger<ColumnsBusiness> logger
            )
            : base(repository
                  //, logger
                  )
        {
        }
    }
}
