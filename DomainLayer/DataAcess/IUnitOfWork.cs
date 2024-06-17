using DomainLayer.Data;

namespace DomainLayer.DataAcess
{
    public interface IUnitOfWork
    {
        public ApexDbContext Context { get; }
        public void Save();


    }
}
