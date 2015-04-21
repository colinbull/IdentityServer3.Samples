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

using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SelfHost.AspId
{
    public class CustomUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole> { }

    public class CustomContext : IdentityDbContext<CustomUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomContext(string connString)
            : base(connString)
        {
        }
    }

    public class CustomUserStore : UserStore<CustomUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(CustomContext ctx)
            : base(ctx)
        {
            ctx.Database.Log = Console.WriteLine;
        }

        public override Task CreateAsync(CustomUser user)
        {
            Console.WriteLine("Wrting user");
            return base.CreateAsync(user);
        }

        

        public override Task AddLoginAsync(CustomUser user, UserLoginInfo login)
        {
            Console.WriteLine("Wrting user");
            return base.AddLoginAsync(user, login);
        }
    }

    public class CustomUserManager : UserManager<CustomUser, int>
    {
        public CustomUserManager(CustomUserStore store)
            : base(store)
        {
        }

        public override Task<IdentityResult> CreateAsync(CustomUser user)
        {
            return base.CreateAsync(user);
        }

        public override Task<IdentityResult> CreateAsync(CustomUser user, string password)
        {
            return base.CreateAsync(user, password);
        }
    }
    
    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(CustomContext ctx)
            : base(ctx)
        {
        }
    }

    public class CustomRoleManager : RoleManager<CustomRole, int>
    {
        public CustomRoleManager(CustomRoleStore store)
            : base(store)
        {
        }
    }
}