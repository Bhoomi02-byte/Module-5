using Microsoft.EntityFrameworkCore;
using Module_5.Data;
using Module_5.Models.Entities;

namespace Module_5.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly BlogDbContext _context;

        public SubscribeService(BlogDbContext context)
        {
            _context = context;
        }
        public async Task<string> SubscribeAsync(int userId, int authorId)
        {
            var exists = await _context.Subscriptions
        .AnyAsync(s => s.UserId == userId && s.AuthorId == authorId);

            if (exists)
                return JsonHelper.GetMessage(149);

            var subscription = new Subscription
            {
                UserId = userId,
                AuthorId = authorId,   
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return JsonHelper.GetMessage(145);
        }

        public async Task<string> UnsubscribeAsync(int userId, int authorId)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.AuthorId == authorId);

           if (subscription == null)
                return JsonHelper.GetMessage(146);

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return JsonHelper.GetMessage(147);
        }
    }
 }

