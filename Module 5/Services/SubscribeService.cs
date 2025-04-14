using Microsoft.EntityFrameworkCore;
using Module_5.Collections;
using Module_5.Data;
using MongoDB.Driver;

namespace Module_5.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly IMongoDbContext _context;

        public SubscribeService(IMongoDbContext context)
        {
            _context = context;
        }
        public async Task<string> SubscribeAsync(string userId, string authorId)
        {
            var exists = await _context.Subscriptions
        .Find(s => s.UserId == userId && s.AuthorId == authorId).AnyAsync();

            if (exists)
                return JsonHelper.GetMessage(149);

            var subscription = new Subscription
            {
                UserId = userId,
                AuthorId = authorId,
            };

           await _context.Subscriptions.InsertOneAsync(subscription);
           
            return JsonHelper.GetMessage(145);
        }

        public async Task<string> UnsubscribeAsync(string userId, string authorId)
        {
            var subscription = await _context.Subscriptions
                .Find(s => s.UserId == userId && s.AuthorId == authorId).FirstOrDefaultAsync();

            if (subscription == null)
                return JsonHelper.GetMessage(146);

          await  _context.Subscriptions.DeleteOneAsync(s => s.Id == subscription.Id);
         
            return JsonHelper.GetMessage(147);
        }
    }
}

