namespace DentalSoft.Common.Mapping
{
    using System;

    /// <summary>
    /// Maps a public field or property to a field or property in single or all origin types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class MapAssociationAttribute : Attribute
    {
        /// <summary>
        /// Gets the assiciation path.
        /// </summary>
        public string AssociationPath
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the origin type.
        /// </summary>
        public Type Origin
        {
            get;
            set;
        }
        /// <summary>
        /// Initializes a new instance of <see cref="T:MapAttribute" />.
        /// </summary>
        /// <param name="associationPath">The association path.</param>
        public MapAssociationAttribute(string associationPath)
        {
            ExceptionUtil.NotEmpty(associationPath, "associationPath");
            this.AssociationPath = associationPath;
        }
    }
}
