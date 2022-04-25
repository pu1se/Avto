using System;
using System.Collections.Generic;
using System.Text;

namespace Avto.DAL.Transactions
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}
