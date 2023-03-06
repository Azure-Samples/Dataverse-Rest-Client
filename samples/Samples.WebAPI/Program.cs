using Dataverse.RestClient;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dataverseClientApplication = ConfidentialClientApplicationBuilder
                .Create(builder.Configuration["Dataverse:ClientId"])
                .WithClientSecret(builder.Configuration["Dataverse:ClientSecret"])
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{builder.Configuration["Dataverse:TenantId"]}/"))
                .Build();
dataverseClientApplication.AddInMemoryTokenCache();
builder.Services.AddSingleton(dataverseClientApplication);
builder.Services.AddTransient(
    sp => new ConfidentialClientAuthDelegatingHandler(sp.GetRequiredService<IConfidentialClientApplication>(), new[] { builder.Configuration["Dataverse:Scope"] })
);

builder.Services.AddHttpClient<IDataverseClient, DataverseClient>((httpClient, sp) =>
{
    return new DataverseClient(httpClient, new DataverseClientOptions()
    {
        DataverseBaseUrl = builder.Configuration["Dataverse:BaseUrl"],
    });
}).AddHttpMessageHandler<ConfidentialClientAuthDelegatingHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
