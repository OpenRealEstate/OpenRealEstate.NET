#OpenRealEstate.NET 
[![Build status](https://ci.appveyor.com/api/projects/status/hdaer866pn98ly6k)](https://ci.appveyor.com/project/PureKrome/openrealestate-net)
- Core[![](http://img.shields.io/nuget/v/OpenRealEstate.Core.svg?style=flat-square)](https://www.nuget.org/packages/OpenRealEstate.Core) ![](http://img.shields.io/nuget/dt/OpenRealEstate.Core.svg?style=flat-square) 
- Services [![](http://img.shields.io/nuget/v/OpenRealEstate.Services.svg?style=flat-square)](https://www.nuget.org/packages/OpenRealEstate.Services) ![](http://img.shields.io/nuget/dt/OpenRealEstate.Services.svg?style=flat-square)
---

This library is an easy to use .NET Client api to help parse OpenRealEstate schema/data.

The library offers two packages:
- `Core` : all the models that represent listing types.
- `Services`: the services that convert other listing formats to this OpenRealestate format.

---
## Installation

TODO:  image for nuget pics, etc.    
 !! split into two parts, one per nuget package

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