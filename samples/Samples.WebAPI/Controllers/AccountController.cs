using Dataverse.RestClient;
using Dataverse.RestClient.Model;
using Microsoft.AspNetCore.Mvc;
using Samples.WebAPI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

using RequestAccount = Samples.WebAPI.Models.Request.Account;
using ResponseAccount = Samples.WebAPI.Models.Response.Account;

namespace Samples.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IDataverseClient dataverseClient;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IDataverseClient dataverseClient, ILogger<AccountController> logger)
        {
            this.dataverseClient = dataverseClient;
            _logger = logger;
        }

        [HttpGet("GetById", Name = "GetById")]
        public async Task<ResponseAccount> GetById(Guid id)
        {
            var account = await this.dataverseClient.ListAsync(
                "accounts",
                new RequestOptions(itemId: id, withAnnotations: true),
                convert: (e, _) => e.Deserialize<ResponseAccount>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            return account.First();
        }

        [HttpGet("GetAllAccounts", Name = "GetAllAccounts")]
        public async Task<JsonArrayResponse<ResponseAccount?>> GetAllAccounts()
        {
            return await this.dataverseClient.ListAsync(
                "accounts",
                new RequestOptions() { WithAnnotations = true },
                convert: (e, _) => e.Deserialize<ResponseAccount>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }

        [HttpGet("SearchAccountByContactName", Name = "SearchAccountByContactName")]
        public async Task<JsonArrayResponse> SearchAccountByContactName(string primaryContactName) 
        {
            return await this.dataverseClient.ListAsync(
                "accounts",
                new RequestOptions(
                    expand: "primarycontactid", expandSelect: "fullname,contactid",
                    filter: $"startswith(primarycontactid/fullname, '{primaryContactName}')", withAnnotations: true));

        }

        [HttpGet("Create", Name = "Create")]
        public async Task<EntityReference> Create()
        {
            return await this.dataverseClient.PostAsync("accounts", JsonSerializer.Serialize(new RequestAccount() { Name = $"Test - {Random.Shared.Next(100)}" }, options: new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), null, null, default);
        }

        [HttpGet("AssociateContact")]
        public async Task<ResponseAccount?> AssociateContact()
        {
            var contacts = await this.dataverseClient.ListAsync("contacts");
            var contact = contacts.ToList()[Random.Shared.Next(contacts.Count())];

            var accounts = await this.dataverseClient.ListAsync<ResponseAccount>("accounts", convert: (e, _) => e.Deserialize<ResponseAccount>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            var account = accounts.First();
            this._logger.LogInformation($"Updating account {account.Name} with contact {contact.GetProperty("fullname")}");

            return await this.dataverseClient.PatchAsync<ResponseAccount>("accounts", JsonSerializer.Serialize(new RequestAccount() { PrimaryContact = contact.GetProperty("contactid").GetGuid().ToString() }, options: new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }), account.AccountId.ToString(), convert: e => e.Deserialize<ResponseAccount>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }

        [HttpGet("SearchAccountByName", Name = "SearchAccountByName")]
        public async Task<JsonArrayResponse<ResponseAccount?>> SearchAccountByName(string name)
        {
            return await this.dataverseClient.ListAsync("accounts", new RequestOptions(filter: $"startswith(name, '{name}')", withAnnotations: true), convert: (e, _) => e.Deserialize<ResponseAccount>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }

        [HttpGet("BatchInsert", Name = "BatchInsert")]
        public async Task<IEnumerable<BatchOperationResult?>> BatchInsert(int numberOfRecordsToCreate)
        {
            var batch = this.dataverseClient.CreateBatchOperation();
            foreach (var _ in Enumerable.Range(0, numberOfRecordsToCreate))
            {
                batch.AddCreate("accounts", JsonSerializer.Serialize(new RequestAccount() { Name = $"Test - {Random.Shared.Next(100)}" }, options: new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
            }
            return await batch.ProcessAsync();
        }
    }
}