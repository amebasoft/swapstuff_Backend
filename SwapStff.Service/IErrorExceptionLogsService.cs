using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;

namespace SwapStff.Service
{
    public interface IErrorExceptionLogService
    {
        List<ErrorExceptionLogs> GetAll();
        ErrorExceptionLogs GetById(string id);
        List<ErrorExceptionLogs> GetErrorExceptionLogs();
        void Insert(ErrorExceptionLogs model);
        void Update(ErrorExceptionLogs model);
        void Delete(ErrorExceptionLogs model);
    }
}
