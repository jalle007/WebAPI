using System.Linq;
using Newtonsoft.Json;
using ProductAPI.Models;
using ProductAPI.Models.Responses;
using RestSharp;

namespace ProductAPI.Repository {
  public class KixifyRepository {
    string kixifyAPI = "https://kixify-util-services.azurewebsites.net/api/product/getbysku";

    public KixifyRepository () { }

    public async System.Threading.Tasks.Task<KOFProductResponse> GetProductAsync (string sku) {
      var client = new RestClient(kixifyAPI);
      var request = new RestRequest(Method.POST);
      request.AddHeader("cache-control", "no-cache");
      request.AddHeader("content-type", "application/json");
      request.AddHeader("authorization", "Basic dXRpbHNlcnZpY2U6dXRpbGF1dGg3Nzc=");
      string param2 = "{\r\n    \"sku\" : \"xxx\"\r\n}".Replace("xxx", sku);
      request.AddParameter("application/json", param2, ParameterType.RequestBody);
      var response = client.Execute(request);

      KOFProductResponse product = JsonConvert.DeserializeObject<KOFProductResponse>(response.Content);
      return product;
      }

    //var httpClinet = new HttpClient();
    //httpClinet.DefaultRequestHeaders.Accept.Clear();
    //httpClinet.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //var requestUrl = $"{kixifyAPI}api/GetImages/{sku}/popular/1/25";
    //var response = await httpClinet.GetAsync(requestUrl);
    //var data = (await response.Content.ReadAsStringAsync());


    public Platform GetSingle (int id) {
      return null;
      }

    public IQueryable<Platform> GetAll () {
      return null;
      }

    }
  }
