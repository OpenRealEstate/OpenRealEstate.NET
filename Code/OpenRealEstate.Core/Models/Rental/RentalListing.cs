using System;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalListing : Listing
    {
        private DateTime? _availableOn;
        private BuildingDetails _buildingDetails;
        private bool _isBuildingDetailsModified;
        private RentalPricing _pricing;
        private bool _isPricingModified;
        private PropertyType _propertyType;

        public PropertyType PropertyType
        {
            get { return _propertyType; }
            set
            {
                _propertyType = value;
                IsPropertyTypeModified = true;
            }
        }

        public bool IsPropertyTypeModified { get; set; }

        public DateTime? AvailableOn
        {
            get { return _availableOn; }
            set
            {
                _availableOn = value;
                IsAvailableOnModified = true;
            }
        }

        public bool IsAvailableOnModified { get; set; }

        public RentalPricing Pricing
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
                       IsAvailableOnModified ||
                       IsPricingModified ||
                       IsBuildingDetailsModified;
            }
        }

        public override string ToString()
        {
            return string.Format("Rental >> {0}", base.ToString());
        }

        public void Copy(RentalListing newRentalListing)
        {
            if (newRentalListing == null)
            {
                throw new ArgumentNullException("newRentalListing");
            }

            base.Copy(newRentalListing);

            if (newRentalListing.IsPropertyTypeModified)
            {
                PropertyType = newRentalListing.PropertyType;
            }

            if (newRentalListing.IsAvailableOnModified)
            {
                AvailableOn = newRentalListing.AvailableOn;
            }

            if (newRentalListing.IsPricingModified)
            {
                if (newRentalListing.Pricing == null)
                {
                    Pricing = null;
                }
                else
                {
                    if (Pricing == null)
                    {
                        Pricing = new RentalPricing();
                    }

                    if (newRentalListing.Pricing.IsModified)
                    {
                        Pricing.Copy(newRentalListing.Pricing);
                    }

                    IsPricingModified = true;
                }
            }

            if (newRentalListing.IsBuildingDetailsModified)
            {
                if (newRentalListing.BuildingDetails == null)
                {
                    BuildingDetails = null;
                }
                else
                {
                    if (BuildingDetails == null)
                    {
                        BuildingDetails = new BuildingDetails();
                    }

                    if (newRentalListing.BuildingDetails.IsModified)
                    {
                        BuildingDetails.Copy(newRentalListing.BuildingDetails);
                    }

                    IsBuildingDetailsModified = true;
                }
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (BuildingDetails != null)
            {
                BuildingDetails.ClearAllIsModified();
            }
            IsBuildingDetailsModified = false; 

            if (Pricing != null)
            {
                Pricing.ClearAllIsModified();
            }
            IsPricingModified = false;

            IsPropertyTypeModified = false;
            IsAvailableOnModified = false;
        }
    }
}