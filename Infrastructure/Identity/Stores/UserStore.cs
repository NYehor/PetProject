using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity.Tables;

namespace Infrastructure.Identity
{
    public class UserStore<TKey> :
        UserStoreBase<ApplicationUser<TKey>, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityRoleClaim<TKey>>,
        IProtectedUserStore<ApplicationUser<TKey>>
        where TKey : IEquatable<TKey>
    {
        private IList<IdentityUserClaim<TKey>> UserClaims { get; set; }
        private IList<IdentityUserRole<TKey>> UserRoles { get; set; }
        private IList<IdentityUserLogin<TKey>> UserLogins { get; set; }
        private IList<IdentityUserToken<TKey>> UserTokens { get; set; }

        public UsersTable<TKey> UsersTable { get; }
        public UserClaimsTable<TKey> UserClaimsTable { get; }
        public UserRolesTable<TKey> UserRolesTable { get; }
        public UserLoginsTable<TKey> UserLoginsTable { get; }
        public UserTokensTable<TKey> UserTokensTable { get; }
        public RolesTable<TKey> RolesTable { get; }

        public UserStore(
                UsersTable<TKey> usersTable,
                UserClaimsTable<TKey> userClaimsTable,
                UserRolesTable<TKey> userRolesTable,
                UserLoginsTable<TKey> userLoginsTable,
                UserTokensTable<TKey> userTokensTable,
                RolesTable<TKey> rolesTable,
                IdentityErrorDescriber describer    
            ) : base(describer)
        {
            UsersTable = usersTable ?? throw new ArgumentException(nameof(usersTable));
            UserClaimsTable = userClaimsTable ?? throw new ArgumentException(nameof(userClaimsTable));
            UserRolesTable = userRolesTable ?? throw new ArgumentException(nameof(userRolesTable));
            UserLoginsTable = userLoginsTable ?? throw new ArgumentException(nameof(userLoginsTable));
            UserTokensTable = userTokensTable ?? throw new ArgumentException(nameof(userTokensTable));
            RolesTable = rolesTable ?? throw new ArgumentException(nameof(rolesTable));
        }

        public override IQueryable<ApplicationUser<TKey>> Users => throw new NotImplementedException();

        public override async Task AddClaimsAsync(ApplicationUser<TKey> user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            claims.ThrowIfNull(nameof(claims));
            UserClaims ??= (await UserClaimsTable.GetClaimsAsync(user.Id)).ToList();

            foreach (var claim in claims)
                UserClaims.Add(CreateUserClaim(user, claim));
        }

        public override async Task AddLoginAsync(ApplicationUser<TKey> user, UserLoginInfo login, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            login.ThrowIfNull(nameof(login));
            UserLogins ??= (await UserLoginsTable.GetLoginsAsync(user.Id)).ToList();
            UserLogins.Add(CreateUserLogin(user, login));
        }

        public override async Task AddToRoleAsync(ApplicationUser<TKey> user, string normalizedRoleName, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));

            if (string.IsNullOrEmpty(normalizedRoleName))
                throw new ArgumentException($"Parameter {nameof(normalizedRoleName)} cannot be null or empty.");

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role == null)
                throw new InvalidOperationException($"Role '{normalizedRoleName}' was not found.");

            var userRoles = (await UserRolesTable.GetRolesAsync(user.Id))?.Select(x => new IdentityUserRole<TKey> {
                UserId = user.Id,
                RoleId = x.Id
            }).ToList();
            UserRoles = userRoles;
            UserRoles.Add(CreateUserRole(user, role));
            
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser<TKey> user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));

            return await UsersTable.CreateAsync(user);
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser<TKey> user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));

            return await UsersTable.DeleteAsync(user);
        }

        public override async Task<ApplicationUser<TKey>> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedEmail))
                throw new ArgumentException($"Parameter {nameof(normalizedEmail)} cannot be null or empty.");

            return await UsersTable.FindByEmailAsync(normalizedEmail);
        }

        public override async Task<ApplicationUser<TKey>> FindByIdAsync(string userId, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException($"Parameter {nameof(userId)} cannot be null or empty.");

            return await UsersTable.FindByIdAsync(ConvertIdFromString(userId));
        }

        public override async Task<ApplicationUser<TKey>> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedUserName))
                throw new ArgumentException($"Parameter {nameof(normalizedUserName)} cannot be null or empty.");

            return await UsersTable.FindByUserNameAsync(normalizedUserName);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(ApplicationUser<TKey> user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            var userClaims = await UserClaimsTable.GetClaimsAsync(user.Id);
            return userClaims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList();
        }

        public override async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser<TKey> user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            var userLogins = await UserLoginsTable.GetLoginsAsync(user.Id);
            return userLogins.Select(x => new UserLoginInfo(x.LoginProvider, x.LoginProvider, x.ProviderDisplayName)).ToList();
        }

        public override async Task<IList<string>> GetRolesAsync(ApplicationUser<TKey> user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            var userLogins = await UserRolesTable.GetRolesAsync(user.Id);
            return userLogins.Select(x => x.Name).ToList();
        }

        public override async Task<IList<ApplicationUser<TKey>>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            claim.ThrowIfNull(nameof(claim));
            var users = await UsersTable.GetUsersForClaimAsync(claim);
            return users.ToList();
        }

        public override async Task<IList<ApplicationUser<TKey>>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
                throw new ArgumentException($"Parameter {nameof(normalizedRoleName)} cannot be null or empty.");

            var users = await UsersTable.GetUsersInRoleAsync(normalizedRoleName);
            return users.ToList();
        }

        public override async Task<bool> IsInRoleAsync(ApplicationUser<TKey> user, string normalizedRoleName, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            if (string.IsNullOrEmpty(normalizedRoleName))
                throw new ArgumentException($"Parameter {nameof(normalizedRoleName)} cannot be null or empty.");

            var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }

            return false;
        }

        public override async Task RemoveClaimsAsync(ApplicationUser<TKey> user, IEnumerable<Claim> claims, CancellationToken cancellationToken )
        {
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            claims.ThrowIfNull(nameof(claims));
            UserClaims ??= (await UserClaimsTable.GetClaimsAsync(user.Id)).ToList();

            foreach (var claim in UserClaims)
            {
                var matchedClaims = UserClaims.Where(x => x.UserId.Equals(user.Id) && x.ClaimType == claim.ClaimType && x.ClaimValue == claim.ClaimValue);
                foreach (var matchedClaim in matchedClaims)
                    UserClaims.Remove(matchedClaim);
            }
        }

        public override async Task RemoveFromRoleAsync(ApplicationUser<TKey> user, string normalizedRoleName, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            if (string.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentException(nameof(normalizedRoleName));
            }
            var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
            if (roleEntity != null)
            {
                var userRoles = (await UserRolesTable.GetRolesAsync(user.Id))?.Select(x => new IdentityUserRole<TKey> { 
                    UserId = user.Id,
                    RoleId = x.Id
                }).ToList();
                UserRoles = userRoles;
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
                if (userRole != null)
                    UserRoles.Remove(userRole);
            }
        }

        public override async Task RemoveLoginAsync(ApplicationUser<TKey> user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            UserLogins ??= (await UserLoginsTable.GetLoginsAsync(user.Id)).ToList();
            var userLogin = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
            if (userLogin != null)
                UserLogins.Remove(userLogin);
        }

        public override async Task ReplaceClaimAsync(ApplicationUser<TKey> user, Claim claim, Claim newClaim, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            claim.ThrowIfNull(nameof(claim));
            newClaim.ThrowIfNull(nameof(newClaim));
            UserClaims ??= (await UserClaimsTable.GetClaimsAsync(user.Id)).ToList();
            var matchedClaims = UserClaims.Where(x => x.UserId.Equals(user.Id) && x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimType = newClaim.Type;
                matchedClaim.ClaimValue = newClaim.Value;
            }
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser<TKey> user, CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.ThrowIfNull(nameof(user));
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            var update = await UsersTable.UpdateAsync(user, UserClaims, UserRoles, UserLogins, UserTokens);

            return update ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { 
                Description = $"User '{user.UserName}' could not be deleted"
            });
        }

        protected override async Task AddUserTokenAsync(IdentityUserToken<TKey> token)
        {
            token.ThrowIfNull(nameof(token));
            UserTokens ??= (await UserTokensTable.GetTokensAsync(token.UserId)).ToList();
            UserTokens.Add(token);
        }

        protected override Task<IdentityRole<TKey>> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedRoleName))
                throw new ArgumentException($"Parameter {nameof(normalizedRoleName)} cannot be null or empty.");

            return RolesTable.FindByUserNameASync(normalizedRoleName);
        }

        protected override async Task<IdentityUserToken<TKey>> FindTokenAsync(ApplicationUser<TKey> user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await UserTokensTable.FindTokenAsync(user.Id, loginProvider, name);
        }

        protected override async Task<ApplicationUser<TKey>> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await UsersTable.FindByIdAsync(userId);
        }

        protected override async Task<IdentityUserLogin<TKey>> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await UserLoginsTable.FindAsync(userId, loginProvider, providerKey);
        }

        protected override async Task<IdentityUserLogin<TKey>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await UserLoginsTable.FindAsync(loginProvider, providerKey);
        }

        protected override Task<IdentityUserRole<TKey>> FindUserRoleAsync(TKey userId, TKey roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return  UserRolesTable.FindUsersRoleAsync(userId, roleId);
        }

        protected override async Task RemoveUserTokenAsync(IdentityUserToken<TKey> token)
        {
            UserTokens ??= (await UserTokensTable.GetTokensAsync(token.UserId)).ToList();
            UserTokens.Remove(token);
        }
    }
}
