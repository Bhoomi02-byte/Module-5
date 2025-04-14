namespace Module_5.Services
{
    public interface ISubscribeService
    {
        Task<string> SubscribeAsync(string userId, string authorId);
        Task<string> UnsubscribeAsync(string userId, string authorId);
    }
}
