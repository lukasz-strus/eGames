using Domain.Games;
using Sieve.Services;

namespace Application.Core.Sieve.Extensions;

internal static class ApplicationSieveProcessorExtensions
{
    internal static void MapGameProperties(this SievePropertyMapper mapper)
    {
        mapper.Property<Game>(e => e.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<Game>(e => e.Publisher)
            .CanFilter()
            .CanSort();

        mapper.Property<Game>(e => e.Price.Amount)
            .HasName(nameof(Game.Price.Amount))
            .CanFilter()
            .CanSort();

        mapper.Property<Game>(e => e.Price.Currency)
            .HasName(nameof(Game.Price.Currency))
            .CanFilter()
            .CanSort();

        mapper.Property<Game>(e => e.IsDeleted)
            .CanFilter();

        mapper.Property<Game>(e => e.IsPublished)
            .CanFilter();
    }

    internal static void MapFullGameProperties(this SievePropertyMapper mapper)
    {
        mapper.Property<FullGame>(e => e.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<FullGame>(e => e.Publisher)
            .CanFilter()
            .CanSort();

        mapper.Property<FullGame>(e => e.Price.Amount)
            .HasName(nameof(FullGame.Price.Amount))
            .CanFilter()
            .CanSort();

        mapper.Property<FullGame>(e => e.Price.Currency)
            .HasName(nameof(FullGame.Price.Currency))
            .CanFilter()
            .CanSort();

        mapper.Property<FullGame>(e => e.IsDeleted)
            .CanFilter();

        mapper.Property<FullGame>(e => e.IsPublished)
            .CanFilter();
    }

    internal static void MapDlcGameProperties(this SievePropertyMapper mapper)
    {
        mapper.Property<DlcGame>(e => e.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<DlcGame>(e => e.Publisher)
            .CanFilter()
            .CanSort();

        mapper.Property<DlcGame>(e => e.Price.Amount)
            .HasName(nameof(DlcGame.Price.Amount))
            .CanFilter()
            .CanSort();

        mapper.Property<DlcGame>(e => e.Price.Currency)
            .HasName(nameof(DlcGame.Price.Currency))
            .CanFilter()
            .CanSort();

        mapper.Property<DlcGame>(e => e.IsDeleted)
            .CanFilter();

        mapper.Property<DlcGame>(e => e.IsPublished)
            .CanFilter();
    }

    internal static void MapSubscriptionProperties(this SievePropertyMapper mapper)
    {
        mapper.Property<Subscription>(e => e.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<Subscription>(e => e.Publisher)
            .CanFilter()
            .CanSort();

        mapper.Property<Subscription>(e => e.Price.Amount)
            .HasName(nameof(Subscription.Price.Amount))
            .CanFilter()
            .CanSort();

        mapper.Property<Subscription>(e => e.Price.Currency)
            .HasName(nameof(Subscription.Price.Currency))
            .CanFilter()
            .CanSort();

        mapper.Property<Subscription>(e => e.IsDeleted)
            .CanFilter();

        mapper.Property<Subscription>(e => e.IsPublished)
            .CanFilter();
    }
}