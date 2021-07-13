using LocalMotoAdsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalMotoAdsWebsite.Repositories
{
    public interface IMakeRepository
    {
        List<Make> GetAllMakes();
        Make GetMake(int id);
    }
}
