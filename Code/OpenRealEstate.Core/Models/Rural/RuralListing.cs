using System;

namespace OpenRealEstate.Core.Models.Rural
{
    public class RuralListing : Listing
    {
        private DateTime? _auctionOn;
        private BuildingDetails _buildingDetails;
        private bool _isBuildingDetailsModified;
        private CategoryType _categoryType;
        private string _councilRates;
        private SalePricing _pricing;
        private bool _isPricingModified;
        private RuralFeatures _ruralFeatures;
        private bool _isRuralFeaturesModified;

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

        public bool IsPricingModified
        {
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

        public bool IsRuralFeaturesModified
        {
            get
            {
                return _isRuralFeaturesModified ||
                       (RuralFeatures != null &&
                        RuralFeatures.IsModified);
            }
            set { _isRuralFeaturesModified = value; }
        }

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
                       IsCategoryTypeModified ||
                       IsPricingModified ||
                       IsAuctionOnModified ||
                       IsRuralFeaturesModified ||
                       IsCouncilRatesModified ||
                       IsBuildingDetailsModified;
            }
        }

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

                    if (newRuralListing.Pricing.IsModified)
                    {
                        Pricing.Copy(newRuralListing.Pricing);
                    }

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

                    if (newRuralListing.RuralFeatures.IsModified)
                    {
                        RuralFeatures.Copy(newRuralListing.RuralFeatures);
                    }

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

                    if (newRuralListing.BuildingDetails.IsModified)
                    {
                        BuildingDetails.Copy(newRuralListing.BuildingDetails);
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

            if (RuralFeatures != null)
            {
                RuralFeatures.ClearAllIsModified();
            }
            IsRuralFeaturesModified = false;

            if (BuildingDetails != null)
            {
                BuildingDetails.ClearAllIsModified();
            }
            IsBuildingDetailsModified = false;

            IsCategoryTypeModified = false;
            IsAuctionOnModified = false;
            IsCouncilRatesModified = false;
        }
    }
}