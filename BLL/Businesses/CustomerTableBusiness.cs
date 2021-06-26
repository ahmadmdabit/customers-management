using BLL.Bases;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<CustomerTable>> GetByUser(string userCode, string ip = "")
        {
            return await (this._repository as CustomerTableRepository).GetByUser(userCode).ConfigureAwait(false);
        }
    }
}
