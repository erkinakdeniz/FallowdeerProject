using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Policies;
public class RateLimeterOptions
{
    public int PermitLimit { get; set; }
    public int Window { get; set; }
    public int QueueLimit { get; set; }
    public int SegmentsPerWindow { get; set; }
    public int TokenLimit { get; set; }
    public int ReplenishmentPeriod { get; set; }
    public int TokensPerPeriod { get; set; }
    public bool AutoReplenishment { get; set; }
}

