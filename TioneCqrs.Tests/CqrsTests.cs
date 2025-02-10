using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TioneCqrs.Commands;
using TioneCqrs.Queries;
using TioneCqrs.Tests.Cqrs.Commands;
using TioneCqrs.Tests.Cqrs.Queries;

namespace TioneCqrs.Tests;

public class CqrsTests : IDisposable
{
    private ServiceProvider _serviceProvider;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        services.RegisterCommands(Assembly.GetExecutingAssembly());
        services.RegisterQueries(Assembly.GetExecutingAssembly());     
        services.RegisterCommands(Assembly.GetCallingAssembly());
        services.RegisterQueries(Assembly.GetCallingAssembly());

        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    public async Task TestQuery()
    {
        var dispatcher = _serviceProvider.GetRequiredService<IQueryDispatcher>();

        Assert.That(dispatcher, Is.Not.Null);

        var testPhrase = "test123";

        var result = await dispatcher.QueryAsync<TestQuery, string>(new (testPhrase));

        Assert.That(testPhrase, Is.EqualTo(result));
    } 
    
    [Test]
    public async Task TestCommandWithResult()
    {
        var dispatcher = _serviceProvider.GetRequiredService<ICommandDispatcher>();

        Assert.That(dispatcher, Is.Not.Null);

        var testPhrase = "test123";

        var result = await dispatcher.ExecuteAsync<TestCommandWithResult, string>(new (testPhrase));

        Assert.That(testPhrase, Is.EqualTo(result));
    }  
    
    [Test]
    public async Task TestCommandWithoutResult()
    {
        var dispatcher = _serviceProvider.GetRequiredService<ICommandDispatcher>();

        Assert.That(dispatcher, Is.Not.Null);

        await dispatcher.ExecuteAsync(new TestCommandWithoutResult("Test"));
    }

    [TearDown]
    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}