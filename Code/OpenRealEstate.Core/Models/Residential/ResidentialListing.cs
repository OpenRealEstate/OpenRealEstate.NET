using System;

namespace OpenRealEstate.Core.Models.Residential
{
    public class ResidentialListing : Listing
    {
        private DateTime? _auctionOn;
        private BuildingDetails _buildingDetails;
        private bool _isBuildingDetailsModified;
        private string _councilRates;
        private PropertyType _propertyType;
        private SalePricing _salePricing;
        private bool _isPricingModified;

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

        public void Copy(ResidentialListing newResidentialListing)
        {
            if (newResidentialListing == null)
            {
                throw new ArgumentNullException("newResidentialListing");
            }

            if (!newResidentialListing.IsModified)
            {
                return;
            }

            base.Copy(newResidentialListing);

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

                    if (newResidentialListing.Pricing.IsModified)
                    {
                        Pricing.Copy(newResidentialListing.Pricing);
                    }

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

                    if (newResidentialListing.BuildingDetails.IsModified)
                    {
                        BuildingDetails.Copy(newResidentialListing.BuildingDetails);
                    }

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
            IsPricingModified = false;

            if (BuildingDetails != null)
            {
                BuildingDetails.ClearAllIsModified();
            }
            IsBuildingDetailsModified = false;

            IsPropertyTypeModified = false;
            IsAuctionOnModified = false;
            IsCouncilRatesModified = false;
        }
    }
}