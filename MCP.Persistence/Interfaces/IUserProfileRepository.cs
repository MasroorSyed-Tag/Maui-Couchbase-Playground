using MCP.Domain;

namespace MCP.Persistence.Interfaces
{
    public interface IUserProfileRepository : IDisposable
    {
        Task<UserProfile> GetAsync(string userProfileId);
        Task<UserProfile> GetAsync(string userProfileId, Action<UserProfile> userProfileUpdate);
        Task<bool> SaveAsync(UserProfile userProfile);
        Task StartReplicationForCurrentUser(UserCredentials currentUser);
    }
}
