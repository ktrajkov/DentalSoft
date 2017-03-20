namespace DentalSoft.Data.Models.PersonalInfo.Addresses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Municipality :DeletableEntity, IListEnitty
    {
        public Municipality()
        {
            this.locations = new HashSet<Location>();
        }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public int RegionId { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Location> Locations
        {
            get { return this.locations; }
            set { this.locations = value; }
        }

        private ICollection<Location> locations { get; set; }
    }
}
