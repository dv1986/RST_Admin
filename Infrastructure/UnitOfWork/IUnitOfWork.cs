using System;

namespace Cos.BCS.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}