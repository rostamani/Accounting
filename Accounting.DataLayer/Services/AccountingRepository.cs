using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Repositories;
using Accounting.ViewModels;

namespace Accounting.DataLayer.Services
{
    public class AccountingRepository : IAccountingRepository
    {
        private Accounting_DBEntities _db;
        public AccountingRepository(Accounting_DBEntities db)
        {
            _db = db;
        }
        public bool DeleteTransaction(Accounting transaction)
        {
            try
            {
                _db.Entry(transaction).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteTransaction(int accountingId)
        {
            var transaction = GetTransactionById(accountingId);
            return DeleteTransaction(transaction);
        }

        public List<Accounting> GetAllTransactions(int transactionType, int customerId,DateTime? fromDate, DateTime? toDate)
        {
            IQueryable<Accounting> result = _db.Accounting.Where(a => a.TypeId == transactionType);

            if (customerId != 0)
                result = result.Where(a => a.CustomerId == customerId);
            if(fromDate!=null)
                result = result.Where(a => a.Date >= fromDate);
            if(toDate != null)
                result = result.Where(a => a.Date <= toDate);

            return result.ToList();
        }

        public MonthlyReportViewModel GetReport()
        {
            DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);
            int receive = _db.Accounting.Where(a => a.TypeId == 1 && a.Date>=fromDate)
                .Select(a => a.Price).ToList().Sum();
            int pay = _db.Accounting.Where(a => a.TypeId == 2 && a.Date <=toDate)
                .Select(a => a.Price).ToList().Sum();
            int balance = receive - pay;
            return new MonthlyReportViewModel()
            {
                Pay = pay,
                Receive = receive,
                Balance = balance
            };
        }

        public Accounting GetTransactionById(int accountingId)
        {
            return _db.Accounting.Find(accountingId);
        }

        public bool InsertTransaction(Accounting transaction)
        {
            try
            {

                _db.Accounting.Add(transaction);
                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool UpdatetTransaction(Accounting transaction)
        {
            try
            {
                var local = _db.Set<Accounting>()
               .Local
               .FirstOrDefault(f => f.AccountingId == transaction.AccountingId);
                if (local != null)
                {
                    _db.Entry(local).State = EntityState.Detached;
                }
                _db.Entry(transaction).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
