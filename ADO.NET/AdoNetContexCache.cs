
using Cos.BCS.Infrastructure.UnitOfWork;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace ADO.NET
{
    public class AdoNetContextCache : IDataContextCache, IDisposable
    {
        private readonly IDbConnection _connection;

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        private readonly IConnectionFactoryCache _connectionFactory;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly LinkedList<AdoNetUnitOfWork> _unitOfWorkList = new LinkedList<AdoNetUnitOfWork>();

        public AdoNetContextCache(IConnectionFactoryCache connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = _connectionFactory.CreateConnection();
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            if (_connection.State != ConnectionState.Open && _connection.State != ConnectionState.Connecting)
            {
                _connection.Open();
            }
            var transaction = _connection.BeginTransaction();
            var unitOfWork = new AdoNetUnitOfWork(transaction, RemoveTransaction,
                RemoveTransaction);
            _lock.EnterWriteLock();
            _unitOfWorkList.AddLast(unitOfWork);
            _lock.ExitWriteLock();
            return unitOfWork;

        }

        public IDbCommand CreateCommand()
        {
            var command = _connection.CreateCommand();
            _lock.EnterReadLock();
            if (_unitOfWorkList.Count > 0)
                command.Transaction = _unitOfWorkList.First.Value.Transaction;
            _lock.ExitReadLock();
            return command;
        }

        private void RemoveTransaction(AdoNetUnitOfWork obj)
        {
            _lock.EnterWriteLock();
            _unitOfWorkList.Remove(obj);
            _lock.ExitWriteLock();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}