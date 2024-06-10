using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Policies;
public static class PolicyNames
{
    public const string AuthFixedRateLimitConfiguration = "RateLimiterOptions:AuthFixedRateLimit";
    public const string GeneralFixedRateLimitConfiguration = "RateLimiterOptions:GeneralFixedRateLimit";
    public const string AuthFixedPolicyName = "Auth";
    public const string GeneralFixedPolicyName = "General";
}
