using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using SperanzaPizzaApi.Data;
using SperanzaPizzaApi.Models;

namespace SperanzaPizzaApi.Services.Addresses
{
    public class AddressParserService
    {
        private dbPizzaContext _context;
        private double longitudeMin { get; }
        private double longitudeMax { get; }
        private double lattitudeMin { get; }
        private double lattitudeMax { get; }

        public AddressParserService (dbPizzaContext context)
        {
            _context = context;
            
            longitudeMin = 22.18;
            longitudeMax = 22.72;
            lattitudeMin = 53.39;
            lattitudeMax = 53.88;

        }

        public async Task ParseAddressfromXml()
        {
            if (_context.DmCities.Any())
                return;
            // Regularly load file from https://download.geofabrik.de/europe/poland/podlaskie.html
            var filename = "podlaskie.osm";
            
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory, filename);

            var purchaseOrder = XElement.Load(purchaseOrderFilepath);
            var result = new List<Address>();

            var partNos =  (from item in purchaseOrder.Descendants("node")
                            select item).Where(x => !x.IsEmpty).ToList();
            
            foreach (var item in partNos)
            {
                double.TryParse(item.Attribute("lon")?.Value, out var longitude);
                double.TryParse(item.Attribute("lat")?.Value, out var lattitude);
                if (longitude < longitudeMin || longitude > longitudeMax || lattitude < lattitudeMin || lattitude > lattitudeMax)
                    continue;               

                var tags = item.Elements("tag").Where(x => x.Attribute("k").Value.ToString().Contains("addr:")).ToList();
                if (tags.Count == 0) continue;
            
                var city = tags.FirstOrDefault(x  => x.Attribute("k").Value.ToString() == "addr:city");
                var street = tags.FirstOrDefault(x  => x.Attribute("k").Value.ToString() == "addr:street");
                
                if (city == null || street == null) continue;
                var housenumber = tags.FirstOrDefault(x  => x.Attribute("k").Value.ToString() == "addr:housenumber");
                var postCode = tags.FirstOrDefault(x  => x.Attribute("k").Value.ToString() == "addr:postcode");
                                

                result.Add(new Address{
                    City = city == null ? string.Empty : city.Attribute("v").Value.ToString(),
                    Street = street == null ? string.Empty : street.Attribute("v").Value.ToString(),
                    HouseNumber = housenumber == null ? string.Empty : housenumber.Attribute("v").Value.ToString(),
                    PostCode = postCode == null ? string.Empty : postCode.Attribute("v").Value.ToString(),
                    Longitude = longitude.ToString(),
                    Latitude = lattitude.ToString()
                });
            }
            var addresses = result.GroupBy(x => x.City).Select(x => x);
            foreach (var item in addresses)
            {
                DmCity newCity = new DmCity{ CityName = item.Key };
                _context.Add(newCity);
                await _context.SaveChangesAsync();
                
                var streets = item.GroupBy(x => x.Street).Select(x => x);
                foreach(var street in streets)
                {
                    DmStreet newStreet = new DmStreet {
                            CityId = newCity.Id,
                            StreetName = street.Key 
                        };
                    _context.DmStreets.Add(newStreet);
                    await _context.SaveChangesAsync();

                    // housenumbers:
                    List<DmAddress> houseList = new List<DmAddress>();
                    foreach(var house in street)
                    {
                        houseList.Add(new DmAddress{
                            StreetId = newStreet.Id,
                            HouseNumber = house.HouseNumber,
                            PostCode = house.PostCode,
                            Longitude = decimal.Parse(house.Longitude),
                            Lattitude = decimal.Parse(house.Latitude)
                        });
                    }
                    _context.DmAddresses.AddRange(houseList);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private class Address {
            public string City { get; set;}
            public string Street { get; set; }
            public string HouseNumber { get; set; }
            public string PostCode { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
        }
    }
}