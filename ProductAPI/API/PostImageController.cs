using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ProductAPI.Models;
using ProductAPI.Models.Responses;
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

    //upload to Azure
    [HttpPost("upload/{userId}/{sku}/{platformId}/{deviceType}/{deviceId}")]
    public async Task<IActionResult> UploadFileAsBlob (IFormFile file, string userId, string sku, int platformId, string deviceType, string deviceId, string title, string description, string Username, string ProfilePicUrl) {

      //error checking
      string error = string.Empty;
      if (file == null) error = "File missing.";
      var product = _repository._products.GetSingle(sku);

      if (product == null)   //add SKU to the table and retrieve it again
      {
        _repository._products.AddOrUpdate(new Product {
          Name = "",
          SKU = sku
          });

        product = _repository._products.GetSingle(sku);
        }

      if (!_repository._platform.Exists(platformId)) error += "Platform ID not found. ";
      KOFProductResponse productKOF = _repository._kixify.GetProductAsync(product.SKU).Result;
      if(productKOF==null)
          error += "SKU not found. ";
      if (error != "")
        return Ok(new { Error = true, Message = error });

      var uploads = Path.Combine(_environment.WebRootPath, "images");
      string fileName = Path.GetRandomFileName() + "_" + file.FileName;

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

      var productId = productKOF.Data.FirstOrDefault().Id;
      string imageUrl = blockBlob?.Uri.ToString();

      var img = new Models.Image{
        Picture = imageUrl,
        DeviceId = deviceId,
        DeviceType = deviceType,
        Title = productKOF.Data.FirstOrDefault().Title,
        ProductId = productId,
        Product = product,
        UserId = userId,
        ProfilePicUrl = ProfilePicUrl,
        Username = Username
        };

      //saving image to a database
      _repository._images.AddOrUpdate(img);

      //update product ID to match KOF Id
      img.ProductId= productId;

      var response = new {
        Data = img,
        Error = false,
        Message = ""
        };

      return Ok(response);
      }

    }
  }
