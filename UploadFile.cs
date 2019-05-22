using System;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;


namespace blobby
{
    //This class cheks if the environment variable is set
    public class UploadFile
    {
        CloudStorageAccount storageAccount = null;
        CloudStorageAccount GetStorageAccount()
        {
            return storageAccount;
        }


        CloudBlobContainer cloudBlobContainer = null;
        CloudBlobContainer GetCloudBlobContainer()
        {
            return cloudBlobContainer;
        }
        string sourceFile;
        public void setSource(string _sourceFile)
        {
            sourceFile = _sourceFile;
        }


        string destinationFile;
        public void setDest(string _destinationFile)
        {
            destinationFile = _destinationFile;
        }

        string storageConnectionString = Environment.GetEnvironmentVariable("storageconnectionstring");
        public string GetStorageConnectionString()
        {
            return storageConnectionString;
        }


        public async Task Upload()
        {   
            if(CloudStorageAccount.TryParse(storageConnectionString,out storageAccount))
            {
                try
                {
                    
                    
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    cloudBlobContainer = cloudBlobClient.GetContainerReference("newcontainer" + Guid.NewGuid().ToString());
                    await cloudBlobContainer.CreateAsync();
                    Console.WriteLine("Created container '{0}'", cloudBlobContainer.Name);
                    Console.WriteLine();
                    BlobContainerPermissions permissions = new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    };
                    await cloudBlobContainer.SetPermissionsAsync(permissions);
                    //till here the container is created ..now you were put a file that one
                    /*
                     string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                     string localFileName = "QuickStart_" + Guid.NewGuid().ToString() + ".txt";
                     sourceFile = Path.Combine(localPath, localFileName);
                     // Write text to the file.
                     File.WriteAllText(sourceFile, "Hello, World!");
                     */
                     //here you can tell what path it has to take  the
                    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string localFileName = sourceFile;
                    sourceFile = Path.Combine(localPath, localFileName);
                    Console.WriteLine("Temp file = {0}", sourceFile);
                    Console.WriteLine("Uploading to Blob storage as blob '{0}'", localFileName);
                    Console.WriteLine();

                    //this were very important 
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
                    await cloudBlockBlob.UploadFromFileAsync(sourceFile);

                    /*
                    // List the blobs in the container.
                    Console.WriteLine("Listing blobs in container.");
                    BlobContinuationToken blobContinuationToken = null;
                    do
                    {
                        var resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                        // Get the value of the continuation token returned by the listing call.
                        blobContinuationToken = resultSegment.ContinuationToken;
                        foreach (IListBlobItem item in resultSegment.Results)
                        {
                            Console.WriteLine(item.Uri);
                        }
                    } while (blobContinuationToken != null); 
                    Console.WriteLine();
                    */
                    /*
                    destinationFile = sourceFile.Replace(".txt", "_DOWNLOADED.txt");
                    Console.WriteLine("Downloading blob to {0}", destinationFile);
                    Console.WriteLine();
                    await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);
                    */
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error returned from the service: {0}", ex.Message);
                }
               /* finally
                {
                    Console.WriteLine("Press any key to delete the sample files and example container.");
                    Console.ReadLine();
                    Console.WriteLine("Deleting the container and any blobs it contains");
                    if (cloudBlobContainer != null)
                    {
                        await cloudBlobContainer.DeleteIfExistsAsync();
                    }
                    Console.WriteLine("Deleting the local source file and local downloaded files");
                    Console.WriteLine();
                    File.Delete(sourceFile);
                    File.Delete(destinationFile);
                }
                */
            }
            else
            {
                Console.WriteLine(
                   "A connection string has not been defined in the system environment variables. " +
                   "Add a environment variable named 'storageconnectionstring' with your storage " +
                   "connection string as a value.");
            }
        }

    }
}
