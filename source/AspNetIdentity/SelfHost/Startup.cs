﻿using IdentityManager.Core.Logging;
using IdentityManager.Logging;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
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
using Owin;
using SelfHost.IdSvr;
using IdentityManager;
using IdentityManager.Configuration;
using Thinktecture.IdentityServer.Core.Configuration;
using SelfHost.IdMgr;

namespace SelfHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

            app.Map("/admin", adminApp =>
            {
                var factory = new IdentityManagerServiceFactory();
                factory.ConfigureSimpleIdentityManagerService("AspId");
               
                adminApp.UseIdentityManager(new IdentityManagerOptions()
                {
                    Factory = factory
                });
            });

            var idSvrFactory = Factory.Configure();
            idSvrFactory.ConfigureUserService("AspId");

            var options = new IdentityServerOptions
            {
                SiteName = "Thinktecture IdentityServer3 - UserService-AspNetIdentity",
                SigningCertificate = Certificate.Get(),
                Factory = idSvrFactory,
                CorsPolicy = CorsPolicy.AllowAll
            };

            app.UseIdentityServer(options);
        }
    }
}