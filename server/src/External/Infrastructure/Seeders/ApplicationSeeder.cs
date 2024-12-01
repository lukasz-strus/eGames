using Application.Authentication;
using Domain.Enums;
using Domain.Games;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders;

public class ApplicationSeeder(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
{
    public async Task Seed()
    {
        if (!await dbContext.Database.CanConnectAsync()) return;

        await MigratePendingChanges();

        await SeedUsers();
        await SeedGame();
    }

    private async Task MigratePendingChanges()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }

    #region User

    private async Task SeedUsers()
    {
        if (await dbContext.DomainUsers.AnyAsync() && await dbContext.Users.AnyAsync()) return;

        var roles = GetRoles();
        await SeedRoles(roles);

        var superAdmin = await SeedUser("Super admin", roles);
        var superAdminIdentityUser = GetSuperAdminIdentityUser();
        await SeedIdentityUser(superAdminIdentityUser, superAdmin);

        var admin = await SeedUser("Admin", roles.Where(x => !x.Equals(UserRole.SuperAdmin)));
        var adminIdentityUser = GetAdminIdentityUser();
        await SeedIdentityUser(adminIdentityUser, admin);

        var customer = await SeedUser("Customer", [UserRole.Customer]);
        var customerIdentityUser = GetCustomerIdentityUser();
        await SeedIdentityUser(customerIdentityUser, customer);
    }

    private static List<UserRole> GetRoles() =>
    [
        UserRole.Customer,
        UserRole.Admin,
        UserRole.SuperAdmin
    ];

    private async Task<User> SeedUser(string userName, IEnumerable<UserRole> roles)
    {
        var domainUser = User.Create(userName, roles);
        await dbContext.DomainUsers.AddAsync(domainUser);
        await dbContext.SaveChangesAsync();
        return domainUser;
    }

    private async Task SeedIdentityUser(ApplicationUser applicationUser, User user)
    {
        if (applicationUser.Email is null) return;

        applicationUser.UserId = user.Id;

        var userStore = new UserStore<ApplicationUser>(dbContext);
        if (!dbContext.Users.Any(u => u.UserId == applicationUser.UserId))
        {
            await userStore.CreateAsync(applicationUser);
        }

        var roles = user.Roles.Select(x => x.Name).ToArray();

        await AssignRoles(applicationUser.Email, roles);

        await dbContext.SaveChangesAsync();
    }

    private static ApplicationUser GetSuperAdminIdentityUser()
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "superadmin@test.com",
            NormalizedUserName = "SUPERADMIN@TEST.COM",
            Email = "superadmin@test.com",
            NormalizedEmail = "SUPERADMIN@TEST.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "Super-admin123!");

        return applicationUser;
    }

    private static ApplicationUser GetAdminIdentityUser()
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "admin@test.com",
            NormalizedUserName = "ADMIN@TEST.COM",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PhoneNumber = "+222222222222",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "Admin123!");

        return applicationUser;
    }

    private static ApplicationUser GetCustomerIdentityUser()
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "customer@test.com",
            NormalizedUserName = "CUSTOMER@TEST.COM",
            Email = "customer@test.com",
            NormalizedEmail = "CUSTOMER@TEST.COM",
            PhoneNumber = "+333333333333",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "Customer123!");

        return applicationUser;
    }

    private async Task SeedRoles(IEnumerable<UserRole> roles)
    {
        var roleStore = new RoleStore<IdentityRole>(dbContext);
        foreach (var role in roles)
        {
            if (!dbContext.Roles.Any(r => r.Name == role.Name))
                await roleStore.CreateAsync(new IdentityRole(role.Name)
                {
                    NormalizedName = role.Name.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D")
                });
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task AssignRoles(string email, string[] roles)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null) await userManager.AddToRolesAsync(user, roles);
    }

    #endregion

    #region Game

    private async Task SeedGame()
    {
        var fullGameId = await SeedFullGame() ??
                         dbContext.FullGames
                             .Where(x => x.Name.Contains("Crusader"))
                             .Select(x => x.Id)
                             .First();

        await SeedDlcGames(fullGameId);

        await SeedSubscriptions();
    }

    private async Task SeedSubscriptions()
    {
        if (dbContext.Subscriptions.Any()) return;

        var subscription = Subscription.Create(
            "World of Warcraft",
            "World of Warcraft is a massively multiplayer online role-playing game released in 2004 by Blizzard Entertainment. It is the fourth released game set in the Warcraft fantasy universe.",
            14.99m,
            Currency.Usd,
            new DateTime(2004, 11, 23),
            "Blizzard Entertainment",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            5000000000,
            "https://www.gry-online.pl/galeria/gry13/177379296.jpg",
            30);

        subscription.Publish();

        await dbContext.Subscriptions.AddAsync(subscription);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedDlcGames(GameId fullGameId)
    {
        if (dbContext.DlcGames.Any()) return;

        var dlcGame1 = DlcGame.Create(
            "Crusader Kings III: Royal Court",
            "The Royal Court expansion is a major addition to Crusader Kings III, offering new ways to maintain and grow your royal court as well as a host of new events, edicts, and interactions to make your royal role truly majestic.",
            79.99m,
            Currency.Pln,
            new DateTime(2022, 6, 28),
            "Paradox Interactive",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            2000000000,
            "https://encrypted-tbn2.gstatic.com/shopping?q=tbn:ANd9GcT6EMcNqVRehicYLHSol7bbCV5E6WVf0oek1nuaKoslMVxGEA8ThPBofCOkzIu3wxszdoWj2wOENf_uS4BsqO_dYkQ-D6SfU41CyRET84KF",
            fullGameId);

        dlcGame1.Publish();

        await dbContext.DlcGames.AddAsync(dlcGame1);

        var dlcGame2 = DlcGame.Create(
            "Crusader Kings III: Tours & Tournaments",
            "The Tours & Tournaments expansion for Crusader Kings III opens up new ways for you to experience the medieval grand strategy game. With new systems for grand tours and the jousting lists, you can now live out your knightly fantasies in the game.",
            39.99m,
            Currency.Pln,
            new DateTime(2021, 3, 16),
            "Paradox Interactive",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            1000000000,
            "https://image.api.playstation.com/vulcan/ap/rnd/202410/2815/b9f49a377646b37d7c17610a0c1246e60572536bce9fc406.png",
            fullGameId);

        dlcGame2.Publish();

        await dbContext.DlcGames.AddAsync(dlcGame2);

        await dbContext.SaveChangesAsync();
    }

    private async Task<GameId?> SeedFullGame()
    {
        if (dbContext.FullGames.Any()) return null;

        var fullGame = FullGame.Create(
            "Crusader Kings III",
            "Love, fight, scheme, and claim greatness. Determine your noble house’s legacy in the sprawling grand strategy of Crusader Kings III. Death is only the beginning as you guide your dynasty’s bloodline in the rich and larger-than-life simulation of the Middle Ages.",
            231.99m,
            Currency.Pln,
            new DateTime(2020, 8, 1),
            "Paradox Interactive",
            "https://eGames.com/b52c0f63-bb5e-4df7-afe3-667785e63511",
            8000000000,
            "https://image.api.playstation.com/vulcan/ap/rnd/202108/1607/czGau6hOvx9iQYOOkACdxqDl.png",
            []);

        fullGame.Publish();

        await dbContext.FullGames.AddAsync(fullGame);
        await dbContext.SaveChangesAsync();

        await SeedAdditionalFullGames();

        return fullGame.Id;
    }

    private async Task SeedAdditionalFullGames()
    {
        var additionalGames = new List<FullGame>();

        var gothic = FullGame.Create(
            "Gothic",
            "War has been waged across the kingdom of Myrtana. Orcish hordes invaded human territory and the king of the land needed a lot of ore to forge enough weapons, should his army stand against this threat. Whoever breaks the law in these darkest of times is sentenced to serve in the giant penal colony of Khorinis, mining the so much needed ore.\r\n\r\nThe whole area, dubbed \"the Colony\", is surrounded by a magical barrier, a sphere two kilometers diameter, sealing off the penal colony from the outside world. The barrier can be passed from the outside in – but once inside, nobody can escape. The barrier was a double-edged sword - soon the prisoners took the opportunity and started a revolt. The Colony became divided into three rivaling factions and the king was forced to negotiate for his ore, not just demand it.\r\n\r\nYou are thrown through the barrier into this prison. With your back against the wall, you have to survive and form volatile alliances until you can finally escape.",
            19.99m,
            Currency.Pln,
            new DateTime(2001, 3, 15),
            "Piranha Bytes",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            10000000000,
            "https://static.wikia.nocookie.net/gothic/images/d/d4/Gothic_1_box.png/revision/latest?cb=20230928155129&path-prefix=pl",
            []);

        gothic.Publish();
        additionalGames.Add(gothic);

        var gothic2 = FullGame.Create(
            "Gothic II: Gold Edition",
            "Gothic II: Gold Edition brings together the excitement of Gothic II and the add-on Night of the Raven to your fingertips!\r\n\r\nYou have torn down the magical barrier and released the prisoners of the Mine Valley. Now the former criminals of the forests and mountains are causing trouble around the capital of Khorinis. The town militia is powerless due to their low amount of force–outside of the town, everyone is helpless against the attacks of the bandits.",
            29.99m,
            Currency.Pln,
            new DateTime(2002, 11, 29),
            "Piranha Bytes",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            20000000000,
            "https://static.wikia.nocookie.net/gothic/images/8/82/Gothic_II_-_ok%C5%82adka.jpg/revision/latest?cb=20150822140040&path-prefix=pl",
            []);

        gothic2.Publish();
        additionalGames.Add(gothic2);

        var civilizationV = FullGame.Create(
            "Sid Meier's Civilization V",
            "The Flagship Turn-Based Strategy Game Returns\r\n\r\nBecome Ruler of the World by establishing and leading a civilization from the dawn of man into the space age: Wage war, conduct diplomacy, discover new technologies, go head-to-head with some of history’s greatest leaders and build the most powerful empire the world has ever known.",
            39.99m,
            Currency.Pln,
            new DateTime(2010, 9, 21),
            "2K",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            30000000000,
            "https://ecsmedia.pl/c/sid-meier-s-civilization-5-bogowie-i-krolowie-pc-b-iext155918678.jpg",
            []);

        civilizationV.Publish();
        additionalGames.Add(civilizationV);

        var civilizationVi = FullGame.Create(
            "Sid Meier's Civilization VI",
            "Civilization VI is the newest installment in the award winning Civilization Franchise. Expand your empire, advance your culture and go head-to-head against history’s greatest leaders. Will your civilization stand the test of time?",
            59.99m,
            Currency.Pln,
            new DateTime(2016, 10, 21),
            "2K",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            40000000000,
            "https://static.muve.pl/uploads/product-cover/0028/4364/2kgmkt-civ6-poland-ag-fob-pegi.jpg",
            []);

        civilizationVi.Publish();
        additionalGames.Add(civilizationVi);

        var callOfDuty = FullGame.Create(
            "Call of Duty\u00ae",
            "The Call of Duty\u00ae experience supports Call of Duty\u00ae: Black Ops 6, Call of Duty\u00ae: Modern Warfare\u00ae III, Call of Duty\u00ae: Modern Warfare\u00ae II and Call of Duty\u00ae: Warzone\u2122.\r\n\r\nCall of Duty\u00ae: Black Ops 6 is signature Black Ops across a cinematic single-player Campaign, a best-in-class Multiplayer experience and with the epic return of Round-Based Zombies.\r\n\r\nCall of Duty\u00ae: Modern Warfare\u00ae III is the direct sequel to the record-breaking Call of Duty\u00ae: Modern Warfare\u00ae II. Captain Price and Task Force 141 face off against the ultimate threat. The ultranationalist war criminal Vladimir Makarov is extending his grasp across the world causing Task Force 141 to fight like never before.\r\n\r\nCall of Duty\u00ae: Modern Warfare\u00ae II drops players into an unprecedented global conflict that features the return of the iconic Operators of Task Force 141.\r\n\r\nCall of Duty\u00ae: Warzone\u2122 is the massive free-to-play combat arena, featuring Battle Royale and Resurgence.\r\n\r\nCall of Duty\u00ae Points (CP) are the in-game currency that can be used in Black Ops 6, Call of Duty\u00ae: Modern Warfare\u00ae III, Modern Warfare\u00ae II, and Warzone\u2122 to obtain new content. Call of Duty\u00ae: Black Ops 6, Call of Duty\u00ae: Modern Warfare\u00ae III, Call of Duty\u00ae: Modern Warfare\u00ae II or Call of Duty\u00ae: Warzone\u2122 game required, sold / downloaded separately.\r\n\r\nCP purchased may also be used to obtain in-game content in certain Call of Duty\u00ae games with CP functionality enabled*. Each game sold separately.",
            349.99m,
            Currency.Pln,
            new DateTime(2024, 10, 21),
            "Activision",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            60000000000,
            "https://sm.ign.com/ign_ap/cover/c/call-of-du/call-of-duty-2024_u1dt.jpg",
            []);

        callOfDuty.Publish();
        additionalGames.Add(callOfDuty);

        var cyberpunk2077 = FullGame.Create(
            "Cyberpunk 2077",
            "Cyberpunk 2077 is an open-world, action-adventure story set in Night City, a megalopolis obsessed with power, glamour and body modification. You play as V, a mercenary outlaw going after a one-of-a-kind implant that is the key to immortality. You can customize your character’s cyberware, skillset and playstyle, and explore a vast city where the choices you make shape the story and the world around you.",
            199.99m,
            Currency.Pln,
            new DateTime(2020, 12, 10),
            "CD PROJEKT RED",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            70000000000,
            "https://image.api.playstation.com/vulcan/ap/rnd/202008/0416/6Bo40lnWU0BhgrOUm7Cb6by3.png",
            []);

        cyberpunk2077.Publish();
        additionalGames.Add(cyberpunk2077);

        var theWitcher3 = FullGame.Create(
            "The Witcher 3: Wild Hunt",
            "The Witcher 3: Wild Hunt is a story-driven, next-generation open world role-playing game set in a visually stunning fantasy universe full of meaningful choices and impactful consequences. In The Witcher, you play as professional monster hunter Geralt of Rivia tasked with finding a child of prophecy in a vast open world rich with merchant cities, pirate islands, dangerous mountain passes, and forgotten caverns to explore.",
            99.99m,
            Currency.Pln,
            new DateTime(2015, 5, 19),
            "CD PROJEKT RED",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            90000000000,
            "https://image.api.playstation.com/vulcan/ap/rnd/202211/0711/qezXTVn1ExqBjVjR5Ipm97IK.png",
            []);

        theWitcher3.Publish();
        additionalGames.Add(theWitcher3);

        await dbContext.FullGames.AddRangeAsync(additionalGames);
        await dbContext.SaveChangesAsync();
    }

    #endregion
}