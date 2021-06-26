using DAL.Bases;
using DAL.DataContext;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CustomerTableRepository : BaseRepository<CustomerTable, DatabaseContext>
    {
        protected IRepository<UserTable> _UserTableRepository { get; set; }
        public CustomerTableRepository(DatabaseContext context, IRepository<UserTable> userTableRepository) : base(context)
        {
            _UserTableRepository = userTableRepository;
        }

        public async Task<List<CustomerTable>> GetByUser(string userCode)
        {
            var user = await this._UserTableRepository.Get(userCode).ConfigureAwait(false);
            if (user == null)
            {
                return new List<CustomerTable>();
            }

            if (user.UserFilter == "*")
            {
                return await this._context.Set<CustomerTable>().AsNoTracking().AsAsyncEnumerable().ToListAsync().ConfigureAwait(false);
            }
            else
            {
                var filter = new List<string>();
                user.UserFilter = user.UserFilter.Replace('*', '%');
                if (user.UserFilter.Contains(","))
                {
                    user.UserFilter.Split(",").ToList().ForEach(x => filter.Add($" CustomerCity LIKE '{x}' "));
                }
                else
                {
                    filter.Add($" CustomerCity LIKE '{user.UserFilter}'");
                }
                var sql = $"SELECT * FROM CustomerTable WHERE {string.Join(" OR ", filter)}";
                return await this.FromSqlRaw(sql).ConfigureAwait(false);
            }
        }
    }
}
