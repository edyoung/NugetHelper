using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.Common;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.PowerShellGet.NugetHelper
{
    public class FindModule
    {
        public async Task<HashSet<SourcePackageDependencyInfo> > Find(string name, string version)
        {
            var packageVersion = NuGetVersion.Parse(version);
            var packageIdentity = new PackageIdentity(name, packageVersion);

            var nuGetFramework = NuGetFramework.ParseFolder("net46");
            var settings = Settings.LoadDefaultSettings(root: null);

            var packageSourceProvider = new PackageSourceProvider(
                settings, new PackageSource[] { new PackageSource("https://www.powershellgallery.com/api/v2/") });

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, Repository.Provider.GetCoreV3());

            var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);

            using (var cacheContext = new SourceCacheContext())
            {
                var repositories = sourceRepositoryProvider.GetRepositories();
                
                foreach (var sourceRepository in repositories)
                {
                    var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                    var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                        packageIdentity, nuGetFramework, cacheContext, NullLogger.Instance , CancellationToken.None);

                    if (dependencyInfo == null) continue;

                    availablePackages.Add(dependencyInfo);
                }
            }

            return availablePackages;
        }
    }
}
