using System;

namespace OpenRealEstate.Core.Models.Land
{
    public class LandListing : Listing
    {
        private DateTime? _auctionOn;
        private CategoryType _categoryType;
        private string _councilRates;
        private LandEstate _estate;
        private bool _isEstateModified;
        private SalePricing _pricing;
        private bool _isPricingModified;

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

        public LandEstate Estate
        {
            get { return _estate; }
            set
            {
                _estate = value;
                IsEstateModified = true;
            }
        }

        public bool IsEstateModified {
            get
            {
                return _isEstateModified ||
                       (Estate != null &&
                        Estate.IsModified);
            }
            set { _isEstateModified = value; }
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

        public override bool IsModified {
            get
            {
                return base.IsModified ||
                       IsCategoryTypeModified ||
                       IsPricingModified ||
                       IsAuctionOnModified ||
                       IsEstateModified ||
                       IsCouncilRatesModified;
            }
        }

        public override string ToString()
        {
            return string.Format("Land >> {0}", base.ToString());
        }

        public void Copy(LandListing newLandListing)
        {
            if (newLandListing == null)
            {
                throw new ArgumentNullException("newLandListing");
            }

            if (!newLandListing.IsModified)
            {
                return;
            }

            base.Copy(newLandListing);

            if (newLandListing.IsCategoryTypeModified)
            {
                CategoryType = newLandListing.CategoryType;
            }

            if (newLandListing.IsPricingModified)
            {
                if (newLandListing.Pricing == null)
                {
                    Pricing = null;
                }
                else
                {
                    if (Pricing == null)
                    {
                        Pricing = new SalePricing();
                    }

                    if (newLandListing.IsPricingModified)
                    {
                        Pricing.Copy(newLandListing.Pricing);
                    }

                    IsPricingModified = true;
                }
            }

            if (newLandListing.IsAuctionOnModified)
            {
                AuctionOn = newLandListing.AuctionOn;
            }

            if (newLandListing.IsEstateModified)
            {
                if (newLandListing.Estate == null)
                {
                    Estate = null;
                }
                else
                {
                    if (Estate == null)
                    {
                        Estate = new LandEstate();
                    }
                    Estate.Copy(newLandListing.Estate);
                    IsEstateModified = true;
                }
            }

            if (newLandListing.IsCouncilRatesModified)
            {
                CouncilRates = newLandListing.CouncilRates;
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (Pricing != null)
            {
                Pricing.ClearAllIsModified();
            }

            if (Estate != null)
            {
                Estate.ClearAllIsModified();
            }

            IsCategoryTypeModified = false;
            IsPricingModified = false;
            IsAuctionOnModified = false;
            IsCouncilRatesModified = false;
            IsEstateModified = false;
        }
    }
}