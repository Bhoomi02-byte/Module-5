namespace Module_5.Services
{
    public interface ISubscribeService
    {
        Task<string> SubscribeAsync(int userId, int authorId);
        Task<string> UnsubscribeAsync(int userId, int authorId);
    }
}
