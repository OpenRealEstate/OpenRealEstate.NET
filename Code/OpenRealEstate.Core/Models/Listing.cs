using System;
using System.Collections.Generic;

namespace OpenRealEstate.Core.Models
{
    public abstract class Listing : AggregateRoot
    {
        private Address _address;
        private string _agencyId;
        private IList<ListingAgent> _agents;
        private DateTime _createdOn;
        private string _description;
        private Features _features;
        private IList<Media> _floorPlans;
        private IList<Media> _images;
        private IList<Inspection> _inspections;
        private LandDetails _landDetails;
        private IList<string> _links;
        private StatusType _statusType;
        private string _title;
        private IList<Media> _videos;

        public string AgencyId
        {
            get { return _agencyId; }
            set
            {
                _agencyId = value;
                IsAgencyIdModified = true;
            }
        }

        public bool IsAgencyIdModified { get; private set; }

        public StatusType StatusType
        {
            get { return _statusType; }
            set
            {
                _statusType = value;
                IsStatusTypeModified = true;
            }
        }

        public bool IsStatusTypeModified { get; private set; }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                _createdOn = value;
                IsCreatedOnModified = true;
            }
        }

        public bool IsCreatedOnModified { get; private set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                IsTitleModified = true;
            }
        }

        public bool IsTitleModified { get; private set; }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                IsDescriptionModified = true;
            }
        }

        public bool IsDescriptionModified { get; private set; }

        public Address Address
        {
            get { return _address; }
            set
            {
                _address = value;
                IsAddressModified = true;
            }
        }

        public bool IsAddressModified { get; private set; }

        public IList<ListingAgent> Agents
        {
            get { return _agents; }
            set
            {
                _agents = value;
                IsAgentsModified = true;
            }
        }

        public bool IsAgentsModified { get; private set; }

        public IList<Media> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                IsImagesModified = true;
            }
        }

        public bool IsImagesModified { get; private set; }

        public IList<Media> FloorPlans
        {
            get { return _floorPlans; }
            set
            {
                _floorPlans = value;
                IsFloorPlansModified = true;
            }
        }

        public bool IsFloorPlansModified { get; private set; }

        public IList<Media> Videos
        {
            get { return _videos; }
            set
            {
                _videos = value;
                IsVideosModified = true;
            }
        }

        public bool IsVideosModified { get; private set; }

        public IList<Inspection> Inspections
        {
            get { return _inspections; }
            set
            {
                _inspections = value;
                IsInspectionsModified = true;
            }
        }

        public bool IsInspectionsModified { get; private set; }

        public LandDetails LandDetails
        {
            get { return _landDetails; }
            set
            {
                _landDetails = value;
                IsLandDetailsModified = true;
            }
        }

        public bool IsLandDetailsModified { get; private set; }

        public Features Features
        {
            get { return _features; }
            set
            {
                _features = value;
                IsFeaturesModified = true;
            }
        }

        public bool IsFeaturesModified { get; private set; }

        public IList<string> Links
        {
            get { return _links; }
            set
            {
                _links = value;
                IsLinksModified = true;
            }
        }

        public bool IsLinksModified { get; private set; }

        public override string ToString()
        {
            return string.Format("Agency: {0}; Id: {1}",
                string.IsNullOrWhiteSpace(AgencyId)
                    ? "--no Agency Id--"
                    : AgencyId,
                string.IsNullOrWhiteSpace(Id)
                    ? "--No Id--"
                    : Id);
        }

        public void CopyOverNewData(Listing newListing)
        {
            if (newListing == null)
            {
                throw new ArgumentNullException("newListing");
            }

            base.CopyOverNewData(newListing);

            if (newListing.IsAgencyIdModified)
            {
                AgencyId = newListing.AgencyId;
            }

            if (newListing.IsStatusTypeModified)
            {
                StatusType = newListing.StatusType;
            }

            if (newListing.IsCreatedOnModified)
            {
                CreatedOn = newListing.CreatedOn;
            }

            if (newListing.IsTitleModified)
            {
                Title = newListing.Title;
            }

            if (newListing.IsDescriptionModified)
            {
                Description = newListing.Description;
            }
            if (newListing.IsAddressModified)
            {
                if (newListing.Address == null)
                {
                    Address = null;
                }
                else
                {
                    if (Address == null)
                    {
                        Address = new Address();
                    }
                    Address.CopyOverNewData(newListing.Address);
                    IsAddressModified = true;
                }
            }

            if (newListing.IsAgentsModified)
            {
                if (newListing.Agents == null)
                {
                    Agents = null;
                }
                else
                {
                    Agents = new List<ListingAgent>();
                    foreach (var newAgent in newListing.Agents)
                    {
                        var agent = new ListingAgent();
                        agent.CopyOverNewData(newAgent);
                        Agents.Add(agent);
                    }
                }
            }

            if (newListing.IsImagesModified)
            {
                if (newListing.Images == null)
                {
                    Images = null;
                }
                else
                {
                    Images = new List<Media>();
                    foreach (var newImage in newListing.Images)
                    {
                        var image = new Media();
                        image.CopyOverNewData(newImage);
                        Images.Add(image);
                    }
                }
            }

            if (newListing.IsFloorPlansModified)
            {
                if (newListing.FloorPlans == null)
                {
                    FloorPlans = null;
                }
                else
                {
                    FloorPlans = new List<Media>();
                    foreach (var newFloorPlan in newListing.FloorPlans)
                    {
                        var floorPlan = new Media();
                        floorPlan.CopyOverNewData(newFloorPlan);
                        FloorPlans.Add(floorPlan);
                    }
                }
            }

            if (newListing.IsVideosModified)
            {
                if (newListing.Videos == null)
                {
                    Videos = null;
                }
                else
                {
                    Videos = new List<Media>();
                    foreach (var newVideo in newListing.Videos)
                    {
                        var video = new Media();
                        video.CopyOverNewData(newVideo);
                        Videos.Add(video);
                    }
                }
            }

            if (newListing.IsInspectionsModified)
            {
                if (newListing.Inspections == null)
                {
                    Inspections = null;
                }

                else
                {
                    Inspections = new List<Inspection>();
                    foreach (var newInspection in newListing.Inspections)
                    {
                        var inspection = new Inspection();
                        inspection.CopyOverNewData(newInspection);
                        Inspections.Add(inspection);
                    }

                }
            }

            if (newListing.IsLandDetailsModified)
            {
                if (newListing.LandDetails == null)
                {
                    LandDetails = null;
                }
                else
                {
                    if (LandDetails == null)
                    {
                        LandDetails = new LandDetails();
                    }
                    LandDetails.CopyOverNewData(newListing.LandDetails);
                    IsLandDetailsModified = true;
                }
            }

            if (newListing.IsFeaturesModified)
            {
                if (newListing.Features == null)
                {
                    Features = null;
                }
                else
                {
                    if (Features == null)
                    {
                        Features = new Features();
                    }
                    Features.CopyOverNewData(newListing.Features);
                    IsFeaturesModified = true;
                }
            }

            if (newListing.IsLinksModified)
            {
                Links = newListing.Links == null
                    ? null
                    : new List<string>(newListing.Links);
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (Address != null)
            {
                Address.ClearAllIsModified();
            }

            if (Agents != null)
            {
                foreach (var agent in Agents)
                {
                    agent.ClearAllIsModified();
                }
            }

            if (Images != null)
            {
                foreach (var image in Images)
                {
                    image.ClearAllIsModified();
                }
            }

            if (FloorPlans != null)
            {
                foreach (var floorPlan in FloorPlans)
                {
                    floorPlan.ClearAllIsModified();
                }
            }

            if (Videos != null)
            {
                foreach (var video in Videos)
                {
                    video.ClearAllIsModified();
                }
            }

            if (Inspections != null)
            {
                foreach (var inspection in Inspections)
                {
                    inspection.ClearAllIsModified();
                }
            }

            if (LandDetails != null)
            {
                LandDetails.ClearAllIsModified();
            }

            if (Features != null)
            {
                Features.ClearAllIsModified();
            }

            IsAgencyIdModified = false;
            IsStatusTypeModified = false;
            IsCreatedOnModified = false;
            IsTitleModified = false;
            IsDescriptionModified = false;
            IsAddressModified = false;
            IsAgentsModified = false;
            IsImagesModified = false;
            IsFloorPlansModified = false;
            IsVideosModified = false;
            IsInspectionsModified = false;
            IsLandDetailsModified = false;
            IsFeaturesModified = false;
            IsLinksModified = false;
        }
    }
}