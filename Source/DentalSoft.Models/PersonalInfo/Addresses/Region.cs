namespace DentalSoft.Data.Models.PersonalInfo.Addresses
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Region :DeletableEntity, IListEnitty
    {
        public Region()
        {
            this.municipalitys = new HashSet<Municipality>();
        }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Municipality> Municipalitys
        {
            get { return this.municipalitys; }
            set { this.municipalitys = value; }
        }

        private ICollection<Municipality> municipalitys { get; set; }
    }
}
