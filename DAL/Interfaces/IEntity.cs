namespace DAL.Interfaces
{
    /// <summary>
    /// Specifies the contract for Entity
    /// </summary>
    public interface IEntity
    {
		public object GetKeyProperty();
		public object GetParentKeyProperty();
    }
}