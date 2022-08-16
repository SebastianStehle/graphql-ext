using GraphQL;
using GraphQL.DI;
using GraphQL.Server.Ui.Playground;
using GraphQLExample.Schema;
using GraphQLExample.Subscriptions;
using GraphQLExample.Test;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfigureExecution,
    Configure>();

builder.Services.AddSingleton<ISubscriptionService,
    SubscriptionService>();

builder.Services.TryAddSingleton<ISubscriptionEvaluator,
    DefaultSubscriptionEvaluator>();

builder.Services.TryAddSingleton<ISubscriptionSenderProvider,
    HostnameSubscriptionSenderProvider>();

builder.Services.TryAddSingleton<ISubscriptionTransport,
    InMemoryTransport>();

builder.Services.AddSingleton<TaskService>();

builder.Services.AddGraphQL(builder =>
{
    builder.AddAutoSchema<TestQuery>(s => s
        .WithMutation<TestMutations>()
        .WithSubscription<TestSubscriptions>());

    builder.AddSystemTextJson();
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.UseWebSockets();

app.UseGraphQL("/graphql");

app.UseGraphQLPlayground("/", new PlaygroundOptions
{
    GraphQLEndPoint = "/graphql",
    SubscriptionsEndPoint = "/graphql",
});

await app.RunAsync();