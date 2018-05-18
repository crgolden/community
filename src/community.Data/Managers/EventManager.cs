using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using community.Core.Models;

namespace community.Data.Managers
{
    public class EventManager : Manager<Event>
    {
        public EventManager(DbContext context) : base(context)
        {
        }

        public override async Task<List<Event>> Index()
        {
            return await Context.Set<Event>()
                .AsNoTracking()
                .Include(x => x.Address)
                .Include(x => x.User)
                .Include(x => x.Attenders)
                .Include(x => x.Followers)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public override async Task<Event> Details(Guid? id)
        {
            if (!id.HasValue) throw new ArgumentNullException();

            return await Context.Set<Event>()
                .AsNoTracking()
                .Include(x => x.Address)
                .Include(x => x.User)
                .Include(x => x.Attenders)
                .Include(x => x.Followers)
                .SingleOrDefaultAsync(x => x.Id == id.Value);
        }
    }
}
