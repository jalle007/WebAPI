using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProductAPI.Models;

namespace ProductAPI.Controllers {

  //PostImage controller
  [Route("api/[controller]")]
  [Consumes("application/json","multipart/form-data")]
  public class PostImageController : Controller {
    private readonly ProductLikesContext _context;
    private IHostingEnvironment _environment;

        public PostImageController (ProductLikesContext context, IHostingEnvironment environment) {
        _context = context;
         _environment = environment;
        }
      
        [HttpPost("upload/{userId}/{productCode}/{platformId}/{deviceType}/{deviceId}")]
        public async Task<IActionResult> Upload(IFormFile file,string userId, int productCode, int platformId, string deviceType, string deviceId)
        {
                var uploads = Path.Combine(_environment.WebRootPath, "images");
                string fileName = "product"+productCode+"-"+Path.GetRandomFileName()+"-"+ file.FileName;

                
                 using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                      await file.CopyToAsync(fileStream);
                      fileStream.Flush();
                }

                //now save to database
                var img = new Image { 
                                      Picture=fileName,
                                      DeviceId=deviceId,
                                      DeviceType=deviceType,
                                      ProductId=productCode,
                                      UserId=userId,
                };
                _context.Image.Add(img);
                _context.SaveChanges();

                return Ok(new { Name = fileName});
        }

       

       ///ToDo : Azure 
       //public async Task<string> UploadFileAsBlob(Stream stream, string filename)
       // {//_configuration["ConnectionString:StorageConnectionString"]
       //     CloudStorageAccount storageAccount = CloudStorageAccount.Parse("");

       //     // Create the blob client.
       //     CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

       //     // Retrieve a reference to a container.
       //     CloudBlobContainer container = blobClient.GetContainerReference("profileimages");

       //     CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

       //     await blockBlob.UploadFromStreamAsync(stream);

       //     stream.Dispose();
       //     return blockBlob?.Uri.ToString();
       // }
 
    }
  }
