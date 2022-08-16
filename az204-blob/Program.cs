using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Azure Blob Storage exercise\n");

// Run the examples asynchronously, wait for the results before proceeding
ProcessAsync().GetAwaiter().GetResult();

Console.WriteLine("Press enter to exit the sample application.");
Console.ReadLine();

static async Task ProcessAsync()
{
    // Build a config object, using env vars and JSON providers.
    var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environmentName}.json", true, true)
        .AddEnvironmentVariables()
        .Build();

    // Get values from the config given their key and their target type.
    Keys keys = config.GetRequiredSection("Keys").Get<Keys>();

    // Write the values to the console.
    Console.WriteLine($"Keys.StorageAccount.ConnectionString = {keys.StorageAccount.ConnectionString}");
    // Copy the connection string from the portal in the variable below.
    string storageConnectionString = keys.StorageAccount.ConnectionString;

    // Create a client that can authenticate with a connection string
    BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);

    // COPY EXAMPLE CODE BELOW HERE

}

public sealed class Keys
{
    public StorageAccount StorageAccount { get; set; } = null!;
}

public sealed class StorageAccount
{
    public string ConnectionString { get; set; } = null!;
}