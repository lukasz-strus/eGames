using Application.Core.Sieve.Extensions;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Application.Core.Sieve;

public class ApplicationSieveProcessor(IOptions<SieveOptions> options) : SieveProcessor(options)
{
    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        mapper.MapGameProperties();
        mapper.MapFullGameProperties();
        mapper.MapDlcGameProperties();
        mapper.MapSubscriptionProperties();

        return mapper;
    }
}