# Introduction

This project is a .NET 7 library that provides a wrapper around the [Microsoft Dataverse Web API](https://docs.microsoft.com/en-us/powerapps/developer/data-platform/webapi/overview-web-api) to make it easier to use in .NET applications. The library is inspired by [WebAPIService class library](https://github.com/microsoft/PowerApps-Samples/tree/master/dataverse/webapi/C%23-NETx/WebAPIService).

## Features
- **Authentication**: The library supports authentication to Dataverse using [OAuth 2.0 Client Credential](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/authenticate-oauth).
- **Compose ODATA Query**: The library simplifies development by composing right query using ODATA specification.
- **Convert Response to POCO class objects**: The library provides mechanism to convert the API response to collection of class objects.
- **CRUD operations**: Supports Create, Read, Update and Delete operations on Dataverse Tables.
- **Batch**: Supports Dataverse *batch* endpoint. Execute multiple CUD operations as part of a single batch request.
- **Changeset**: Create changeset during batch request. Changeset is a group of operations that are executed as a single unit of work. If any operation in the changeset fails, all operations in the changeset fail.
- **Entity Metadata**: Get Metadata information for all entities.
- **Resilience and Fault Handling**: The library provides mechanism to retry failed requests. The retry mechanism is based on [Polly](https://github.com/App-vNext/Polly).


## Running the Sample
1. [Register an Azure AD Application](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/use-single-tenant-server-server-authentication), add it to your Dataverse instance as [application user](https://learn.microsoft.com/en-us/power-platform/admin/manage-application-users#create-an-application-user) and give it at least [Basic User role](https://learn.microsoft.com/en-us/power-platform/admin/manage-application-users#manage-roles-for-an-application-user).
2. Fill in the `Dataverse` section in `appsettings.json` with the values for your Dataverse instance.
```csharp
  "Dataverse": {
    "ClientId": "YOUR_CLIENT_ID",
    "ClientSecret": "YOUR_SECRET",
    "TenantId": "TENANT_ID",
    "Scope": "https://YOUR_ORG.api.crm.dynamics.com/.default",
    "BaseUrl": "https://YOUR_ORG.api.crm.dynamics.com"
  }
```
3. Build and Run the application.

Head over to [samples](/samples) folder to learn more.


## Use SDK in your own app
1. Reference `src/Dataverse.RestClient` project in your solution.
1. Setup authentication by registering a `ConfidentialClientAuthDelegatingHandler` in your application. Provide the `ClientId`, `ClientSecret`, `TenantId` and `Scope` in the configuration. The application must be added as a user of the Dataverse instance.
```csharp
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
```
3. Register `DataverseClient` as `HttpClient`. Pass the DataverseBaseUrl as part without forward slash at the end. For example, `https://myorg.api.crm.dynamics.com`.
```csharp
builder.Services.AddHttpClient<IDataverseClient, DataverseClient>((httpClient, sp) =>
{
    return new DataverseClient(httpClient, new DataverseClientOptions()
    {
        DataverseBaseUrl = builder.Configuration["Dataverse:BaseUrl"],
    });
}).AddHttpMessageHandler<ConfidentialClientAuthDelegatingHandler>();
```
4. Inject `IDataverseClient` in your application and use it to interact with Dataverse.
```csharp
public class AccountRepository
    {
        private readonly IDataverseClient dataverseClient;

        public AccountController(IDataverseClient dataverseClient)
        {
            this.dataverseClient = dataverseClient;
        }

        public async Task<ResponseAccount> GetById(Guid id)
        {
            var accounts = await this.dataverseClient.ListAsync(
                "accounts",
                itemId: id,
                withAnnotations: true,
                convert: (e, _) => e.Deserialize<ResponseAccount>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            return accounts.First();
        }
```

## Details
Below are the descriptions on some of the important classes exposed by the library. Look at the `Samples` section for more details on these classes.

### DataverseClient
`DataverseClient` is the main class that provides methods to interact with Dataverse. It is a wrapper around `HttpClient` and provides methods to execute CRUD requests to Dataverse.

### ConfidentialClientAuthDelegatingHandler
`ConfidentialClientAuthDelegatingHandler` is a `DelegatingHandler` that is used to authenticate requests to Dataverse. It uses `Microsoft.Identity.Client` to get access token for the application.

### BatchOperation
`BatchOperation` is a class that represents a single operation in a batch request. It provides methods to create a batch request with multiple operations.

### ChangeSet
Any number of operations can be grouped into a changeset. If any operation in the changeset fails, all operations in the changeset fail. `ChangeSet` is a class that represents a group of operations that are executed as a single unit of work.

### JsonArrayResponse
`JsonArrayResponse` represents the response from Dataverse when the response is an array of JSON objects.
