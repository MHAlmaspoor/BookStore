// using NBomber.CSharp;

// var baseUrl = "http://localhost:5039";

// using var httpClient = new HttpClient
// {
//     BaseAddress = new Uri(baseUrl),
//     Timeout = TimeSpan.FromSeconds(60)
// };

// var scenario = Scenario.Create("redis_direct", async context =>
//     {
//         var response = await httpClient.GetAsync("/api/redis-test");

//         return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//     })
//     .WithWarmUpDuration(TimeSpan.FromSeconds(10))
//     .WithLoadSimulations(Simulation.Inject(rate: 500, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));

// NBomberRunner
//     .RegisterScenarios(scenario)
//     .Run();
