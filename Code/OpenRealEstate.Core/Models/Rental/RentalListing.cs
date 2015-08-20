using System;

namespace OpenRealEstate.Core.Models.Rental
{
    public class RentalListing : Listing
    {
        private DateTime? _availableOn;
        private BuildingDetails _buildingDetails;
        private RentalPricing _pricing;
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

        public bool IsPricingModified { get; set; }

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
            return string.Format("Rental >> {0}", base.ToString());
        }

        public void CopyOverNewData(RentalListing newRentalListing)
        {
            if (newRentalListing == null)
            {
                throw new ArgumentNullException("newRentalListing");
            }

            base.CopyOverNewData(newRentalListing);

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
                    Pricing.CopyOverNewData(newRentalListing.Pricing);
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
                    BuildingDetails.CopyOverNewData(newRentalListing.BuildingDetails);
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

            if (Pricing != null)
            {
                Pricing.ClearAllIsModified();
            }

            IsPropertyTypeModified = false;
            IsAvailableOnModified = false;
            IsPricingModified = false;
            IsBuildingDetailsModified = false; 
        }
    }
}