using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Core.Security.Entities;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.MainRepositories;
public class SliderRepository : EfRepositoryBase<Slider, int, BaseDbContext>, ISliderRepository
{
    public SliderRepository(BaseDbContext context) : base(context)
    {
    }
}
