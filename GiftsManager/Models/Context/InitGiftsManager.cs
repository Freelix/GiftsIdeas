using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GiftsManager.Models.Context
{
    public class InitGiftsManager : DropCreateDatabaseAlways<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            TestSeed(context);
        }

        private void TestSeed(DataBaseContext context)
        {
            User user = new User()
            {
                Id = 1,
                FirstName = "Foo",
                LastName = "Bar",
                Email = "foobar@gmail.com",
                Password = "E2-F3-48-0C-BB-23-31-D8-46-FE-D1-F9-5B-F5-EE-F1"
            };

            User otherUser = new User()
            {
                Id = 2,
                FirstName = "Bar",
                LastName = "Foo",
                Email = "barfoo@gmail.com",
                Password = "E2-F3-48-0C-BB-23-31-D8-46-FE-D1-F9-5B-F5-EE-F1"
            };

            Group group = new Group()
            {
                Id = 1,
                Name = "Party Mix",
                GroupAdmin = "foobar@gmail.com"
            };

            Event event1 = new Event()
            {
                Id = 1,
                Name = "Event 1",
                Group = group
            };

            Event event2 = new Event()
            {
                Id = 2,
                Name = "Event 2",
                Group = group
            };

            group.Events.Add(event1);
            group.Events.Add(event2);

            group.Users.Add(user);
            group.Users.Add(otherUser);

            var wishList = new List<Gift>()
            {
                new Gift { Id = 1, Name = "Xbox ONE", Status = Gift.StatusOption.Open, User = user, Event = event1},
                new Gift { Id = 2, Name = "Xbox 360", Status = Gift.StatusOption.Open, User = user, Event = event1},
                new Gift { Id = 3, Name = "PS4", Status = Gift.StatusOption.Open, User = user, Event = event2},
                new Gift { Id = 4, Name = "PS3", Status = Gift.StatusOption.Open, User = user, Event = event2}
            };

            var wishList2 = new List<Gift>()
            {
                new Gift { Id = 5, Name = "Xbox ONE", Status = Gift.StatusOption.Reserved, Buyer = user, User = otherUser, Event = event2},
                new Gift { Id = 6, Name = "Xbox 360", Status = Gift.StatusOption.Reserved, Buyer = user, User = otherUser, Event = event2},
                new Gift { Id = 7, Name = "Gift 1", Status = Gift.StatusOption.Open, User = otherUser, Price = 14.99, Event = event2},
                new Gift { Id = 8, Name = "Gift 34", Status = Gift.StatusOption.Open, User = otherUser, Event = event1},
                new Gift { Id = 9, Name = "Fallout", Status = Gift.StatusOption.Bought, Buyer = user, User = otherUser, Price = 24.99, Event = event1, IsBought = true},
                new Gift { Id = 10, Name = "Far cry", Status = Gift.StatusOption.Bought, Buyer = user, User = otherUser, Price = 29.99, Event = event1, IsBought = true},
                new Gift { Id = 11, Name = "PS3", Status = Gift.StatusOption.Open, User = otherUser, Event = event1}
            };

            var reservedGifts = new List<Gift>
            {
                wishList2.FirstOrDefault(x => x.Id == 5),
                wishList2.FirstOrDefault(x => x.Id == 6)
            };

            var boughtGifts = new List<Gift>
            {
                wishList2.FirstOrDefault(x => x.Id == 9),
                wishList2.FirstOrDefault(x => x.Id == 10)
            };

            user.WishList = wishList;
            user.ReservedGifts = reservedGifts;
            user.BoughtGifts = boughtGifts;

            otherUser.WishList = wishList2;

            context.Users.Add(user);
            context.Users.Add(otherUser);
            context.Groups.Add(group);
            context.Events.Add(event1);
            context.Events.Add(event2);

            base.Seed(context);
        }
    }
}