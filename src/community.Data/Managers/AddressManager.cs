using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using community.Core.Models;

namespace community.Data.Managers
{
    public class AddressManager : Manager<Address>
    {
        public AddressManager(DbContext context) : base(context)
        {
        }

        public override async Task<List<Address>> Index()
        {
            return await Context.Set<Address>()
                .AsNoTracking()
                .Include(x => x.Events)
                .ToListAsync();
        }

        public override async Task<Address> Details(Guid? id)
        {
            if (!id.HasValue) throw new ArgumentNullException();
            return await Context.Set<Address>()
                .AsNoTracking()
                .Include(x => x.Events)
                .SingleOrDefaultAsync(x => x.Id == id.Value);
        }

        public override async Task<Address> Create(Address address)
        {
            if (address == null) throw new ArgumentNullException();

            var existing = await Context.Set<Address>().FirstOrDefaultAsync(x =>
                x.Street.Equals(address.Street, StringComparison.InvariantCultureIgnoreCase) &&
                x.City.Equals(address.City, StringComparison.InvariantCultureIgnoreCase) &&
                x.State.Equals(address.State, StringComparison.InvariantCultureIgnoreCase) &&
                x.ZipCode.Equals(address.ZipCode, StringComparison.InvariantCultureIgnoreCase));
            if (existing != null) return existing;
            Context.Set<Address>().Add(address);
            await Context.SaveChangesAsync();
            return address;
        }
    }
}
