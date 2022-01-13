using Couchbase.Lite;
using MCP.Domain;

namespace MCP.Persistence
{
    public abstract class BaseRepository : IDisposable
    {
        private readonly string _databaseName;
        protected DatabaseManager _databaseManager;
        protected DatabaseManager DatabaseManager
        {
            get
            {
                if (_databaseManager == null)
                {
                    _databaseManager = new DatabaseManager(_databaseName);
                }

                return _databaseManager;
            }
        }

        protected BaseRepository(string databaseName)
        {
            _databaseName = databaseName;
        }

        protected virtual Task<Database> GetDatabaseAsync() => DatabaseManager?.GetDatabaseAsync();

        public virtual void Dispose() => DatabaseManager?.Dispose();

    }
}
