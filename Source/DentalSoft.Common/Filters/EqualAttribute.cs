namespace DentalSoft.Common.Filters
{
    /// <summary>
    /// Marks that the value of the underlying association must be equal to the filter value.
    /// </summary>
    public class EqualAttribute : AssociationFilterAttribute
    {
        /// <summary>
        /// Gets or sets a value indicating that the value is case sensitive.
        /// </summary>
        public bool CaseSensitive
        {
            get;
            set;
        }
    }
}
