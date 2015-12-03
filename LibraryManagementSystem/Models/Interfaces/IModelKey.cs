namespace LibraryManagementSystem.Models.Interfaces
{
    /// <summary>
    /// Requires all classes that implement this interface to use "Id" as the primary key / identifier.
    /// </summary>
    public interface IModelKey
    {
        int Id { get; set; }
    }
}
