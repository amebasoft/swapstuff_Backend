using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwapStff.Entity;
using SwapStff.Core.Cache;

namespace SwapStff.Service
{
    public class UserService : IUserService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string UserS_ALL_KEY = "SwapStff.Users.all";

        /// <summary>
        /// Time in seconds before cache expires
        /// </summary>
        private const int UserS_CACHE_TIME = 2000;

        #endregion

        private IRepository<User> _UserRepository;
        private readonly ICacheManager _cacheManager;

        public UserService(IRepository<User> UserRepository, ICacheManager cacheManager)
        {
            this._UserRepository = UserRepository;
            this._cacheManager = cacheManager;
        }

        public List<User> GetAll()
        {
            //return _cacheManager.Get(UserS_ALL_KEY, () =>
            //{
               
            //});
            return _UserRepository.GetAll().ToList();
        }

        public User GetById(string id)
        {

            return GetAll().Find(l => l.UserID.ToString() == id);
        }

        public List<User> GetUsers()
        {
            var Users = _UserRepository.GetBy(x => new
            {
                x.UserID,
                x.UserName,
                x.EmailID,
                x.Password,
                x.ParentID,
                x.CreatedOn,
                x.LastUpdatedOn,
                x.IsActive
            }, x => x.UserID != -1);

            var userList = new List<User>();
            foreach (var item in Users)
            {
                userList.Add(new User
                {
                    UserID=item.UserID,
                    UserName = item.UserName,
                    EmailID=item.EmailID,
                    Password=item.Password,
                    ParentID=item.ParentID,
                    CreatedOn=item.CreatedOn,
                    LastUpdatedOn=item.LastUpdatedOn,
                    IsActive=item.IsActive
                });
            }
            return userList;
        }

        public void Insert(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("User");
            }
            
            
            List<User> Users = GetAll();
            Users.Add(model);
            _cacheManager.Set(UserS_ALL_KEY, Users, UserS_CACHE_TIME);

            _UserRepository.Insert(model);
        }

        public void Update(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("User");
            }

            User User = GetById(model.UserID.ToString());

            _cacheManager.Set(UserS_ALL_KEY, User, UserS_CACHE_TIME);

            if (User != null)
                _UserRepository.Update(model);
        }

        public void Delete(User model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("User");
            }

            List<User> Users = GetAll();
            Users.Remove(Users.Find(l => l.UserID == model.UserID));
            _cacheManager.Set(UserS_ALL_KEY, Users, UserS_CACHE_TIME);

            _UserRepository.Delete(model);
        }
    }
}
