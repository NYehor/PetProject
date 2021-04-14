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
    public class RoleStore<TKey> : RoleStoreBase<IdentityRole<TKey>, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>
        where TKey : IEquatable<TKey>
    {
        private IList<IdentityRoleClaim<TKey>> RoleClaims { get; set; }

        public RolesTable<TKey> RolesTable { get; set; }
        public RoleClaimsTable<TKey> RoleClaimsTable { get; set; }

        public RoleStore(IdentityErrorDescriber describer, 
            RolesTable<TKey> rolesTable,
            RoleClaimsTable<TKey> roleClaimsTable
            ) : base(describer)
        {
            RolesTable = rolesTable;
            RoleClaimsTable = roleClaimsTable;
        }

        public override IQueryable<IdentityRole<TKey>> Roles => throw new NotImplementedException();

        public override async Task AddClaimAsync(IdentityRole<TKey> role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));

            RoleClaims ??= (await RoleClaimsTable.GetClaimsAsync(role.Id)).ToList();
            RoleClaims.Add(CreateRoleClaim(role, claim));
        }

        public override async Task<IdentityResult> CreateAsync(IdentityRole<TKey> role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.ThrowIfNull(nameof(role));

            var created = await RolesTable.CreateAsync(role);

            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError{
                Description = $"Role '{role.Name}' could not be created."
            });
        }

        public override async Task<IdentityResult> DeleteAsync(IdentityRole<TKey> role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.ThrowIfNull(nameof(role));

            return await RolesTable.DeleteAsync(role);
        }

        public override async Task<IdentityRole<TKey>> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"Parameter {nameof(id)} cannot be null or empty.");

            return await RolesTable.FindByIdAsync(id);
        }

        public override async Task<IdentityRole<TKey>> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(normalizedName))
                throw new ArgumentException($"Parameter {nameof(normalizedName)} cannot be null or empty.");

            return await RolesTable.FindByNameAsync(normalizedName);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(IdentityRole<TKey> role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.ThrowIfNull(nameof(role));
            var roleClaims = await RoleClaimsTable.GetClaimsAsync(role.Id);
            return roleClaims.Select(x=> new Claim(x.ClaimType, x.ClaimValue)).ToList(); 
        }

        public override async Task RemoveClaimAsync(IdentityRole<TKey> role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));

            RoleClaims ??= (await RoleClaimsTable.GetClaimsAsync(role.Id)).ToList();
            var roleClaims = RoleClaims.Where(x => x.RoleId.Equals(role.Id) && x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            foreach (var roleCalim in roleClaims)
                RoleClaims.Remove(roleCalim);
        }

        public override async Task<IdentityResult> UpdateAsync(IdentityRole<TKey> role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.ThrowIfNull(nameof(role));

            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            var updated = await RolesTable.UpdateAsync(role, RoleClaims);

            return updated ? IdentityResult.Success : IdentityResult.Failed( 
                new IdentityError { Description = $"Role '{role.Name}' could not be updated."}
            );
        }
    }
}

