using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kixify.Dashboard.Data;
using Kixify.Dashboard.Models;
using Kixify.OnFeet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kixify.OnFeet.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Kixify.Dashboard.Util;
using System.IO;
using Kixify.OnFeet.Dal.Entity;

namespace Kixify.Dashboard.Controllers {
  [Authorize]
  public class ImagesController : Controller {
    private readonly ApplicationDbContext _context;
    private IHostingEnvironment _env;
    public const string sAll = "-- ALL --";
    List<string> skus;
    private readonly ImageService _imageService;
    private readonly SkuService _skuService;

    public ImagesController (ApplicationDbContext context, IHostingEnvironment env, ImageService imageService, SkuService skuService) {
      _context = context;
      _imageService = imageService;
      _skuService = skuService;
      _env = env;
      }

    [HttpPost]
    public async Task<JsonResult> Autocomplete (string Prefix) {
      if (skus.IsNull()) skus = await GetSKUsAsync(false);
      var res = skus.Where(sku => sku.ToLower().StartsWith(Prefix.ToLower())).ToList();

      return Json(res);
      }

    [HttpGet]
    public async Task<IActionResult> Create () {
      return View();
      }

    [HttpGet]
    public async Task<IActionResult> Upload (IList<IFormFile> files) {
      if (ViewBag.SKUs == null) {
        ViewBag.SKUs = await GetSKUsAsync(false);
        // ViewBag.SKUs.Insert(0, "new");
        }

      return View();
      }

    [HttpPost]
    public async Task<IActionResult> Upload ([FromForm]ImageBindingModel model) {
      if (!ModelState.IsValid) {
        return Ok(new ApiResponse() {
          Success = false,
          Message = "Invalid data",
          Data = ModelState
          });
        }

      var sku1 = (string)Request.Form["sku1"];
      if (sku1 != "") model.Sku = sku1;

      model.Sku = model.Sku.ToAlphaNumericOnly();

      var productDetails = _skuService.GetProductDetailsBySku(model.Sku);
      SkuServiceProduct skuServiceProduct = null;

      if (!productDetails.Success || productDetails.Data == null || !productDetails.Data.Any()) {
        skuServiceProduct = null;
        } else {
        skuServiceProduct = productDetails.Data.First();

        if (skuServiceProduct.Sku.ToLower() != model.Sku.ToAlphaNumericOnly().ToLower()) {
          skuServiceProduct = null;
          }
        }

      if (Request.Form.Files == null || !Request.Form.Files.Any()) {
        return Ok(new ApiResponse() {
          Success = false,
          Message = "No files in your request"
          });
        }

      var file = Request.Form.Files[0];

      var uploadedPath = await _imageService.UplaodImage(Path.GetExtension(file.FileName), file.ContentType, file.OpenReadStream());

      var image = await _imageService.AddImage(new Image() {
        Created = DateTimeOffset.UtcNow,
        Sku = model.Sku,
        EventId = skuServiceProduct?.Id,
        Platform = model.Platform,
        Title = skuServiceProduct == null ? model.Title : skuServiceProduct.Title,
        DeviceToken = model.DeviceToken,
        DeviceType = model.DeviceType,
        ProfileUrl = model.ProfileUrl,
        UserId = model.UserId,
        ImageUrl = uploadedPath,
        Username = model.Username,
        });

      return RedirectToAction("Index", "Images");
      }

    // GET: Images
    [HttpGet]
    public async Task<IActionResult> Index () {
      return View(await GetImages());
      }

    [HttpPost]
    public async Task<IActionResult> Index (string sku) {
      var sku1 = (string)Request.Form["sku1"];
      if (sku == sAll && sku1 == "") return RedirectToAction("Index", "Images");

      return View(await GetImages(sku1 != "" ? sku1 : sku));
      }


    private async Task<List<ImagesViewModel>> GetImages (string sku = null) {
      if (ViewBag.SKUs == null)
        ViewBag.SKUs = await GetSKUsAsync();

      var imageResponse = await _imageService.GetImages("chronological", 0, 1, 100, sku);

      var imagesView = new List<ImagesViewModel>() { };
      foreach (var img in imageResponse.Images) {
        imagesView.Add(new ImagesViewModel { Image = img, Checked = false });
        }

      ViewBag.Count = imageResponse.Count;
      ViewBag.TotalPages = imageResponse.TotalPages;

      return imagesView;
      }

    // POST: Images/Delete/5
    [HttpGet]
    [Route("images/delete/{id}")]
    public async Task<IActionResult> Delete (long id) {
      var response = await _imageService.DeleteImage(id);

      return RedirectToAction("Index");
      }

    // POST: Images/Delete/selected  List<ImageItemResponse> model
    [HttpPost]
    public async Task<IActionResult> DeleteSelected (List<ImagesViewModel> model) {

      var itemId = Request.Form["item.Image.Id"];
      var itemChecked = Request.Form["item.Checked"];

      for (int i = 0; i < itemChecked.Count; i++) {
        if (Convert.ToBoolean(itemChecked[i])) {
          // call del api
          var response = await _imageService.DeleteImage(Convert.ToInt32(itemId[i]));
          }
        }

      return RedirectToAction("Index");
      }

    public async Task<List<string>> GetSKUsAsync (bool all = true) {
      var images = await _imageService.GetImages("chronological", 0, 1, int.MaxValue, null);
      var SKUs = images.Images.Select(l => l.Sku).Distinct().ToList();

      if (all) SKUs.Insert(0, sAll);
      return SKUs;
      }
    }
  }
