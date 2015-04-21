/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SelfHost.AspId
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public class AdUser : IUser<string>
    {
        public AdUser()
        {
            Claims = new List<IdentityUserClaim>();
            Roles = new List<IdentityUserRole>();
            Logins = new List<IdentityUserLogin>();
        }

        public ICollection<IdentityUserClaim> Claims { get; private set; }

        public ICollection<IdentityUserRole> Roles { get; private set; }

        public ICollection<IdentityUserLogin> Logins { get; private set; } 

        public string Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class Role : IdentityRole { }

    //public class Context : DbContext
    //{
    //    /// <summary>
    //    /// IDbSet of Users
    //    /// 
    //    /// </summary>
    //    public virtual IDbSet<AdUser> Users { get; set; }

    //    /// <summary>
    //    /// IDbSet of Roles
    //    /// 
    //    /// </summary>
    //    public virtual IDbSet<Role> Roles { get; set; }

    //    public Context(string connectionString)
    //        : base(connectionString)
    //    { }

    //    /// <summary>
    //    /// Maps table names, and sets up relationships between the various user entities
    //    /// 
    //    /// </summary>
    //    /// <param name="modelBuilder"/>
    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        //TODO: Replace with real code but this will do for now
    //        if (modelBuilder == null)
    //        {
    //            throw new ArgumentNullException("modelBuilder");
    //        }

    //        var user = modelBuilder.Entity<AdUser>().ToTable("Users");

    //        user.HasMany(u => u.Roles).WithRequired().HasForeignKey(u => u.UserId);
    //        user.HasMany(u => u.Logins).WithRequired().HasForeignKey(u => u.UserId);
    //        user.HasMany(u => u.Claims).WithRequired().HasForeignKey(u => u.UserId);

    //        user.Property(u => u.UserName).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index", (object)new IndexAnnotation(new IndexAttribute("UserNameIndex")
    //        {
    //            IsUnique = true
    //        }));

    //        modelBuilder.Entity<IdentityUserRole>().HasKey(r => new {r.UserId, r.RoleId}).ToTable("UserRoles");
    //        modelBuilder.Entity<IdentityUserLogin>()
    //            .HasKey(r => new {r.LoginProvider, r.ProviderKey, r.UserId})
    //            .ToTable("UserLogins");
    //        modelBuilder.Entity<IdentityUserClaim>().ToTable("Claims");

    //        var roles = modelBuilder.Entity<Role>().ToTable("Roles");
    //        roles.Property(u => u.Name).IsRequired().HasMaxLength(256).HasColumnAnnotation("Index", (object)new IndexAnnotation(new IndexAttribute("RoleNameIndex")
    //        {
    //            IsUnique = true
    //        }));

    //        roles.HasMany(r => r.Users).WithRequired().HasForeignKey(r => r.RoleId);
    //    }

    //    /// <summary>
    //    /// Validates that UserNames are unique and case insenstive
    //    /// 
    //    /// </summary>
    //    /// <param name="entityEntry"/><param name="items"/>
    //    /// <returns/>
    //    protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
    //    {
    //      //TODO: replace with actual code
    //      return base.ValidateEntity(entityEntry, items);
    //    }
    //}

    public class Context : IdentityDbContext<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public Context(string connString)
            : base(connString)
        {
        }
    }

    public class UserStore : UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(Context context)
            : base(context)
        {
            
        }
    }

    //public class UserStore : IQueryableUserStore<AdUser>
    //{
    //    public Context Db { get; set; }

    //    public UserStore(Context db)
    //    {
    //        Db = db;
    //        db.Database.Log = Console.WriteLine;
    //    }

    //    public void Dispose()
    //    {
            
    //    }

    //    public Task CreateAsync(AdUser user)
    //    {
    //        Db.Users.Add(user);
    //        return Db.SaveChangesAsync();
    //    }

    //    public Task UpdateAsync(AdUser user)
    //    {
    //        var attached = Db.Users.Attach(user);

    //        Db.Entry(attached).State = EntityState.Modified;
    //        return Db.SaveChangesAsync();

    //    }

    //    public Task DeleteAsync(AdUser user)
    //    {
    //        Db.Users.Remove(user);
    //        return Db.SaveChangesAsync();
    //    }

    //    public Task<AdUser> FindByIdAsync(string userId)
    //    {
    //        return Db.Users.FirstOrDefaultAsync(u => u.Id == userId);
    //    }

    //    public Task<AdUser> FindByNameAsync(string userName)
    //    {
    //        return Db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    //    }

    //    public IQueryable<AdUser> Users { get { return Db.Users; } }
    //}

    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            this.ClaimsIdentityFactory = new ClaimsFactory();
           
        }

    }

    public class ClaimsFactory : ClaimsIdentityFactory<User, string>
    {
        public ClaimsFactory()
        {
            this.UserIdClaimType = Thinktecture.IdentityServer.Core.Constants.ClaimTypes.Subject;
            this.UserNameClaimType = Thinktecture.IdentityServer.Core.Constants.ClaimTypes.PreferredUserName;
            this.RoleClaimType = Thinktecture.IdentityServer.Core.Constants.ClaimTypes.Role;
        }

        public override async Task<System.Security.Claims.ClaimsIdentity> CreateAsync(UserManager<User, string> manager, User user, string authenticationType)
        {
            var ci = await base.CreateAsync(manager, user, authenticationType);
            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                ci.AddClaim(new Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                ci.AddClaim(new Claim("family_name", user.LastName));
            }
            return ci;
        }
    }
    
    public class RoleStore : RoleStore<Role>
    {
        public RoleStore(Context ctx)
            : base(ctx)
        {
        }
    }

    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }
    }


}