using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenRealEstate.Core.Primitives;

namespace OpenRealEstate.Core.Models
{
    public abstract class Listing : AggregateRoot
    {
        private const string AddressName = "Address";
        private const string AgencyIdName = "AgencyId";
        private const string AgentsName = "Agents";
        private const string CreatedOnName = "CreatedOn";
        private const string DescriptionName = "Description";
        private const string FeaturesNames = "Features";
        private const string FloorPlansName = "FloorPlans";
        private const string ImagesName = "Images";
        private const string InspectionsName = "Inspections";
        private const string LandDetailsName = "LandDetails";
        private const string LinksName = "Links";
        private const string StatusTypeNane = "StatusType";
        private const string TitleName = "Title";
        private const string VideosName = "Videos";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<Address> _address;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _agencyId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<ListingAgent> _agents;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTimeNotified _createdOn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _description;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<Features> _features;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Media> _floorPlans;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Media> _images;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Inspection> _inspections;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly InstanceObjectNotified<LandDetails> _landDetails;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<string> _links;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly EnumNotified<StatusType> _statusType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly StringNotified _title;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableCollection<Media> _videos;

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

            _links = new ObservableCollection<string>();
            _links.CollectionChanged += (sender, args) => { ModifiedData.OnCollectionChanged(LinksName); };

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

        public StatusType StatusType
        {
            get { return _statusType.Value; }
            set { _statusType.Value = value; }
        }

        public DateTime CreatedOn
        {
            get { return _createdOn.Value; }
            set { _createdOn.Value = value; }
        }

        public string Title
        {
            get { return _title.Value; }
            set { _title.Value = value; }
        }

        public string Description
        {
            get { return _description.Value; }
            set { _description.Value = value; }
        }

        public Address Address
        {
            get { return _address.Value; }
            set { _address.Value = value; }
        }

        public ReadOnlyCollection<ListingAgent> Agents
        {
            get { return _agents.ToList().AsReadOnly(); }
            set
            {
                _agents.Clear();
                AddAgents(value);
            }
        }

        public ReadOnlyCollection<Media> Images
        {
            get { return _images.ToList().AsReadOnly(); }
            set
            {
                _images.Clear();
                AddImages(value);
            }
        }

        public ReadOnlyCollection<Media> FloorPlans
        {
            get { return _floorPlans.ToList().AsReadOnly(); }
            set
            {
                _floorPlans.Clear();
                AddFloorPlans(value);
            }
        }

        public ReadOnlyCollection<Media> Videos
        {
            get { return _videos.ToList().AsReadOnly(); }
            set
            {
                _videos.Clear();
                AddVideos(value);
            }
        }

        public ReadOnlyCollection<Inspection> Inspections
        {
            get { return _inspections.ToList().AsReadOnly(); }
            set
            {
                _inspections.Clear();
                AddInspections(value);
            }
        }

        public LandDetails LandDetails
        {
            get { return _landDetails.Value; }
            set { _landDetails.Value = value; }
        }

        public Features Features
        {
            get { return _features.Value; }
            set { _features.Value = value; }
        }

        public ReadOnlyCollection<string> Links
        {
            get { return _links.ToList().AsReadOnly(); }
            set
            {
                _links.Clear();
                AddLinks(value);
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

        public void AddLinks(ICollection<string> links)
        {
            if (links == null)
            {
                throw new ArgumentNullException("links");
            }

            if (!links.Any())
            {
                throw new ArgumentOutOfRangeException("links");
            }

            foreach (var link in links)
            {
                _links.Add(link);
            }
        }

        public void RemoveLink(string link)
        {
            if (link == null)
            {
                throw new ArgumentNullException("link");
            }

            if (_links != null)
            {
                _links.Remove(link);
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

        public void Copy(Listing newListing, bool isModifiedPropertiesOnly = true)
        {
            base.Copy(newListing, isModifiedPropertiesOnly);

            if (newListing.ModifiedData.ModifiedCollections.Contains(AgentsName))
            {
                _agents.Clear();

                if (newListing.Agents.Any())
                {
                    var agents = new List<ListingAgent>();
                    foreach (var agent in newListing.Agents)
                    {
                        var newAgent = new ListingAgent();
                        newAgent.Copy(agent, false);
                        agents.Add(newAgent);
                    }
                    AddAgents(agents);
                }
            }

            // TODO: Somehow make this smarter using reflection.
            //       We need to manually 'Copy' the features because 
            //       this child property contains one or more Collections
            //       in it :/
            if (newListing.Features != null &&
                newListing.Features.ModifiedData.IsModified)
            {
                Features = new Features();
                Features.Copy(newListing.Features);
            }
            if (newListing.LandDetails != null &&
                newListing.LandDetails.ModifiedData.IsModified)
            {
                LandDetails = new LandDetails();
                LandDetails.Copy(newListing.LandDetails);
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(FloorPlansName))
            {
                _floorPlans.Clear();

                if (newListing.FloorPlans.Any())
                {
                    var floorPlans = new List<Media>();
                    foreach (var floorPlan in newListing.FloorPlans)
                    {
                        var newFloorPlan = new Media();
                        newFloorPlan.Copy(floorPlan, false);
                        floorPlans.Add(newFloorPlan);
                    }
                    AddFloorPlans(floorPlans);
                }
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(ImagesName))
            {
                _images.Clear();

                if (newListing.Images.Any())
                {
                    var images = new List<Media>();
                    foreach (var image in newListing.Images)
                    {
                        var newImage = new Media();
                        newImage.Copy(image, false);
                        images.Add(newImage);
                    }
                    AddImages(images);
                }
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(InspectionsName))
            {
                _inspections.Clear();

                if (newListing.Inspections.Any())
                {
                    var inspections = new List<Inspection>();
                    foreach (var inspection in newListing.Inspections)
                    {
                        var newInspection = new Inspection();
                        newInspection.Copy(inspection, false);
                        inspections.Add(newInspection);
                    }
                    AddInspections(inspections);
                }
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(VideosName))
            {
                _videos.Clear();

                if (newListing.Videos.Any())
                {
                    var videos = new List<Media>();
                    foreach (var video in newListing.Videos)
                    {
                        var newVideo = new Media();
                        newVideo.Copy(video, false);
                        videos.Add(newVideo);
                    }
                    AddVideos(videos);
                }
            }

            if (newListing.ModifiedData.ModifiedCollections.Contains(LinksName))
            {
                _links.Clear();

                if (newListing.Links.Any())
                {
                    AddLinks(newListing.Links.ToList());
                }
            }
        }

        public override void ClearAllIsModified()
        {
            base.ClearAllIsModified();

            if (_address.Value != null &&
                _address.Value.ModifiedData.IsModified)
            {
                _address.Value.ClearAllIsModified();
            }

            if (_features.Value != null &&
                _features.Value.ModifiedData.IsModified)
            {
                _features.Value.ClearAllIsModified();
            }

            if (_landDetails.Value != null &&
                _landDetails.Value.ModifiedData.IsModified)
            {
                _landDetails.Value.ClearAllIsModified();
            }

            HelperUtilities.ClearAllObservableCollectionItems(_agents);
            HelperUtilities.ClearAllObservableCollectionItems(_images);
            HelperUtilities.ClearAllObservableCollectionItems(_floorPlans);
            HelperUtilities.ClearAllObservableCollectionItems(_inspections);
            HelperUtilities.ClearAllObservableCollectionItems(_videos);
        }
    }
}