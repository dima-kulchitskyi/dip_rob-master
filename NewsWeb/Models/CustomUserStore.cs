using Microsoft.AspNet.Identity;
using NewsWebSite.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NewsWebSite.Models
{
    public class CustomUserStore : IUserStore<AppUser, int>, IUserPasswordStore<AppUser, int>, IUserLockoutStore<AppUser, int>, IUserTwoFactorStore<AppUser, int>
    {
        readonly IUserRepository repository;
        public CustomUserStore(IUserRepository repository)
        {
            this.repository = repository;
        }

        public Task CreateAsync(AppUser user)
        {
            return Task.FromResult(repository.Save(user));
        }

        public Task DeleteAsync(AppUser user)
        {
            return Task.FromResult(true);
        }

        public void Dispose()
        {
        }

        public Task<AppUser> FindByIdAsync(int userId)
        {
            return Task.FromResult(repository.GetById(userId));
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(repository.FindByName(userName));
        }

        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            return Task.FromResult(user.lockoutEnd);
        }

        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasPasswordAsync(AppUser user)
        {
            return Task.FromResult(string.IsNullOrEmpty(user.Password));
        }

        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(--user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(true);

        }

        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(true);
        }

        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            user.lockoutEnd = lockoutEnd;
            return Task.FromResult(true);
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(true);
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AppUser user)
        {
            return Task.FromResult(repository.Save(user));
        }
    }
}