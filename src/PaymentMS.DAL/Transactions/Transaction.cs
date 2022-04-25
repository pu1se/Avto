using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace PaymentMS.DAL.Transactions
{
    public class Transaction : ITransaction
    {
        private IDbContextTransaction DbTransaction { get; }
        private bool TransactionWasCommited { get; set; }

        public Transaction(IDbContextTransaction dbTransaction)
        {
            DbTransaction = dbTransaction;
            TransactionWasCommited = false;
        }

        public void Commit()
        {
            try
            {
                DbTransaction.Commit();
                TransactionWasCommited = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }


        public void Dispose()
        {
            ReleaseResources();
            GC.SuppressFinalize(this);
        }

        ~Transaction()
        {
            ReleaseResources();
        }

        private bool _resourcesWasRelease = false;
        private void ReleaseResources()
        {
            if (_resourcesWasRelease)
                return;

            if (!TransactionWasCommited)
            {
                DbTransaction.Rollback();
            }
            DbTransaction.Dispose();
            
            _resourcesWasRelease = true;
        }
    }
}