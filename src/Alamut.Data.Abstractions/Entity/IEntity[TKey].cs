namespace Alamut.Data.Abstractions.Entity
{
    /// <summary>
    /// provide base interface of an entity
    /// it's required for work with repository 
    /// Id is mandatory
    /// the type of Id will define by type parameter
    /// </summary>
    public interface IEntity<TKey> 
    {
        TKey Id { get; set; }
    }
}
