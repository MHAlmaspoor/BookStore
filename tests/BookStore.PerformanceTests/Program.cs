// using NBomber.CSharp;
// using NBomber.Http.CSharp;

// var httpClient = Http.CreateDefaultClient();
// const string productId= "01a08ca5-8913-707f-814b-40e3ea524c51";

// var scenario = Scenario.Create("get_product_cache_hit", async context =>
// {
//    var request = Http.CreateRequest("GET", $"http://localhost:5039/api/products/{productId}");
//    var response = await Http.Send(httpClient, request);
//    return response;
// })
// .WithWarmUpDuration(TimeSpan.FromSeconds(10))
// .WithLoadSimulations(Simulation.Inject(500, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));

// NBomberRunner.RegisterScenarios(scenario).Run();


