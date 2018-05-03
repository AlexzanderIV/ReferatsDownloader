using Microsoft.EntityFrameworkCore;
using ReferatsDownloader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReferatsDownloader.DAL
{
    public class ReferatsRepository: IDisposable
    {
        private ReferatsContext dbContext;

        public ReferatsRepository()
        {
            dbContext = new ReferatsContext();
        }

        async public Task<List<Referat>> GetAllReferats()
        {    
            return await GetEntities<Referat>().Include("Category").ToListAsync();
        }

        async public Task<List<Category>> GetAllCategories()
        {
            return await GetEntities<Category>().Include("Referats").ToListAsync();
        }

        private DbSet<T> GetEntities<T>() where T: DatabaseEntity
        {
            return dbContext.Set<T>();
        }

        async public Task<int> Insert<T>(T entity) where T : DatabaseEntity
        {
            entity.CreatedDate = DateTime.UtcNow;
            dbContext.Add(entity);
            var res = await dbContext.SaveChangesAsync();
            return res;
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
