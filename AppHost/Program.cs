var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebAPI>("webapi");

builder.AddProject<Projects.GraphQL>("graphql");

builder.AddProject<Projects.EventDriven>("eventdriven");

builder.Build().Run();
