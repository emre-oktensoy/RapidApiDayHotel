using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RapidApiDayHotel.Models
{
    public class SearchHotelViewModel
    {
        public string Location { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int Adults { get; set; }
        public int RoomQty { get; set; }
    }

    public class DestinationApiResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<DestinationViewModel> Data { get; set; }
    }

    public class DestinationViewModel
    {
        [JsonProperty("dest_id")]
        public string DestId { get; set; }

        [JsonProperty("search_type")]
        public string SearchType { get; set; }
    }

    public class HotelApiResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public HotelData Data { get; set; }
    }

    public class HotelData
    {
        [JsonProperty("hotels")]
        public List<HotelTestViewModel> Hotels { get; set; }
    }

    public class HotelTestViewModel
    {
        [JsonProperty("hotel_id")]
        public int HotelId { get; set; }

        [JsonProperty("accessibilityLabel")]
        public string AccessibilityLabel { get; set; }

        [JsonProperty("property")]
        public HotelProperty Property { get; set; }
    }
     
    public class HotelProperty
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("photoUrls")]
        public List<string> PhotoUrls { get; set; }

        [JsonProperty("priceBreakdown")]
        public PriceBreakdown PriceBreakdown { get; set; }

        [JsonProperty("reviewScore")]
        public double? ReviewScore { get; set; }

        [JsonProperty("reviewScoreWord")]
        public string ReviewScoreWord { get; set; }

        [JsonProperty("checkin")]
        public Chekin Checkin { get; set; }

        [JsonProperty("checkout")]
        public Chekout Checkout { get; set; }

    }

    public class Chekin
    {
        [JsonProperty("untilTime")]
        public string UntilTime { get; set; }

        [JsonProperty("fromTime")]
        public string FromTime { get; set; }
    }
    public class Chekout
    {
        [JsonProperty("untilTime")]
        public string UntilTime { get; set; }

        [JsonProperty("fromTime")]
        public string FromTime { get; set; }
    }

    public class PriceBreakdown
    {
        [JsonProperty("grossPrice")]
        public Price GrossPrice { get; set; }

        [JsonProperty("excludedPrice")]
        public Price ExcludedPrice { get; set; }
    }

    public class Price
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}