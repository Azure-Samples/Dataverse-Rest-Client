# WebAPI Sample
This sample application demonstrates how to use the Dataverse.RestClient library to interact with Dataverse Web API.

## Running the sample
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

## Overview
### Models
The Models folder contains the POCO classes for the entities in the Dataverse instance. The `Account` class is used as an example in the sample application. Since the JSON between request and response from Dataverse is different, the `RequestAccount` and `ResponseAccount` classes are used to convert the JSON to POCO class objects.

### Controllers
The `AccountController` uses the `IDataverseClient` to interact with Dataverse. The `IDataverseClient` is injected in the constructor of the `AccountController` using the Dependency Injection framework. The `IDataverseClient` is configured in the `Program.cs` file.

The `AccountController` demonstrates the following operations:
- List all accounts: Demonstrates how to use the `ListAsync` method to list all accounts and converting response to `ResponseAccount` object.
- Get account by Id: Demonstrates how to use the `ListAsync` method to get an account by primary key.
- Create account: Demonstrates how to use the `PostAsync` method to create an account.
- AssociateContact: Associate a contact to an account. Demonstrates how to use the `PatchAsync` method to create an association between two entities.
- SearchAccountByName: Use filter parameter in the `ListAsync` method to search for an account by name.
- SearchAccountByContactName: Use filter parameter in the `ListAsync` method to search for an account by contact name. Demonstrates how to use the `$expand` parameter to expand the contact entity and filter the parent entity by a property of the child entity.
- BatchInsert: Demonstrates use of `BatchOperation` to insert multiple records in a single request.