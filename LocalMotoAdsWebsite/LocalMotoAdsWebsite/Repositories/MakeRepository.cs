using LocalMotoAdsWebsite.Data;
using LocalMotoAdsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMotoAdsWebsite.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        private readonly AppDbContext appDbContext;

        public MakeRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public List<Make> GetAllMakes()
        {
            return appDbContext.Makes.ToList();
        }

        public Make GetMake(int id)
        {
            return appDbContext.Makes.Where(x => x.Id.Equals(id)).FirstOrDefault();
        }
    }
}
