using System;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralListing : Listing
    {
        private DateTime? _auctionOn;
        private BuildingDetails _buildingDetails;
        private CategoryType _categoryType;
        private string _councilRates;
        private SalePricing _pricing;
        private RuralFeatures _ruralFeatures;

        public CategoryType CategoryType
        {
            get { return _categoryType; }
            set
            {
                _categoryType = value;
                IsCategoryTypeModified = true;
            }
        }

        public bool IsCategoryTypeModified { get; set; }

        public SalePricing Pricing
        {
            get { return _pricing; }
            set
            {
                _pricing = value;
                IsPricingModified = true;
            }
        }

        public bool IsPricingModified { get; set; }

        public DateTime? AuctionOn
        {
            get { return _auctionOn; }
            set
            {
                _auctionOn = value;
                IsAuctionOnModified = true;
            }
        }

        public bool IsAuctionOnModified { get; set; }

        public RuralFeatures RuralFeatures
        {
            get { return _ruralFeatures; }
            set
            {
                _ruralFeatures = value;
                IsRuralFeaturesModified = true;
            }
        }

        public bool IsRuralFeaturesModified { get; set; }

        public string CouncilRates
        {
            get { return _councilRates; }
            set
            {
                _councilRates = value;
                IsCouncilRatesModified = true;
            }
        }

        public bool IsCouncilRatesModified { get; set; }

        public BuildingDetails BuildingDetails
        {
            get { return _buildingDetails; }
            set
            {
                _buildingDetails = value;
                IsBuildingDetailsModified = true;
            }
        }

        public bool IsBuildingDetailsModified { get; set; }

        public override string ToString()
        {
            return string.Format("Rural >> {0}", base.ToString());
        }

        public void Copy(RuralListing newRuralListing)
        {
            if (newRuralListing == null)
            {
                throw new ArgumentNullException("newRuralListing");
            }

            base.Copy(newRuralListing);

            if (newRuralListing.IsCategoryTypeModified)
            {
                CategoryType = newRuralListing.CategoryType;
            }

            if (newRuralListing.IsPricingModified)
            {
                if (newRuralListing.Pricing == null)
                {
                    Pricing = null;
                }
                else
                {
                    if (Pricing == null)
                    {
                        Pricing = new SalePricing();
                    }
                    Pricing.Copy(newRuralListing.Pricing);
                    IsPricingModified = true;
                }
            }

            if (newRuralListing.IsAuctionOnModified)
            {
                AuctionOn = newRuralListing.AuctionOn;
            }

            if (newRuralListing.IsRuralFeaturesModified)
            {
                if (newRuralListing.RuralFeatures == null)
                {
                    RuralFeatures = null;
                }
                else
                {
                    if (RuralFeatures == null)
                    {
                        RuralFeatures = new RuralFeatures();
                    }
                    RuralFeatures.Copy(newRuralListing.RuralFeatures);
                    IsRuralFeaturesModified = true;
                }
            }

            if (newRuralListing.IsCouncilRatesModified)
            {
                CouncilRates = newRuralListing.CouncilRates;
            }

            if (newRuralListing.IsBuildingDetailsModified)
            {
                if (newRuralListing.BuildingDetails == null)
                {
                    BuildingDetails = null;
                }
                else
                {
                    if (BuildingDetails == null)
                    {
                        BuildingDetails = new BuildingDetails();
                    }
                    BuildingDetails.Copy(newRuralListing.BuildingDetails);
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

            if (RuralFeatures != null)
            {
                RuralFeatures.ClearAllIsModified();
            }

            if (BuildingDetails != null)
            {
                BuildingDetails.ClearAllIsModified();
            }

            IsCategoryTypeModified = false;
            IsPricingModified = false;
            IsAuctionOnModified = false;
            IsCouncilRatesModified = false;
            IsRuralFeaturesModified = false;
            IsBuildingDetailsModified = false;
        }
    }
}