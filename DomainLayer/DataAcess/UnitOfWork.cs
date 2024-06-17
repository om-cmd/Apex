using DomainLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DataAcess
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ApexDbContext dbContext) 
        {
            Context = dbContext;
        }

        public ApexDbContext Context { get; private set; }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
