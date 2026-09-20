using System.Diagnostics;
using NBomber.CSharp;


var httpClient = new HttpClient();
var scenario = Scenario.Create("aspnet_ping", async context =>
    {
        var response = await httpClient.GetAsync("http://localhost:5039/api/products/ping");

        return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(Simulation.Inject(rate: 500, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)));
 NBomberRunner.RegisterScenarios(scenario).Run();


