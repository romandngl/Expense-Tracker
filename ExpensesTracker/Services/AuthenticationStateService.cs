using System;
using ExpensesTracker.Models;

namespace ExpensesTracker.Services
{
    public class AuthenticationStateService
    {
        private User authenticatedUser;
        public Task<string> GetAuthenticatedUserEmailAsync()
        {
            return Task.FromResult(authenticatedUser?.Email);
        }

        public User GetAuthenticatedUser()
        {
            return authenticatedUser;
        }

        public void SetAuthenticatedUser(User user)
        {
            authenticatedUser = user;
        }

        public bool IsAuthenticated()
        {
            if (authenticatedUser != null)
            {
                return true;
            }
            return false;
        }

        public void Logout()
        {
            authenticatedUser = null;
        }
    }
}
