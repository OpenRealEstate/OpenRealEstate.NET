# THIS REPO IS NOW OBSOLETE AND REPLACED BY A NEWER REPO: https://github.com/OpenRealEstate
### Please refer to that main repo for all the latest code and sub-repo's for this project.
### This repo will be nuked in sometime in the future as all this code has already been migrated and to avoid confusion.

---

# OpenRealEstate.NET 

|            | Production | Dev |
| ----------:| ---------- | --- |
|            | [![Build status](https://ci.appveyor.com/api/projects/status/hdaer866pn98ly6k/branch/master?svg=true)](https://ci.appveyor.com/project/PureKrome/openrealestate-net-wof7s) | [![Build status](https://ci.appveyor.com/api/projects/status/hdaer866pn98ly6k?svg=true)](https://ci.appveyor.com/project/PureKrome/openrealestate-net) |
| Core       | [![NuGet Badge](https://buildstats.info/nuget/OpenRealEstate.Core?includePreReleases=true)](https://www.nuget.org/packages/OpenRealEstate.Core/) | [![MyGet Badge](https://buildstats.info/myget/openrealestate/OpenRealEstate.Core)](https://www.myget.org/feed/openrealestate/package/nuget/OpenRealEstate.Core) |
| Services   | [![NuGet Badge](https://buildstats.info/nuget/OpenRealEstate.Services?includePreReleases=true)](https://www.nuget.org/packages/OpenRealEstate.Services/) | [![MyGet Badge](https://buildstats.info/myget/openrealestate/OpenRealEstate.Services)](https://www.myget.org/feed/openrealestate/package/nuget/OpenRealEstate.Services) |
| Validation | [![NuGet Badge](https://buildstats.info/nuget/OpenRealEstate.Validation?includePreReleases=true)](https://www.nuget.org/packages/OpenRealEstate.Validation/) | [![MyGet Badge](https://buildstats.info/myget/openrealestate/OpenRealEstate.Validation)](https://www.myget.org/feed/openrealestate/package/nuget/OpenRealEstate.Validation) |
| FakeData   | N.A. | [![MyGet Badge](https://buildstats.info/myget/openrealestate/OpenRealEstate.FakeData)](https://www.myget.org/feed/openrealestate/package/nuget/OpenRealEstate.FakeData) |




---

This library is an easy to use .NET Client api to help parse OpenRealEstate schema/data.

The library offers three packages:
- `Core` : all the models that represent listing types.
- `Services`: the services that convert other listing formats to this OpenRealestate format.
- `Validation`: validation logic for a listing. Eg. Latitude/Longitudes are legit, price is valid, etc.
- `FakeData`: some hardcoded and random fake data, to help for testing and/or seeding your dev/test databases.

---
##Installation

![](http://i.imgur.com/LKwcJ2U.png)
![](http://i.imgur.com/LdCxDle.png)
![](http://i.imgur.com/8YMVxXE.png)  
TODO: Add CLI for FakeData.
---

###Pricing Notes
The [`SalePricing`](https://github.com/OpenRealEstate/OpenRealEstate.NET/blob/master/Code/OpenRealEstate.Core/Models/SalePricing.cs) class contains two main properties which define the sale price of a listing
- `SalePrice`
- `SalePriceText`

The `SalePrice` is the listing price which should never been displayed to the public. This value is intended for services (like websites) to flter listings by a price.

The `SalePriceText` value is what should be displayed to the public. If you wish to display the actual sale price, then copy this value into both properties.

Examples
- `SalePrice: 200000 | SalePriceText: null` : Nothing will be displayed to the public.
- `SalePrice: 200000 | SalePriceText: Offers about 200K` : The text _Offers about 200K_ should be displayed to the public.
 
---
[![I'm happy to accept tips](http://img.shields.io/gittip/purekrome.svg?style=flat-square)](https://gratipay.com/PureKrome/)  
![Lic: MIT](http://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)
