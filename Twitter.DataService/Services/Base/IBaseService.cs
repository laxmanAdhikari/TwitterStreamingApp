using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Model.Entities;

namespace Twitter.Service.Services.Base
{
    public interface IBaseService<TEntity> where TEntity : class, IBase
    { 
        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(int id, TEntity entity);

        Task DeleteAsync(int id);
    }
}
