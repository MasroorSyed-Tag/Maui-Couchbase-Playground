using Couchbase.Lite;
using Couchbase.Lite.Query;
using MCP.Domain;
using MCP.Persistence.Interfaces;

namespace MCP.Persistence
{
    public sealed class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        private IQuery _userQuery;
        private ListenerToken _userQueryToken;

        public UserProfileRepository() : base("userprofile")
        {
        }

        public async Task<UserProfile> GetAsync(string userProfileId)
        {
            UserProfile userProfile = null;

            try
            {
                var database = await GetDatabaseAsync();

                if (database != null)
                {
                    var document = database.GetDocument(userProfileId);

                    userProfile = new UserProfile
                    {
                        Id = document.Id,
                        Name = document.GetString("name"),
                        Email = document.GetString("email"),
                        Address = document.GetString("address"),
                        ImageData = document.GetBlob("imageData")?.Content,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserProfileRepository Exception: {ex.Message}");
            }

            return userProfile;
        }

        public async Task<UserProfile> GetAsync(string userProfileId, Action<UserProfile> userProfileUpdated)
        {
            UserProfile userProfile = null;

            try
            {
                var database = await GetDatabaseAsync();

                if (database != null)
                {
                    _userQuery = QueryBuilder
                                    .Select(SelectResult.All())
                                    .From(DataSource.Database(database))
                                    .Where(Meta.ID.EqualTo(Expression.String(userProfileId)));

                    if (userProfileUpdated != null)
                    {
                        _userQueryToken = _userQuery.AddChangeListener((object sender, QueryChangedEventArgs e) =>
                        {
                            if (e?.Results != null && e.Error == null)
                            {
                                foreach (var result in e.Results.AllResults())
                                {
                                    var dictionary = result.GetDictionary("userprofile");

                                    if (dictionary != null)
                                    {
                                        userProfile = new UserProfile
                                        {
                                            Name = dictionary.GetString("name"),
                                            Email = dictionary.GetString("email"),
                                            Address = dictionary.GetString("address"),
                                            ImageData = dictionary.GetBlob("imageData")?.Content
                                        };
                                    }
                                }

                                if (userProfile != null)
                                {
                                    userProfileUpdated.Invoke(userProfile);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserProfileRepository Exception: {ex.Message}");
            }

            return userProfile;

        }

        public async Task<bool> SaveAsync(UserProfile userProfile)
        {
            try
            {
                if (userProfile != null)
                {
                    var mutableDocument = new MutableDocument(userProfile.Id);
                    mutableDocument.SetString("name", userProfile.Name);
                    mutableDocument.SetString("email", userProfile.Email);
                    mutableDocument.SetString("address", userProfile.Address);
                    mutableDocument.SetString("type", "user");

                    if (userProfile.ImageData != null)
                    {
                        mutableDocument.SetBlob("imageData", new Blob("image/jpeg", userProfile.ImageData));
                    }

                    var database = await GetDatabaseAsync();

                    database.Save(mutableDocument);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UserProfileRepository Exception: {ex.Message}");
            }

            return false;
        }

        public Task StartReplicationForCurrentUser(UserCredentials currentUser)
        {
            return Task.Run(async () => await DatabaseManager.StartReplicationAsync(currentUser.Username, currentUser.Password, new string[] { currentUser.Username }));
        }

        public override void Dispose()
        {
            _userQuery?.RemoveChangeListener(_userQueryToken);

            base.Dispose();
        }
    }
}
