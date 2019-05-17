using System;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.PowerShell.PowerShellGet.NugetHelper
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            await Program.Main();
        }
    }
}
