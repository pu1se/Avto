using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS.DAL.Transactions
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}
