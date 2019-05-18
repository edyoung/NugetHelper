using System;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.PowerShell.PowerShellGet.NugetHelper
{
    public class UnitTest1
    {
        //[Fact]
        public async Task Test1()
        {
            await Program.Main();
        }

        [Fact]
        public async Task PSGallery_FindPowerShellGet()
        {
            FindModule f = new FindModule();
            var packages = await f.Find("PowerShellGet","2.1.3");
            int count = 0;
            foreach(var p in packages)
            {
                count++;
            }
            Console.WriteLine(count);
        }
    }
}
