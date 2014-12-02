using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class ErrorExceptionLogService : IErrorExceptionLogService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string ErrorExceptionLogs_ALL_KEY = "SwapStff.ErrorExceptionLogs.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int ErrorExceptionLogs_CACHE_TIME = 2000;

        #endregion

        private IRepository<ErrorExceptionLogs> _ErrorExceptionLogRepository;
        private readonly ICacheManager _cacheManager;

        public ErrorExceptionLogService(IRepository<ErrorExceptionLogs> ErrorExceptionLogRepository, ICacheManager cacheManager)
        {
            this._ErrorExceptionLogRepository = ErrorExceptionLogRepository;
            this._cacheManager = cacheManager;
        }

        public List<ErrorExceptionLogs> GetAll()
        {
            //return _cacheManager.Get(ErrorExceptionLogs_ALL_KEY, () =>
            //{
            //    return _ErrorExceptionLogRepository.GetAll().ToList();
            //});

            return _ErrorExceptionLogRepository.GetAll().ToList();
        }

        public ErrorExceptionLogs GetById(string id)
        {
            return GetAll().Find(l => l.EventId.ToString() == id);
        }

        public List<ErrorExceptionLogs> GetErrorExceptionLogs()
        {
            var ErrorExceptionLogs = _ErrorExceptionLogRepository.GetBy(x => new
            {
                x.EventId,
                x.LogDateTime,
                x.Source,
                x.Message,
                x.QueryString,
                x.TargetSite,
                x.StackTrace,
                x.ServerName,
                x.RequestURL,
                x.UserAgent,
                x.UserIP,
                x.UserAuthentication,
                x.UserName
            }, x => x.EventId != -1);

            var ErrorExceptionLogList = new List<ErrorExceptionLogs>();
            foreach (var item in ErrorExceptionLogs)
            {
                ErrorExceptionLogList.Add(new ErrorExceptionLogs {  EventId=item.EventId, LogDateTime = item.LogDateTime,
                                                                    Source = item.Source,
                                                                    Message = item.Message,
                                                                    QueryString = item.QueryString,
                                                                    TargetSite = item.TargetSite,
                                                                    StackTrace = item.StackTrace,
                                                                    ServerName = item.ServerName,
                                                                    RequestURL = item.RequestURL,
                                                                    UserAgent = item.UserAgent,
                                                                    UserIP = item.UserIP,
                                                                    UserAuthentication = item.UserAuthentication,
                                                                    UserName = item.UserName});
            }
            return ErrorExceptionLogList;
        }

        public void Insert(ErrorExceptionLogs model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("ErrorExceptionLog");
            }
            
            List<ErrorExceptionLogs> ErrorExceptionLogs = GetAll();
            ErrorExceptionLogs.Add(model);
            _cacheManager.Set(ErrorExceptionLogs_ALL_KEY, ErrorExceptionLogs, ErrorExceptionLogs_CACHE_TIME);

            _ErrorExceptionLogRepository.Insert(model);
        }

        public void Update(ErrorExceptionLogs model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("ErrorExceptionLog");
            }

            ErrorExceptionLogs ErrorExceptionLog = GetById(model.EventId.ToString());

            _cacheManager.Set(ErrorExceptionLogs_ALL_KEY, ErrorExceptionLog, ErrorExceptionLogs_CACHE_TIME);

            if (ErrorExceptionLog != null)
                _ErrorExceptionLogRepository.Update(model);
        }

        public void Delete(ErrorExceptionLogs model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("ErrorExceptionLogs");
            }

            List<ErrorExceptionLogs> ErrorExceptionLogs = GetAll();
            ErrorExceptionLogs.Remove(ErrorExceptionLogs.Find(l => l.EventId == model.EventId));
            _cacheManager.Set(ErrorExceptionLogs_ALL_KEY, ErrorExceptionLogs, ErrorExceptionLogs_CACHE_TIME);

            _ErrorExceptionLogRepository.Delete(model);
        }
    }
}
