using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SwapStff
{
    public class BlobCloudService
    {
        readonly CloudBlobContainer _container;
        public BlobCloudService()
        {
            string Connection = System.Configuration.ConfigurationManager.AppSettings["StorageCS"].ToString();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Connection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container. 
            _container = blobClient.GetContainerReference("productimages");
            // Create the container if it doesn't already exist.
            _container.CreateIfNotExists();
            //By default, the new container is private and you must specify your storage access key to download blobs from this container
            _container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            
        }
        public string UploadBlobImage(string blobName, byte[] imageBytes)
        {
            string ImageURL = "";
            try
            {
                CloudBlockBlob blobref = _container.GetBlockBlobReference(blobName);
                using (var stream = new MemoryStream(imageBytes, writable: false))
                {
                    blobref.UploadFromStream(stream);
                }
                ImageURL = blobref.Uri.ToString();
            }
            catch
            {
                ImageURL = "";
            }

            return ImageURL;
        }
        public Boolean DeleteImageFromBlob(string Url)
        {
            Boolean Status = false;
            try
            {
                //Full Url : https://swapstffstorage123.blob.core.windows.net/productimages/image_36221769-6927-4546-bb8f-730d3e46bddd.jpg
                //Url Name : image_36221769-6927-4546-bb8f-730d3e46bddd.jpg
                Url = Url.Substring(Url.IndexOf("image_"));

                CloudBlockBlob blob = _container.GetBlockBlobReference(Url);
                blob.DeleteIfExists();
                Status = true;
            }
            catch
            {
                Status = false;
            }

            return Status;
        }
    }
}