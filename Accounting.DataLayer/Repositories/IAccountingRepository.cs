using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.ViewModels;

namespace Accounting.DataLayer.Repositories
{
    public interface IAccountingRepository
    {
        List<Accounting> GetAllTransactions(int transactionType, int customerId,DateTime? fromDate,DateTime? toDate);
        Accounting GetTransactionById(int accountingId);
        bool InsertTransaction(Accounting transaction);
        bool UpdatetTransaction(Accounting transaction);
        bool DeleteTransaction(Accounting transaction);
        bool DeleteTransaction(int accountingId);

        MonthlyReportViewModel GetReport();
    }
}
