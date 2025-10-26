using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TioneCqrs.Commands;
using TioneCqrs.Queries;
using TioneCqrs.Tests.Cqrs.Commands;
using TioneCqrs.Tests.Cqrs.Queries;

namespace TioneCQRS.TestsLegacy
{
    [TestClass]
    public class CqrsTests : IDisposable
    {
        private ServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            services.RegisterCommands(Assembly.GetExecutingAssembly());
            services.RegisterQueries(Assembly.GetExecutingAssembly());
            services.RegisterCommands(Assembly.GetCallingAssembly());
            services.RegisterQueries(Assembly.GetCallingAssembly());

            _serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public async Task TestQuery()
        {
            var dispatcher = _serviceProvider.GetRequiredService<IQueryDispatcher>();

            Assert.IsNotNull(dispatcher);

            var testPhrase = "test123";

            var result = await dispatcher.QueryAsync<TestQuery, string>(new TestQuery(testPhrase));

            Assert.AreEqual(testPhrase, result);
        }

        [TestMethod]
        public async Task TestCommandWithResult()
        {
            var dispatcher = _serviceProvider.GetRequiredService<ICommandDispatcher>();

            Assert.IsNotNull(dispatcher);

            var testPhrase = "test123";

            var result = await dispatcher.ExecuteAsync<TestCommandWithResult, string>(new TestCommandWithResult(testPhrase));

            Assert.AreEqual(testPhrase, result);
        }

        [TestMethod]
        public async Task TestCommandWithoutResult()
        {
            var dispatcher = _serviceProvider.GetRequiredService<ICommandDispatcher>();

            Assert.IsNotNull(dispatcher);

            await dispatcher.ExecuteAsync(new TestCommandWithoutResult("Test"));
        }

        [TestCleanup]
        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }
    }
}