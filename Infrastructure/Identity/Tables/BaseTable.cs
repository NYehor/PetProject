using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Infrastructure.Identity.Tables
{
    public abstract class BaseTable : IDisposable
    {
        protected readonly SqlConnection _connection;

        public BaseTable(SqlConnection connection)
        {
            _connection = connection;
        }

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) {
                return;
            }

            if (disposing) {
                OnDispose();
            }

            _disposed = true;
        }
    }
}
