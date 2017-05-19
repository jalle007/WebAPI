using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ProductAPI.Models;

namespace ProductAPI.Controllers {

  //PostImage controller
  [Route("api/[controller]")]
  [Consumes("application/json", "multipart/form-data")]
  public class PostImageController : Controller {
    private readonly ProductLikesContext _context;
    public static IConfigurationRoot Configuration { get; set; }
    private IHostingEnvironment _environment;

    public PostImageController (ProductLikesContext context, IHostingEnvironment environment) {
      _context = context;
      _environment = environment;
      }


    //azure upload********************************************************************
    [HttpPost("upload/{userId}/{productCode}/{platformId}/{deviceType}/{deviceId}")]
    public async Task<IActionResult> UploadFileAsBlob (IFormFile file, string userId, int productCode, int platformId, string deviceType, string deviceId) {

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

      string fileUrl = blockBlob?.Uri.ToString();
      Save2DB(userId, productCode, deviceType, deviceId, fileUrl);

      return Ok(new { Name = fileUrl });
      }

    private void Save2DB (string userId, int productCode, string deviceType, string deviceId, string fileName) {
      //now save to database
      var img = new Image {
        Picture = fileName,
        DeviceId = deviceId,
        DeviceType = deviceType,
        ProductId = productCode,
        UserId = userId,
        };
      _context.Image.Add(img);
      _context.SaveChanges();
      }

    //local upload
    //[HttpPost("upload/{userId}/{productCode}/{platformId}/{deviceType}/{deviceId}")]
    public async Task<IActionResult> Upload (IFormFile file, string userId, int productCode, int platformId, string deviceType, string deviceId) {
      var uploads = Path.Combine(_environment.WebRootPath, "images");
      string fileName = "product" + productCode + "-" + Path.GetRandomFileName() + "-" + file.FileName;


      using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create)) {
        await file.CopyToAsync(fileStream);
        fileStream.Flush();
        }

      Save2DB(userId, productCode, deviceType, deviceId, fileName);

      return Ok(new { Name = fileName });
      }

    }
  }
