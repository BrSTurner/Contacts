namespace FIAP.Contacts.SharedKernel.UoW
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
