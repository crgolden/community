using System;
using System.Threading.Tasks;
using community.Data;
using community.Models;
using Microsoft.EntityFrameworkCore;

namespace community.Extensions
{
    public static class ViewModelExtensions
    {
        public static async Task<Address> ExisitingMatch(this AddressViewModel model, ApplicationDbContext context)
        {
            return await context.Addresses.FirstOrDefaultAsync(x => x.Street.Equals(model.Street, StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.City.Equals(model.City, StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.State.Equals(model.State, StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.ZipCode.Equals(model.ZipCode, StringComparison.InvariantCultureIgnoreCase));
        }

        public static async Task<Address> ExisitingMatch(this EventViewModel model, ApplicationDbContext context)
        {
            return await context.Addresses.FirstOrDefaultAsync(x => x.Street.Equals(model.Street, StringComparison.InvariantCultureIgnoreCase) &&
                                                                    x.City.Equals(model.City, StringComparison.InvariantCultureIgnoreCase) &&
                                                                    x.State.Equals(model.State, StringComparison.InvariantCultureIgnoreCase) &&
                                                                    x.ZipCode.Equals(model.ZipCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
