using System;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        private readonly DateTimeNullableNotified _auctionOn;
        private readonly InstanceObjectNotified<BuildingDetails> _buildingDetails;
        private bool _isBuildingDetailsModified;
        private readonly StringNotified _councilRates;
        private PropertyType _propertyType;
        private SalePricing _salePricing;
        private bool _isPricingModified;

        public ResidentialListing()
        {
            _auctionOn = new DateTimeNullableNotified("AuctionOn");
            _auctionOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _councilRates = new StringNotified("CouncilRates");
            _councilRates.PropertyChanged += ModifiedData.OnPropertyChanged;

            _buildingDetails = new InstanceObjectNotified<BuildingDetails>("BuildingDetails");
            _buildingDetails.PropertyChanged += ModifiedData.OnPropertyChanged;
        }

        public PropertyType PropertyType
        {
            get { return _propertyType; }
            set
            {
                _propertyType = value;
                IsPropertyTypeModified = true;
            }
        }

        public bool IsPropertyTypeModified { get; private set; }

        public SalePricing Pricing
        {
            get { return _salePricing; }
            set
            {
                _salePricing = value;
                IsPricingModified = true;
            }
        }

        public bool IsPricingModified {
            get
            {
                return _isPricingModified ||
                       (Pricing != null &&
                        Pricing.IsModified);
            }
            set { _isPricingModified = value; }
        }

        public DateTime? AuctionOn
        {
            get { return _auctionOn.Value; }
            set
            {
                _auctionOn.Value = value;
            }
        }

        [Obsolete]
        public bool IsAuctionOnModified { get; private set; }

        public string CouncilRates
        {
            get { return _councilRates.Value; }
            set { _councilRates.Value = value; }
        }

        public bool IsCouncilRatesModified { get; private set; }

        public BuildingDetails BuildingDetails
        {
            get { return _buildingDetails.Value; }
            set { _buildingDetails.Value = value; }
        }

        [Obsolete]
        public bool IsBuildingDetailsModified
        {
            get
            {
                return _isBuildingDetailsModified ||
                       (BuildingDetails != null &&
                        BuildingDetails.IsModified);
            }
            set { _isBuildingDetailsModified = value; }
        }

        public override bool IsModified
        {
            get
            {
                return base.IsModified ||
                       IsPropertyTypeModified ||
                       IsPricingModified ||
                       IsAuctionOnModified ||
                       IsCouncilRatesModified ||
                       IsBuildingDetailsModified;
            }
        }

        public override string ToString()
        {
            return string.Format("Residential >> {0}", base.ToString());
        }

        public void CopyX<T>(T newResidentialListing) where T : class, new()
        {
            //if (newResidentialListing == null)
            //{
            //    throw new ArgumentNullException("newResidentialListing");
            //}

            //ModifiedData.Copy(newResidentialListing, this);

//            base.Copy(newResidentialListing);
            

            //if (!newResidentialListing.IsModified)
            //{
            //    return;
            //}

            //base.Copy(newResidentialListing);

            //if (newResidentialListing.IsPropertyTypeModified)
            //{
            //    PropertyType = newResidentialListing.PropertyType;
            //}

            //if (newResidentialListing.IsPricingModified)
            //{
            //    if (newResidentialListing.Pricing == null)
            //    {
            //        Pricing = null;
            //    }
            //    else
            //    {
            //        if (Pricing == null)
            //        {
            //            Pricing = new SalePricing();
            //        }

            //        if (newResidentialListing.Pricing.IsModified)
            //        {
            //            Pricing.Copy(newResidentialListing.Pricing);
            //        }

            //        IsPricingModified = true;
            //    }
            //}

            //if (newResidentialListing.IsAuctionOnModified)
            //{
            //    AuctionOn = newResidentialListing.AuctionOn;
            //}

            //if (newResidentialListing.IsCouncilRatesModified)
            //{
            //    CouncilRates = newResidentialListing.CouncilRates;
            //}

            //if (newResidentialListing.IsBuildingDetailsModified)
            //{
            //    if (newResidentialListing.BuildingDetails == null)
            //    {
            //        BuildingDetails = null;
            //    }
            //    else
            //    {
            //        if (BuildingDetails == null)
            //        {
            //            BuildingDetails = new BuildingDetails();
            //        }

            //        if (newResidentialListing.BuildingDetails.IsModified)
            //        {
            //            BuildingDetails.Copy(newResidentialListing.BuildingDetails);
            //        }

            //        IsBuildingDetailsModified = true;
            //    }
            //}
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();
            if (_buildingDetails.Value.IsModified)
            {
                _buildingDetails.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedProperties(new[]
            {
                "AuctionOn", 
                "CouncilRates"
            });
        }
    }
}