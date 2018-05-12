﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using community.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace community.Data.Managers
{
    public abstract class Manager<T> : IManager<T> where T : class
    {
        protected readonly DbContext Context;

        protected Manager(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<List<T>> Index()
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> Details(Guid? id)
        {
            if (!id.HasValue) throw new ArgumentNullException();
            return await Context.Set<T>().FindAsync(id.Value);
        }

        public virtual async Task<T> Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException();
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task Edit(T entity)
        {
            if (entity == null) throw new ArgumentNullException();
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid? id)
        {
            if (!id.HasValue) throw new ArgumentNullException();
            var entity = await Context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                Context.Set<T>().Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}
