using System.Threading.Tasks;

namespace DAL.Interfaces
{
    /// <summary>
    /// Specifies the contract for RepositoryWrapper
    /// </summary>
    public interface IRepositoryWrapper
    {
        /// <summary>
        /// Saves the async.
        /// </summary>
        /// <returns>A Task.</returns>
        Task SaveAsync();
    }
}