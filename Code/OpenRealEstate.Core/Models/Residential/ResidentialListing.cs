using System;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        private DateTime? _auctionOn;
        private BuildingDetails _buildingDetails;
        private string _councilRates;
        private PropertyType _propertyType;
        private SalePricing _salePricing;

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

        public bool IsPricingModified { get; private set; }

        public DateTime? AuctionOn
        {
            get { return _auctionOn; }
            set
            {
                _auctionOn = value;
                IsAuctionOnModified = true;
            }
        }

        public bool IsAuctionOnModified { get; private set; }

        public string CouncilRates
        {
            get { return _councilRates; }
            set
            {
                _councilRates = value;
                IsCouncilRatesModified = true;
            }
        }

        public bool IsCouncilRatesModified { get; private set; }

        public BuildingDetails BuildingDetails
        {
            get { return _buildingDetails; }
            set
            {
                _buildingDetails = value;
                IsBuildingDetailsModified = true;
            }
        }

        public bool IsBuildingDetailsModified { get; private set; }

        public override string ToString()
        {
            return string.Format("Residential >> {0}", base.ToString());
        }

        public void CopyOverNewData(ResidentialListing newResidentialListing)
        {
            if (newResidentialListing == null)
            {
                throw new ArgumentNullException("newResidentialListing");
            }

            base.CopyOverNewData(newResidentialListing);

            if (newResidentialListing.IsPropertyTypeModified)
            {
                PropertyType = newResidentialListing.PropertyType;
            }

            if (newResidentialListing.IsPricingModified)
            {
                if (newResidentialListing.Pricing == null)
                {
                    Pricing = null;
                }
                else
                {
                    if (Pricing == null)
                    {
                        Pricing = new SalePricing();
                    }
                    Pricing.CopyOverNewData(newResidentialListing.Pricing);
                    IsPricingModified = true;
                }
            }

            if (newResidentialListing.IsAuctionOnModified)
            {
                AuctionOn = newResidentialListing.AuctionOn;
            }

            if (newResidentialListing.IsCouncilRatesModified)
            {
                CouncilRates = newResidentialListing.CouncilRates;
            }

            if (newResidentialListing.IsBuildingDetailsModified)
            {
                if (newResidentialListing.BuildingDetails == null)
                {
                    BuildingDetails = null;
                }
                else
                {
                    if (BuildingDetails == null)
                    {
                        BuildingDetails = new BuildingDetails();
                    }
                    BuildingDetails.CopyOverNewData(newResidentialListing.BuildingDetails);
                    IsBuildingDetailsModified = true;
                }
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (Pricing != null)
            {
                Pricing.ClearAllIsModified();
            }

            if (BuildingDetails != null)
            {
                BuildingDetails.ClearAllIsModified();
            }

            IsPropertyTypeModified = false;
            IsPricingModified = false;
            IsAuctionOnModified = false;
            IsCouncilRatesModified = false;
            IsBuildingDetailsModified = false;
        }
    }
}