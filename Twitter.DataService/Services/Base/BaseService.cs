using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Twitter.Core.Extentions;
using Twitter.Model.Entities;
using Twitter.Service.Data;

namespace Twitter.Service.Services.Base
{
    public abstract class BaseService<TEntity> : IBaseService<TEntity>
        where TEntity : class, IBase
    {

        protected readonly DbContextOptions<TwitterDbContext> _twitterDbContextOption;
        protected  readonly ILogger<BaseService<TEntity>> _logger;

        protected readonly TwitterDbContext _twitterDbContext;

        protected BaseService(DbContextOptions<TwitterDbContext> twitterDbContextOption, [NotNull] ILogger<BaseService<TEntity>> logger)
        {
            _twitterDbContextOption = twitterDbContextOption;
            _logger = logger;
            _twitterDbContext  = new TwitterDbContext(_twitterDbContextOption);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", string.Format("CreateAsync<{0}>", typeof(TEntity).Name));

            try
            {
                entity.Created = DateTimeOffset.Now;
                await _twitterDbContext.AddAsync(entity);
                await _twitterDbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

        
        public async Task<TEntity> UpdateAsync(int id, TEntity updateEntity)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", string.Format("UpdateAsync<{0}>", typeof(TEntity).Name));
            parameters.Add("Id", id);

            try
            {
                
                var entity = await GetAsync(id); 

                if (entity == null)
                {
                    return null; 
                }

                _twitterDbContext.Entry(entity).CurrentValues.SetValues(updateEntity);
                _twitterDbContext.Entry(entity).State = EntityState.Modified; 

                if (_twitterDbContext.Entry(entity).Properties.Any(property => property.IsModified))
                {
                   
                    entity.Updated = DateTimeOffset.Now;
                    await _twitterDbContext.SaveChangesAsync();
                }

                return entity; 
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

        protected async Task<TEntity> GetAsync(int id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", string.Format("GetAsync<{0}>", typeof(TEntity).Name));
            parameters.Add("Id", id);

            try
            {
                return await _twitterDbContext.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", string.Format("DeleteAsync<{0}>", typeof(TEntity).Name));
            parameters.Add("Id", id);

            try
            {
                var entity = await GetAsync(id); 

                if (entity == null)
                {
                    return;
                }


                _twitterDbContext.Entry(entity).State = EntityState.Deleted; 
                await _twitterDbContext.SaveChangesAsync(); 
            }
            catch (Exception exception)
            {
                _logger.LogWithParameters(LogLevel.Error, exception, "Unable to complete method due to an exception", parameters);
                throw;
            }
        }

        
    }
}

