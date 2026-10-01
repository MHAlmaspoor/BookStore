// using System.Diagnostics;
// using NBomber.CSharp;
// using StackExchange.Redis;

// var connectionString = "localhost:6379";

// var multiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);
// var db = multiplexer.GetDatabase();

// var key = "product:01a08ca5-8913-707f-814b-40e3ea524c51";

// var scenario = Scenario.Create("redis_client_direct", async context =>
//     {
//         try
//         {
//             var value = await db.HashGetAsync(key, "data");

//             return value.HasValue ? Response.Ok() : Response.Fail();
//         }
//         catch (Exception ex)
//         {
//             return Response.Fail(message: ex.Message);
//         }
//     })
//     .WithWarmUpDuration(TimeSpan.FromSeconds(10))
//     .WithLoadSimulations(Simulation.Inject(rate: 500, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));

// NBomberRunner
//     .RegisterScenarios(scenario)
//     .Run();

// await multiplexer.CloseAsync();

