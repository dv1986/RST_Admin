using System;
using System.Data;
using Cos.BCS.Infrastructure.UnitOfWork;


namespace ADO.NET
{
    public class AdoNetUnitOfWork : IUnitOfWork
    {
        private IDbTransaction _transaction;

        public IDbTransaction Transaction
        {
            get { return _transaction; }
        }

        private readonly Action<AdoNetUnitOfWork> _rolledBack;
        private readonly Action<AdoNetUnitOfWork> _committed;

        public AdoNetUnitOfWork(IDbTransaction transaction, Action<AdoNetUnitOfWork> committed,
            Action<AdoNetUnitOfWork> rolledBack)
        {
            _transaction = transaction;
            _committed = committed;
            _rolledBack = rolledBack;
        }

        public void Complete()
        {
            if (_transaction == null)
                throw new InvalidOperationException("May not call Commit twice.");
            _transaction.Commit();
            if (_transaction.Connection != null && _transaction.Connection.State != ConnectionState.Closed)
            {
                _transaction.Connection.Close();
            }
            _transaction = null;
            _committed(this);
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            if (_transaction.Connection != null && _transaction.Connection.State != ConnectionState.Closed)
            {
                _transaction.Rollback();
                if (_transaction.Connection != null && _transaction.Connection.State != ConnectionState.Closed)
                { _transaction.Connection.Close(); }
            }
            _transaction = null;
            _rolledBack(this);

        }
    }
}