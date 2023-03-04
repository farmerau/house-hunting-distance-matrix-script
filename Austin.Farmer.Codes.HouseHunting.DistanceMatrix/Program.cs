// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Austin.Farmer.Codes.HouseHunting.DistanceMatrix;

Regex regex = new("\"(.+)\", \"(.+)\", \"(.+)\"", RegexOptions.Compiled);

string googleMapsApiKey = File.ReadLines("api.key").First();

Dictionary<string, PointOfInterest> locationsByAddress = new();
Dictionary<string, IList<PointOfInterest>> locationsByCategory = new();
Dictionary<PointOfInterest, long> travelTimesInSeconds = new();
HashSet<string> destinations = new();

foreach (string line in File.ReadLines("input.csv"))
{
    MatchCollection matches = regex.Matches(line);
    Match match = matches[0];
    PointOfInterest poi = new(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
    locationsByAddress[poi.Address] = poi;
    destinations.Add(poi.Address);
    locationsByCategory.TryAdd(poi.Category, new List<PointOfInterest>());
    locationsByCategory[poi.Category].Add(poi);
}

Console.Write("What is the target address?\n");
string originAddress = Console.ReadLine();

using (HttpClient client = new())
{
    foreach (IEnumerable<string> destinationSet in destinations.Batch(20))
    {
        IList<string> destinationList = destinationSet.ToList();
        string destinationsArg = string.Join("|", destinationList);
        string originsArg = HttpUtility.UrlEncode(originAddress);
        HttpResponseMessage response = await  client.GetAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?destinations={destinationsArg}&origins={originsArg}&units=imperial&key={googleMapsApiKey}");

        GoogleMapsDistanceMatrixResponse? deserializedResponse = await JsonSerializer.DeserializeAsync<GoogleMapsDistanceMatrixResponse>(response.Content.ReadAsStream(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        for (int i = 0; i < destinationList.Count; i++)
        {
            string destinationAddress = destinationList[i];
            GoogleMapsDistanceMatrixRowElement destinationResponse = deserializedResponse.Rows[0].Elements[i];
            PointOfInterest destination = locationsByAddress[destinationAddress];
            Console.WriteLine($"It would take approximately {destinationResponse.Duration.Text} to travel to {destination.Description}.");
            travelTimesInSeconds[destination] = destinationResponse.Duration.Value;
        }
    }
}

Func<long, string> timeInSecondsToEnglish = time =>
{
    TimeSpan seconds = TimeSpan.FromSeconds(time);
    string result = string.Empty;
    if (seconds.Hours > 0)
    {
        result += seconds.ToString("hh") + " hours ";
    }

    result += seconds.ToString("mm") + " minutes";
    return result;
};

Console.WriteLine("<==== Time by Category ====>");
foreach ((string? category, IList<PointOfInterest>? locations) in locationsByCategory)
{
    long totalTime = 0,
        numberOfLocations = locations.Count,
        maxTime = 0,
        minTime = travelTimesInSeconds[locations.First()];

    foreach (PointOfInterest poi in locations)
    {
        long travelTime = travelTimesInSeconds[poi];
        totalTime += travelTime;
        maxTime = Math.Max(travelTime, maxTime);
        minTime = Math.Min(travelTime, minTime);
    }

    long average = totalTime / numberOfLocations;
    
    Console.WriteLine($"\n*** {category} ***");
    Console.WriteLine($"Min travel time: {timeInSecondsToEnglish(minTime)}");
    Console.WriteLine($"Average travel time: {timeInSecondsToEnglish(average)}");
    Console.WriteLine($"Max travel time: {timeInSecondsToEnglish(maxTime)}");
}

