using System;
using System.Threading.Tasks;
using community.Api.v1.ViewModels;
using community.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace community.Api.v1.Extensions
{
    public static class AddressViewModelExtensions
    {
        public static async Task<Address> ExisitingMatch(this AddressViewModel model, DbContext context)
        {
            return await context.Set<Address>().FirstOrDefaultAsync(x =>
                x.Street.Equals(model.Street, StringComparison.InvariantCultureIgnoreCase) &&
                x.City.Equals(model.City, StringComparison.InvariantCultureIgnoreCase) &&
                x.State.Equals(model.State, StringComparison.InvariantCultureIgnoreCase) &&
                x.ZipCode.Equals(model.ZipCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
