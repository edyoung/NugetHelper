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
using System.Linq;

namespace Microsoft.PowerShell.PowerShellGet.NugetHelper
{
    public class FindModule
    {
        public async Task<IEnumerable<IPackageSearchMetadata> > Find(string name, string version)
        {
            var settings = NullSettings.Instance; 
            var packageSourceProvider = new PackageSourceProvider(
                settings, 
                new PackageSource[] { new PackageSource("https://www.powershellgallery.com/api/v2/") }
                );

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, Repository.Provider.GetCoreV3());

            using (var cacheContext = new SourceCacheContext())
            {
                var repositories = sourceRepositoryProvider.GetRepositories();

                IEnumerable<IPackageSearchMetadata> results = new List<IPackageSearchMetadata>();

                var searchFilter = new SearchFilter(true, SearchFilterType.IsAbsoluteLatestVersion);
                
                foreach(var repo in repositories)
                {
                    var resource = await repo.GetResourceAsync<PackageSearchResource>();
             
                    var info = await resource.SearchAsync(name, searchFilter, 0, 200, NullLogger.Instance, CancellationToken.None);
                    results = results.Concat(info);
                }
                return results;
              
            }
        }
    }
}
