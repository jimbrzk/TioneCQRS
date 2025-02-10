# TioneCQRS

TioneCQRS is a lightweight .NET 9 library that provides an easy-to-use implementation of the **Command Query Responsibility Segregation (CQRS) pattern**. It simplifies handling commands and queries by utilizing dependency injection and an intuitive API for dispatching commands and queries.

## Features

- Simple registration of **commands** and **queries** using `IServiceCollection`.
- Supports **command execution with and without results**.
- Provides an **async query dispatcher** for efficient query handling.
- Built-in integration with **Microsoft.Extensions.DependencyInjection**.

## Installation

To use TioneCQRS, install the package via NuGet:

```sh
Install-Package TioneCqrs
```

## Usage

### 1. Registering Commands and Queries

In your `Startup.cs` or during service configuration, register commands and queries from the desired assemblies:

```csharp
var services = new ServiceCollection();

// Register commands and queries from the current assembly
services.RegisterCommands(Assembly.GetExecutingAssembly());
services.RegisterQueries(Assembly.GetExecutingAssembly());

// Register commands and queries from the calling assembly (if needed)
services.RegisterCommands(Assembly.GetCallingAssembly());
services.RegisterQueries(Assembly.GetCallingAssembly());

var serviceProvider = services.BuildServiceProvider();
```

### 2. Defining a Query

Create a query that returns a result:

```csharp
public record TestQuery(string Phrase) : IQuery;

public class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    public Task<string> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(query.Phrase);
    }
}
```

### 3. Defining a Command

Commands can return a result or execute an action without returning data.

#### Command with a result:

```csharp
public record TestCommandWithResult(string Phrase) : ICommand;

public class TestCommandWithResultHandler : ICommandHandler<TestCommandWithResult, string>
{
    public Task<string> HandleAsync(TestCommandWithResult command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(command.Phrase);
    }
}
```

#### Command without a result:

```csharp
public record TestCommandWithoutResult(string Message) : ICommand;

public class TestCommandWithoutResultHandler : ICommandHandler<TestCommandWithoutResult>
{
    public Task HandleAsync(TestCommandWithoutResult command, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Executing command: {command.Message}");
        return Task.CompletedTask;
    }
}
```

### 4. Dispatching Commands and Queries

Retrieve the appropriate dispatcher from the service provider and execute commands or queries:

```csharp
var serviceProvider = services.BuildServiceProvider();

// Query Execution
var queryDispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();
string queryResult = await queryDispatcher.QueryAsync<TestQuery, string>(new TestQuery("test123"));
Console.WriteLine(queryResult); // Output: test123

// Command Execution with Result
var commandDispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
string commandResult = await commandDispatcher.ExecuteAsync<TestCommandWithResult, string>(new TestCommandWithResult("test123"));
Console.WriteLine(commandResult); // Output: test123

// Command Execution without Result
await commandDispatcher.ExecuteAsync(new TestCommandWithoutResult("Executing CQRS command"));
```

## Testing

The following unit tests validate the correct behavior of TioneCQRS:

```csharp
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
```


---

Feel free to contribute or report issues on [GitHub](https://github.com/jimbrzk/TioneCQRS)!