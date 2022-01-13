using Couchbase.Lite;
using Couchbase.Lite.DI;
using Couchbase.Lite.Sync;
using MCP.Domain;

namespace MCP.Persistence
{
    public class DatabaseManager : IDisposable
    {
        private readonly Uri _remoteSyncUrl = new Uri("ws://localhost:4984/userprofile");
        private readonly string _databaseName;

        private Replicator _replicator;
        private ListenerToken _replicatorListenerToken;
        private Database _database;

        public DatabaseManager(string databaseName)
        {
            _databaseName = databaseName;
        }

        public async Task<Database> GetDatabaseAsync()
        {
            if (_database == null)
            {
                var defaultDirectory = Service.GetInstance<IDefaultDirectoryResolver>().DefaultDirectory();

                try
                {
                    var databaseConfig = new DatabaseConfiguration
                    {
                        Directory = defaultDirectory
                    };
                    _database = new Database(_databaseName, databaseConfig);
                }
                catch (Exception ex)
                {

                }

            }

            return _database;
        }

        public async Task StartReplicationAsync(string username, string password, string[] channels, ReplicatorType replicatorType = ReplicatorType.PushAndPull, bool continuous = true)
        {
            try
            {
                var database = await GetDatabaseAsync();

                var targetUrlEndpoint = new URLEndpoint(new Uri(_remoteSyncUrl, _databaseName));

                var configuration = new ReplicatorConfiguration(database, targetUrlEndpoint)
                {
                    ReplicatorType = replicatorType,
                    Continuous = continuous,
                    Authenticator = new BasicAuthenticator(username, password),
                    Channels = channels?.Select(x => $"channel.{x}").ToArray()
                };

                _replicator = new Replicator(configuration);

                _replicatorListenerToken = _replicator.AddChangeListener(OnReplicatorUpdate);

                _replicator.Start();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void OnReplicatorUpdate(object sender, ReplicatorStatusChangedEventArgs e)
        {
            // TODO
        }

        public void StopReplication()
        {
            _replicator.RemoveChangeListener(_replicatorListenerToken);
            _replicator.Stop();
        }

        public void Dispose()
        {
            if (_replicator != null)
            {
                StopReplication();

                while (true)
                {
                    if (_replicator.Status.Activity == ReplicatorActivityLevel.Stopped)
                    {
                        break;
                    }
                }

                _replicator.Dispose();
            }

            _database.Close();
            _database = null;
        }
    }
}
