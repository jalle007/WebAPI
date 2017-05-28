using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ProductAPI.Models;
using ProductAPI.Repository;

namespace ProductAPI.Controllers {

  //PostImage controller
  [Route("api/[controller]")]
  [Consumes("application/json", "multipart/form-data")]
  public class PostImageController : Controller {
    public static IConfigurationRoot Configuration { get; set; }
    private IHostingEnvironment _environment;
   private MyRepository _repository;

    public PostImageController (ProductLikesContext context, IHostingEnvironment environment) {
      _environment = environment;
      _repository = new MyRepository(context);
      }

    //azure upload 
    [HttpPost("upload/{userId}/{productCode}/{platformId}/{deviceType}/{deviceId}")]
    public async Task<IActionResult> UploadFileAsBlob (IFormFile file, string userId, int productCode, int platformId, string deviceType, string deviceId, string title, string description) {
    
      var uploads = Path.Combine(_environment.WebRootPath, "images");
      string fileName = "product" + productCode + "-" + Path.GetRandomFileName() + "-" + file.FileName;

      string connStorage = Startup.connectionStrings.connStorage;

      CloudBlockBlob blockBlob;
      CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connStorage);

      // Retrieve a reference to a container.
      CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
      CloudBlobContainer container = blobClient.GetContainerReference("images");
      blockBlob = container.GetBlockBlobReference(fileName);

      using (var fileStream = file.OpenReadStream()) {
        await blockBlob.UploadFromStreamAsync(fileStream);
        }

      string imageUrl = blockBlob?.Uri.ToString();
      var img = new Image {
        Picture = imageUrl,
        DeviceId = deviceId,
        DeviceType = deviceType,
        ProductId = productCode,
        UserId = userId,
        Title=title,
        Description=description
        };

      //saving image to a database
      _repository._images.AddOrUpdate(img);

      return Ok(new { Name = imageUrl });
      }

    }
  }
