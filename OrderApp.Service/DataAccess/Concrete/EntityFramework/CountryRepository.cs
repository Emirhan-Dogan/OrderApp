
using System;
using System.Linq;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using DataAccess.Concrete.EntityFramework.Contexts;
using DataAccess.Abstract;
namespace DataAccess.Concrete.EntityFramework
{
    public class CountryRepository : EfEntityRepositoryBase<Country, ProjectDbContext>, ICountryRepository
    {
        public CountryRepository(ProjectDbContext context) : base(context)
        {
        }
    }
}
