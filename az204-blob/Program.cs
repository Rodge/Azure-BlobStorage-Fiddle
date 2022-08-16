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

    //Create a unique name for the container
    string containerName = "wtblob" + Guid.NewGuid().ToString();

    // Create the container and return a container client object
    BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
    Console.WriteLine("A container named '" + containerName + "' has been created. " +
        "\nTake a minute and verify in the portal." + 
        "\nNext a file will be created and uploaded to the container.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();


    // Create a local file in the ./data/ directory for uploading and downloading
    string localPath = "./data/";
    string fileName = "wtfile" + Guid.NewGuid().ToString() + ".txt";
    string localFilePath = Path.Combine(localPath, fileName);

    // Write text to the file
    await File.WriteAllTextAsync(localFilePath, "Hello, World!");

    // Get a reference to the blob
    BlobClient blobClient = containerClient.GetBlobClient(fileName);

    Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

    // Open the file and upload its data
    using FileStream uploadFileStream = File.OpenRead(localFilePath);
    await blobClient.UploadAsync(uploadFileStream, true);

    Console.WriteLine("\nThe file was uploaded. We'll verify by listing" + 
            " the blobs next.");
    Console.WriteLine("Press 'Enter' to continue.");
    Console.ReadLine();
}

public sealed class Keys
{
    public StorageAccount StorageAccount { get; set; } = null!;
}

public sealed class StorageAccount
{
    public string ConnectionString { get; set; } = null!;
}