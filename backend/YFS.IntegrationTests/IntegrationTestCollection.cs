using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFS.IntegrationTests
{
    [CollectionDefinition("IntegrationTests")]
    public class IntegrationTestCollection : IClassFixture<TestingWebAppFactory<Program>>
    {
        // This class has no code because it only serves as a marker for the test collection fixture
    }
}
