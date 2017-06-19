using System;
using System.IO;
using System.Linq;
using Kixify.OnFeet.Dal.Entity;
using Kixify.OnFeet.Service;
using Kixify.OnFeet.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Kixify.OnFeet.WebApi.Util;

namespace Kixify.OnFeet.WebApi.Controllers
{

    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        private readonly SkuService _skuService;

        public ImageController(ImageService imageService, SkuService skuService)
        {
            _imageService = imageService;
            _skuService = skuService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string order, string sku = null, int page = 1, int pageSize = 100)
        {
            var images = await _imageService.GetImages(order, page, pageSize, sku);
            return Ok(new ApiResponse()
            {
                Success = true,
                Data = images
            });
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromForm]ImageBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "Invalid data",
                    Data = ModelState
                });
            }

            model.Sku = model.Sku.ToAlphaNumericOnly();

            var productDetails = _skuService.GetProductDetailsBySku(model.Sku);

            if (!productDetails.Success || productDetails.Data == null || !productDetails.Data.Any())
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "Unable to get the product details from sku " + model.Sku
                });
            }

            var skuServiceProduct = productDetails.Data.First();

            if (skuServiceProduct.Sku.ToLower() != model.Sku.ToLower())
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "Unable to get the product details from sku " + model.Sku
                });
            }

            if (Request.Form.Files == null || !Request.Form.Files.Any())
            {
                return Ok(new ApiResponse()
                {
                    Success = false,
                    Message = "No files in your request"
                });
            }

            var file = Request.Form.Files[0];

            var uploadedPath = await _imageService.UplaodImage(Path.GetExtension(file.FileName), file.ContentType, file.OpenReadStream());

            var image = await _imageService.AddImage(new Image()
            {
                Created = DateTimeOffset.UtcNow,
                Sku = model.Sku,
                Platform = model.Platform,
                Title = skuServiceProduct.Title,
                DeviceToken = model.DeviceToken,
                DeviceType = model.DeviceType,
                ProfileUrl = model.ProfileUrl,
                UserId = model.UserId,
                ImageUrl = uploadedPath,
                Username = model.Username,
            });

            return Ok(new ApiResponse()
            {
                Success = true,
                Data = image
            });

        }

        [HttpPost]
        [Route("Like")]
        public async Task<IActionResult> Like([FromBody]LikeBindingModel model)
        {
            await _imageService.Like(model.ImageId, model.UserId, model.Platform, model.Like);
            return Ok(new ApiResponse()
            {
                Success = true
            });
        }
    }
}