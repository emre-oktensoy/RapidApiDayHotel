using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RapidApiDayHotel.Models;

namespace RapidApiDayHotel.Controllers
{
    public class DefaultController : Controller
    {
        private readonly string _apiKey = "e1a5d17523msh0ca2ad180b79317p1740c3jsna7e56514cda4";
        private readonly string _apiHost = "booking-com15.p.rapidapi.com";

        public IActionResult Index()
        {

            return View(new SearchHotelViewModel
            {
                ArrivalDate = DateTime.Today.AddDays(1),
                DepartureDate = DateTime.Today.AddDays(3),
                Adults = 2,
                RoomQty = 1
            });
        }

        public IActionResult HotelDetail(int id)
        {

            var hotelListJson = HttpContext.Session.GetString("HotelList");

            if (string.IsNullOrEmpty(hotelListJson))
            {
                TempData["ErrorMessage"] = "Otel bilgisi bulunamadı. Lütfen aramayı tekrar yapın.";
                return RedirectToAction("SearchHotel");
            }

            var hotels = JsonConvert.DeserializeObject<List<HotelTestViewModel>>(hotelListJson);

            var hotel = hotels.FirstOrDefault(h => h.HotelId == id);
            if (hotel == null)
            {
                TempData["ErrorMessage"] = "Belirtilen otel bulunamadı.";
                return RedirectToAction("SearchHotel");
            }

            return View(hotel);
        }

        // Otel listesini getiren aksiyon
        public async Task<IActionResult> HotelList(string location, DateTime? arrivalDate, DateTime? departureDate, int adults, int roomQty)
        {
            if (string.IsNullOrEmpty(location) || !arrivalDate.HasValue || !departureDate.HasValue || adults < 1 || roomQty < 1)
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
                return RedirectToAction("SearchHotel");
            }

            try
            {
                var client = new HttpClient();

                // 1. Adım: searchDestination API'sini çağır
                var destRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={Uri.EscapeDataString(location)}"),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };

                string destId;
                string searchType;
                using (var destResponse = await client.SendAsync(destRequest))
                {
                    destResponse.EnsureSuccessStatusCode();
                    var destBody = await destResponse.Content.ReadAsStringAsync();
                    var destApiResponse = JsonConvert.DeserializeObject<DestinationApiResponse>(destBody);

                    if (destApiResponse?.Data == null || !destApiResponse.Data.Any())
                    {
                        TempData["ErrorMessage"] = "Belirtilen lokasyon için destinasyon bulunamadı.";
                        return RedirectToAction("SearchHotel");
                    }

                    var destination = destApiResponse.Data.First();
                    destId = destination.DestId;
                    searchType = destination.SearchType;
                }

                // 2. Adım: searchHotels API'sini çağır
                var hotelRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={destId}&search_type={searchType}&arrival_date={arrivalDate:yyyy-MM-dd}&departure_date={departureDate:yyyy-MM-dd}&adults={adults}&room_qty={roomQty}&currency_code=TRY"),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };

                using (var hotelResponse = await client.SendAsync(hotelRequest))
                {
                    hotelResponse.EnsureSuccessStatusCode();
                    var hotelBody = await hotelResponse.Content.ReadAsStringAsync();
                    var hotelApiResponse = JsonConvert.DeserializeObject<HotelApiResponse>(hotelBody);
                    //return View("HotelList", hotelApiResponse.Data.Hotels);


                    var hotels = hotelApiResponse.Data.Hotels;
                    // Otel listesini JSON olarak Session'a yaz
                    HttpContext.Session.SetString("HotelList", JsonConvert.SerializeObject(hotels));

                    return View("HotelList", hotels);
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Otel verileri alınırken bir hata oluştu: " + ex.Message;
                return RedirectToAction("SearchHotel");
            }
        }
    }
}
