using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Delobytes.AspNetCore.Swagger;

internal static class FilterDescriptorExtensions
{
    public static IList<IAuthorizationRequirement> GetPolicyRequirements(this IList<FilterDescriptor> filterDescriptors)
    {
        List<IAuthorizationRequirement> policyRequirements = new List<IAuthorizationRequirement>();

        for (int i = filterDescriptors.Count - 1; i >= 0; --i)
        {
            FilterDescriptor filterDescriptor = filterDescriptors[i];
            if (filterDescriptor.Filter is AllowAnonymousFilter)
            {
                break;
            }

            if (filterDescriptor.Filter is AuthorizeFilter authorizeFilter)
            {
                if (authorizeFilter.Policy != null)
                {
                    policyRequirements.AddRange(authorizeFilter.Policy.Requirements);
                }
            }
        }

        return policyRequirements;
    }
}
