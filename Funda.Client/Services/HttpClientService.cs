using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Funda.Client.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Funda.Client.Services
{
    public class HttpClientService : IHttpClientService
    {
        //using httpclient for consuming restful api
        //the object does not need to be disposed
        //it is intended to live as long as the application needs to make http requests
        private static readonly HttpClient _httpClient = new HttpClient();

        //to avoid being rejected by the api, the request id made after 605 milliseconds
        //at the best case it might send 60000/610 = 98 requests per minutes
        private static readonly int sleepTime = 610;
        public HttpClientService()
        {
            _httpClient.Timeout = new TimeSpan(0, 0, 60);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        public async Task Execute()
        {
            Console.WriteLine("Please wait until the API returns results...");
            var top10AllObjects = await GetTopRealEstateAgents("koop", "koop", 10);
            if(top10AllObjects.Count() > 0)
            {
                Console.WriteLine("Top 10 real estate agents with the most objects for sale in Amsterdam:");
                foreach (var agent in top10AllObjects)
                {
                    Console.WriteLine(agent.Id + "\t" + agent.Name + "\t" + agent.NumberOfItemsForSale);
                }
            }
            else
            {
                Console.WriteLine("The API call did not find any item.");
            }

            Console.WriteLine();
            Console.WriteLine("Please wait until the API returns results...");
            var top10ObjectsWithGarden = await GetTopRealEstateAgents("koop", "tuin", 10);
            if (top10ObjectsWithGarden.Count() > 0)
            {
                Console.WriteLine("Top 10 real estate agents with the most objects with a garden for sale in Amsterdam:");
                foreach (var agent in top10ObjectsWithGarden)
                {
                    Console.WriteLine(agent.Id + "\t" + agent.Name + "\t" + agent.NumberOfItemsForSale);
                }
            }
            else
            {
                Console.WriteLine("The API call did not find any item.");
            }
        }

        //this function receives as input three parameters and returns top real states with the most item for that type of sale
        //typeOfSale such as koop or huur
        //propertyType such as tuin
        //top number such as top 10 or 20
        public async Task<List<RealEstateAgents>> GetTopRealEstateAgents(string typeOfSale, string propertType, int topNumber)
        {
            var agents = new List<RealEstateAgents>();
            var request = GenerateRequestUri(typeOfSale, propertType, 1);

            var result = await CallApiEndPoint(request);
            //the first call is to get the first 25 items together with total number of items and total number of pages
            if(result != null && result.TotalNumberOfItems > 0)
            {
                //in case we have more than one page we call the api for other sets of data
                for (int i = 2; i <= result.TotalPages; i++)
                {
                    //calling to api happens every 610 milliseconds
                    Thread.Sleep(sleepTime);
                    request = GenerateRequestUri(typeOfSale, propertType, i);

                    var partialResult = await CallApiEndPoint(request);

                    //the property sets of other api calls are added to the first property set
                    result.PropertiesForSale.AddRange(partialResult.PropertiesForSale);
                }

                //calling the object to group the properties by real estate agents and sort them by number of items
                agents = GetTopRealEstateAgentsFromPropertyList(result.PropertiesForSale, topNumber);
            }
            return agents;
        }

        //this function receives a list of properties for sale and numbers to take and returns the top items of the list
        public List<RealEstateAgents> GetTopRealEstateAgentsFromPropertyList(List<PropertiesForSale> propertiesForSale, int topNumber)
        {
            var agents = new List<RealEstateAgents>();
            agents = (from property in propertiesForSale
                      group property by new
                      {
                          property.RealEstateAgentId,
                          property.RealEstateAgentName
                      } into groupedProperties
                      orderby groupedProperties.Count() descending, groupedProperties.Key.RealEstateAgentName
                      select new RealEstateAgents()
                      {
                          Id = groupedProperties.Key.RealEstateAgentId,
                          Name = groupedProperties.Key.RealEstateAgentName,
                          NumberOfItemsForSale = groupedProperties.Count()
                      }).Take(topNumber).ToList();

            return agents;
        }

        //this method receives the parametr and generate a valid request uri
        public Uri GenerateRequestUri(string typeOfSale, string propertyType, int pageNum)
        {
            string baseAddress = "https://partnerapi.funda.nl/feeds/Aanbod.svc/json/";
            string accessKey = "ac1b0b1572524640a0ecc54de453ea9f";
            string saleType = "/?type=" + typeOfSale;
            string searchQuery = "&zo=/amsterdam/" + propertyType;
            string pageNumber = "/&page=" + pageNum;
            string pageSize = "&pagesize=25";

            return new Uri(baseAddress + accessKey + saleType + searchQuery + pageNumber + pageSize);
        }

        //this method receives the request uri and makes the api call and returns the result after deserializing the object
        public async Task<DataObject?> CallApiEndPoint(Uri requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            JObject jsonObject = JObject.Parse(content);
            DataObject? responseDataObject = jsonObject.ToObject<DataObject>();
            if(responseDataObject != null)
                responseDataObject.TotalPages = (int)jsonObject.SelectToken("Paging.AantalPaginas");
            return responseDataObject;
        }
    }
}
