namespace Alamut.Data.Abstractions.Entity
{
    /// <summary>
    /// provides base Entity type with int Id as key
    /// </summary>
    public interface IEntity : IEntity<int>
    { }
}
