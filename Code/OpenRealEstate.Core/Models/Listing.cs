using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public abstract class Listing : AggregateRoot
    {
        private readonly InstanceObjectNotified<Address> _address;
        [Obsolete]
        private bool _isAddressModified;
        private readonly StringNotified _agencyId;
        private readonly ObservableCollection<ListingAgent> _agents;
        private readonly DateTimeNotified _createdOn;
        private readonly StringNotified _description;
        private readonly InstanceObjectNotified<Features> _features;
        [Obsolete]
        private bool _isFeaturesModified;
        private readonly ObservableCollection<Media> _floorPlans;
        private readonly ObservableCollection<Media> _images;
        private readonly ObservableCollection<Inspection> _inspections;
        private InstanceObjectNotified<LandDetails> _landDetails;
        [Obsolete]
        private bool _isLandDetailsModified;
        private IList<string> _links;
        private readonly EnumNotified<StatusType> _statusType;
        private readonly StringNotified _title;
        private readonly ObservableCollection<Media> _videos;
        private const string AddressName = "Address";
        private const string AgencyIdName = "AgencyId";
        private const string AgentsName = "AgencyId";
        private const string CreatedOnName = "CreatedOn";
        private const string DescriptionName = "Description";
        private const string FeaturesNames = "Features";
        private const string FloorPlansName = "FloorPlans";
        private const string ImagesName = "Images";
        private const string InspectionsName = "Inspections";
        private const string LandDetailsName = "LandDetails";
        private const string StatusTypeNane = "StatusType";
        private const string TitleName = "Title";
        private const string VideosName = "Videos";
        
        protected Listing()
        {
            _address = new InstanceObjectNotified<Address>(AddressName);
            _address.PropertyChanged += ModifiedData.OnPropertyChanged;

            _agencyId = new StringNotified(AgencyIdName);
            _agencyId.PropertyChanged += ModifiedData.OnPropertyChanged;

            _agents = new ObservableCollection<ListingAgent>();
            _agents.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(AgentsName); };

            _createdOn = new DateTimeNotified(CreatedOnName);
            _createdOn.PropertyChanged += ModifiedData.OnPropertyChanged;

            _description = new StringNotified(DescriptionName);
            _description.PropertyChanged += ModifiedData.OnPropertyChanged;

            _features = new InstanceObjectNotified<Features>(FeaturesNames);
            _features.PropertyChanged += ModifiedData.OnPropertyChanged;

            _floorPlans = new ObservableCollection<Media>();
            _floorPlans.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(FloorPlansName); };

            _images = new ObservableCollection<Media>();
            _images.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(ImagesName); };

            _inspections = new ObservableCollection<Inspection>();
            _inspections.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(InspectionsName); };

            _landDetails = new InstanceObjectNotified<LandDetails>(LandDetailsName);
            _landDetails.PropertyChanged += ModifiedData.OnPropertyChanged;

            _statusType = new EnumNotified<StatusType>(StatusTypeNane);
            _statusType.PropertyChanged += ModifiedData.OnPropertyChanged;

            _title = new StringNotified(TitleName);
            _title.PropertyChanged += ModifiedData.OnPropertyChanged;

            _videos = new ObservableCollection<Media>();
            _videos.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(VideosName); };
        }

        public string AgencyId
        {
            get { return _agencyId.Value; }
            set { _agencyId.Value = value; }
        }

        [Obsolete]
        public bool IsAgencyIdModified { get; private set; }

        public StatusType StatusType
        {
            get { return _statusType.Value; }
            set { _statusType.Value = value; }
        }

        [Obsolete]
        public bool IsStatusTypeModified { get; private set; }

        public DateTime CreatedOn
        {
            get { return _createdOn.Value; }
            set { _createdOn.Value = value; }
        }

        [Obsolete]
        public bool IsCreatedOnModified { get; private set; }

        public string Title
        {
            get { return _title.Value; }
            set { _title.Value = value; }
        }
        [Obsolete]
        public bool IsTitleModified { get; private set; }

        public string Description
        {
            get { return _description.Value; }
            set { _description.Value = value; }
        }
        [Obsolete]
        public bool IsDescriptionModified { get; private set; }

        public Address Address
        {
            get { return _address.Value; }
            set { _address.Value = value; }
        }
        [Obsolete]
        public bool IsAddressModified
        {
            get
            {
                return _isAddressModified ||
                       (Address != null &&
                        Address.IsModified);
            }
            set { _isAddressModified = value; }
        }

        public ReadOnlyCollection<ListingAgent> Agents
        {
            get { return _agents.ToList().AsReadOnly(); }
        }
        [Obsolete]
        public bool IsAgentsModified { get; private set; }

        public IList<Media> Images
        {
            get { return _images.ToList().AsReadOnly(); }
        }

        [Obsolete]
        public bool IsImagesModified { get; private set; }

        public ReadOnlyCollection<Media> FloorPlans
        {
            get { return _floorPlans.ToList().AsReadOnly(); }
        }

        [Obsolete]
        public bool IsFloorPlansModified { get; private set; }

        public IList<Media> Videos
        {
            get { return _videos.ToList().AsReadOnly(); }
        }
        [Obsolete]
        public bool IsVideosModified { get; private set; }

        public IList<Inspection> Inspections
        {
            get { return _inspections.ToList().AsReadOnly(); }
        }
        [Obsolete]
        public bool IsInspectionsModified { get; private set; }

        public LandDetails LandDetails
        {
            get { return _landDetails.Value; }
            set { _landDetails.Value = value; }
        }
        [Obsolete]
        public bool IsLandDetailsModified
        {
            get
            {
                return _isLandDetailsModified ||
                       (LandDetails != null &&
                        LandDetails.IsModified);
            }
            set { _isLandDetailsModified = value; }
        }

        public Features Features
        {
            get { return _features.Value; }
            set { _features.Value = value; }
        }
        [Obsolete]
        public bool IsFeaturesModified
        {
            get
            {
                return _isFeaturesModified ||
                       (Features != null &&
                        Features.IsModified);
            }
            set { _isFeaturesModified = value; }
        }

        public IList<string> Links
        {
            get { return _links; }
            set
            {
                _links = value;
                IsLinksModified = true;
            }
        }
        [Obsolete]
        public bool IsLinksModified { get; private set; }

        public override bool IsModified {
            get
            {
                return base.IsModified ||
                       IsAgencyIdModified ||
                       IsStatusTypeModified ||
                       IsCreatedOnModified ||
                       IsTitleModified ||
                       IsDescriptionModified ||
                       IsAddressModified ||
                       (Address != null && Address.IsModified) ||
                       IsAgentsModified ||
                       IsImagesModified ||
                       IsFloorPlansModified ||
                       IsVideosModified ||
                       IsInspectionsModified ||
                       IsLandDetailsModified ||
                       (LandDetails != null && LandDetails.IsModified) ||
                       IsFeaturesModified ||
                       (Features != null && Features.IsModified) ||
                       IsLinksModified;
            }
        }

        public void AddFloorPlans(ICollection<Media> floorPlans)
        {
            if (floorPlans == null)
            {
                throw new ArgumentNullException("floorPlans");
            }

            if (!floorPlans.Any())
            {
                throw new ArgumentOutOfRangeException("floorPlans");
            }

            foreach (var floorplan in floorPlans)
            {
                _floorPlans.Add(floorplan);
            }
        }

        public void RemoveFloorPlan(Media floorPlan)
        {
            if (floorPlan == null)
            {
                throw new ArgumentNullException("floorPlan");
            }

            if (_floorPlans != null)
            {
                _floorPlans.Remove(floorPlan);
            }
        }

        public void AddAgents(ICollection<ListingAgent> agents)
        {
            if (agents == null)
            {
                throw new ArgumentNullException("agents");
            }

            if (!agents.Any())
            {
                throw new ArgumentOutOfRangeException("agents");
            }

            foreach (var agent in agents)
            {
                _agents.Add(agent);
            }
        }

        public void RemoveAgent(ListingAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }

            if (_agents != null)
            {
                _agents.Remove(agent);
            }
        }

        public void AddImages(ICollection<Media> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException("images");
            }

            if (!images.Any())
            {
                throw new ArgumentOutOfRangeException("images");
            }

            foreach (var image in images)
            {
                _images.Add(image);
            }
        }

        public void RemoveImage(Media image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            if (_images != null)
            {
                _images.Remove(image);
            }
        }

        public void AddInspections(ICollection<Inspection> inspections)
        {
            if (inspections == null)
            {
                throw new ArgumentNullException("inspections");
            }

            if (!inspections.Any())
            {
                throw new ArgumentOutOfRangeException("inspections");
            }

            foreach (var inspection in inspections)
            {
                _inspections.Add(inspection);
            }
        }

        public void RemoveInspection(Inspection inspection)
        {
            if (inspection == null)
            {
                throw new ArgumentNullException("inspection");
            }

            if (_inspections != null)
            {
                _inspections.Remove(inspection);
            }
        }

        public void AddVideos(ICollection<Media> videos)
        {
            if (videos == null)
            {
                throw new ArgumentNullException("videos");
            }

            if (!videos.Any())
            {
                throw new ArgumentOutOfRangeException("videos");
            }

            foreach (var video in videos)
            {
                _videos.Add(video);
            }
        }

        public void RemoveVideo(Media video)
        {
            if (video == null)
            {
                throw new ArgumentNullException("video");
            }

            if (_videos != null)
            {
                _videos.Remove(video);
            }
        }

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

        public void Copy(Listing newListing)
        {
            base.Copy(newListing);

            if (newListing.ModifiedData.ModifiedCollections.Contains(AgentsName))
            {
                var agents = new List<ListingAgent>();
                foreach (var agent in newListing.Agents)
                {
                    var newAgent = new ListingAgent();
                    newAgent.Copy(agent);
                    agents.Add(newAgent);
                }
                AddAgents(agents);
            }

            // TODO: Somehow make this smarter using reflection.
            //       We need to manually 'Copy' the features because 
            //       this child property contains one or more Collections
            //       in it :/
            if (Features != null &&
                Features.ModifiedData.IsModified)
            {
                Features.Copy(newListing.Features);
            }
            if (LandDetails!= null &&
                LandDetails.ModifiedData.IsModified)
            {
                LandDetails.Copy(newListing.LandDetails);
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(FloorPlansName))
            {
                var floorPlans = new List<Media>();
                foreach (var floorPlan in newListing.FloorPlans)
                {
                    var newFloorPlan = new Media();
                    newFloorPlan.Copy(floorPlan);
                    floorPlans.Add(newFloorPlan);
                }
                AddFloorPlans(floorPlans);
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(ImagesName))
            {
                var images = new List<Media>();
                foreach (var image in newListing.Images)
                {
                    var newImage = new Media();
                    newImage.Copy(image);
                    images.Add(newImage);
                }
                AddImages(images);
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(InspectionsName))
            {
                var inspections = new List<Inspection>();
                foreach (var inspection in newListing.Inspections)
                {
                    var newInspection = new Inspection();
                    newInspection.Copy(inspection);
                    inspections.Add(newInspection);
                }
                AddInspections(inspections);
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(VideosName))
            {
                var videos = new List<Media>();
                foreach (var video in newListing.Videos)
                {
                    var newVideo = new Media();
                    newVideo.Copy(video);
                    videos.Add(newVideo);
                }
                AddVideos(videos);
            }
        }
    
        //public void CopyX(Listing newListing)
        //{
        //    if (newListing == null)
        //    {
        //        throw new ArgumentNullException("newListing");
        //    }

        //    if (!newListing.IsModified)
        //    {
        //        return;
        //    }

        //    base.Copy(newListing);

        //    if (newListing.IsAgencyIdModified)
        //    {
        //        AgencyId = newListing.AgencyId;
        //    }

        //    if (newListing.IsStatusTypeModified)
        //    {
        //        StatusType = newListing.StatusType;
        //    }

        //    if (newListing.IsCreatedOnModified)
        //    {
        //        CreatedOn = newListing.CreatedOn;
        //    }

        //    if (newListing.IsTitleModified)
        //    {
        //        Title = newListing.Title;
        //    }

        //    if (newListing.IsDescriptionModified)
        //    {
        //        Description = newListing.Description;
        //    }

        //    if (newListing.IsAddressModified)
        //    {
        //        if (newListing.Address == null)
        //        {
        //            Address = null;
        //        }
        //        else
        //        {
        //            if (Address == null)
        //            {
        //                Address = new Address();
        //            }

        //            if (newListing.Address.IsModified)
        //            {
        //                Address.Copy(newListing.Address);
        //            }

        //            IsAddressModified = true;
        //        }
        //    }

        //    if (newListing.IsAgentsModified)
        //    {
        //        if (newListing.Agents == null)
        //        {
        //            Agents = null;
        //        }
        //        else
        //        {
        //            Agents = new List<ListingAgent>();
        //            foreach (var newAgent in newListing.Agents)
        //            {
        //                var agent = new ListingAgent();
        //                agent.Copy(newAgent);
        //                Agents.Add(agent);
        //            }
        //        }
        //    }

        //    if (newListing.IsImagesModified)
        //    {
        //        if (newListing.Images == null)
        //        {
        //            Images = null;
        //        }
        //        else
        //        {
        //            Images = new List<Media>();
        //            foreach (var newImage in newListing.Images)
        //            {
        //                var image = new Media();
        //                image.Copy(newImage);
        //                Images.Add(image);
        //            }
        //        }
        //    }

        //    if (newListing.IsFloorPlansModified)
        //    {
        //        if (newListing.FloorPlans == null)
        //        {
        //            FloorPlans = null;
        //        }
        //        else
        //        {
        //            FloorPlans = new List<Media>();
        //            foreach (var newFloorPlan in newListing.FloorPlans)
        //            {
        //                var floorPlan = new Media();
        //                floorPlan.Copy(newFloorPlan);
        //                FloorPlans.Add(floorPlan);
        //            }
        //        }
        //    }

        //    if (newListing.IsVideosModified)
        //    {
        //        if (newListing.Videos == null)
        //        {
        //            Videos = null;
        //        }
        //        else
        //        {
        //            Videos = new List<Media>();
        //            foreach (var newVideo in newListing.Videos)
        //            {
        //                var video = new Media();
        //                video.Copy(newVideo);
        //                Videos.Add(video);
        //            }
        //        }
        //    }

        //    if (newListing.IsInspectionsModified)
        //    {
        //        if (newListing.Inspections == null)
        //        {
        //            Inspections = null;
        //        }

        //        else
        //        {
        //            Inspections = new List<Inspection>();
        //            foreach (var newInspection in newListing.Inspections)
        //            {
        //                var inspection = new Inspection();
        //                inspection.Copy(newInspection);
        //                Inspections.Add(inspection);
        //            }

        //        }
        //    }

        //    if (newListing.IsLandDetailsModified)
        //    {
        //        if (newListing.LandDetails == null)
        //        {
        //            LandDetails = null;
        //        }
        //        else
        //        {
        //            if (LandDetails == null)
        //            {
        //                LandDetails = new LandDetails();
        //            }

        //            if (newListing.LandDetails.IsModified)
        //            {
        //                LandDetails.Copy(newListing.LandDetails);
        //            }

        //            IsLandDetailsModified = true;
        //        }
        //    }

        //    if (newListing.IsFeaturesModified)
        //    {
        //        if (newListing.Features == null)
        //        {
        //            Features = null;
        //        }
        //        else
        //        {
        //            if (Features == null)
        //            {
        //                Features = new Features();
        //            }

        //            if (newListing.Features.IsModified)
        //            {
        //                Features.Copy(newListing.Features);
        //            }

        //            IsFeaturesModified = true;
        //        }
        //    }

        //    if (newListing.IsLinksModified)
        //    {
        //        Links = newListing.Links == null
        //            ? null
        //            : new List<string>(newListing.Links);
        //    }
        //}

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (_address.Value != null &&
                _address.Value.IsModified)
            {
                _address.Value.ClearAllIsModified();
            }

            if (_features.Value != null &&
                _features.Value.IsModified)
            {
                _features.Value.ClearAllIsModified();
            }

            if (_landDetails.Value != null &&
                _landDetails.Value.IsModified)
            {
                _landDetails.Value.ClearAllIsModified();
            }

            ModifiedData.ClearModifiedProperties(new[]
            {
                AddressName,
                AgencyIdName,
                AgentsName,
                CreatedOnName,
                DescriptionName,
                ImagesName,
                FloorPlansName,
                InspectionsName,
                LandDetailsName,
                StatusTypeNane,
                TitleName,
                VideosName,
            });




            //if (Address != null)
            //{
            //    Address.ClearAllIsModified();
            //}
            //IsAddressModified = false;

            //if (Agents != null)
            //{
            //    foreach (var agent in Agents)
            //    {
            //        agent.ClearAllIsModified();
            //    }
            //}
            //IsAgentsModified = false;

            //if (Images != null)
            //{
            //    foreach (var image in Images)
            //    {
            //        image.ClearAllIsModified();
            //    }
            //}
            //IsImagesModified = false;

            //if (FloorPlans != null)
            //{
            //    foreach (var floorPlan in FloorPlans)
            //    {
            //        floorPlan.ClearAllIsModified();
            //    }
            //}
            //IsFloorPlansModified = false;

            //if (Videos != null)
            //{
            //    foreach (var video in Videos)
            //    {
            //        video.ClearAllIsModified();
            //    }
            //}
            //IsVideosModified = false;

            //if (Inspections != null)
            //{
            //    foreach (var inspection in Inspections)
            //    {
            //        inspection.ClearAllIsModified();
            //    }
            //}

            //if (LandDetails != null)
            //{
            //    LandDetails.ClearAllIsModified();
            //}
            //IsLandDetailsModified = false;

            //if (Features != null)
            //{
            //    Features.ClearAllIsModified();
            //}
            //IsFeaturesModified = false;

            //IsAgencyIdModified = false;
            //IsStatusTypeModified = false;
            //IsCreatedOnModified = false;
            //IsTitleModified = false;
            //IsDescriptionModified = false;
            //IsInspectionsModified = false;
            //IsLinksModified = false;
        }
    }
}