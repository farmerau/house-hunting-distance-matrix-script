# Disclaimer
This is **NOT** intended in any way to be a productionized tool. There are lots of changes here I would want to make prior to making it generally usable.

# House Hunting: Distance Matrix

When buying a home, it's important to know how the home sits relative to the things you love. However, keeping track of the addresses of everything and running Google Maps searches can be tedious. Consequently, I've created this script to handle the tedium for you.

## Usage

### Dependencies

#### APIs

This script depends on the [Google Maps Distance Matrix REST API](https://developers.google.com/maps/documentation/distance-matrix/overview), which does all of the hard work. Google being Google, however, requires an API key so that they get paid for their data and processing power.

If you don't already have an API Key, please read [their documentation](https://developers.google.com/maps/documentation/distance-matrix/get-api-key) to do so.

#### Framework

This script was written in C# and depends on the .NET Core 7 framework. Read [Microsoft's documentation](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) for installing this onto your system.

### Setup

To make the script maximally useful, we need to perform a (near) one-time setup by editing the following files:

- `api.key`
    This file should contain the value of your API Key mentioned in the Dependencies.APIs section above. Nothing more, nothing less.
- `input.csv`
    As denoted by the file extension, this file should contain comma separated values containing the following:
    - Name / Description
        This could be something like "Work", "Mom's House", "The Library", etc.
    - Category
        The script assumes that you might want to group certain destinations, like "Friends", "Restaurants", "Grocery Stores", etc.
    - Address
        This should be the value of the address supplied to the Google Maps API.

### Execution

Inside the `Austin.Farmer.Codes.HouseHunting.DistanceMatrix` subfolder, run `dotnet run`. The script will prompt you for the address. Type it in, press enter.