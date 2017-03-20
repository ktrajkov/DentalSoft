namespace DentalSoft.Data.Models.Status.TeethStatus
{
    using DentalSoft.Common.CustomAttributes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.ComponentModel;
    using System.Runtime.Serialization;


    public enum ChartType
    {
        [LocalizedDescription("ChartType_PartOfCrown", typeof(Strings))]
        PartOfCrown,


        [LocalizedDescription("ChartType_Crown", typeof(Strings))]
        Crown,

        [LocalizedDescription("ChartType_MissingCrown", typeof(Strings))]
        MissingCrown,

        [LocalizedDescription("ChartType_Bridge", typeof(Strings))]
        Bridge,

        [LocalizedDescription("ChartType_PartOfRoot", typeof(Strings))]
        PartOfRoot,

        [LocalizedDescription("ChartType_Root", typeof(Strings))]
        Root,

        [LocalizedDescription("ChartType_Pin", typeof(Strings))]
        RadixAnchor,

        [LocalizedDescription("ChartType_Implant", typeof(Strings))]
        Implant,

        [LocalizedDescription("ChartType_MissingTooth", typeof(Strings))]
        MissingTooth,

        [LocalizedDescription("ChartType_Prosthesis", typeof(Strings))]
        Prosthesis,

        [LocalizedDescription("ChartType_PartialProsthesis", typeof(Strings))]
        PartialProsthesis,

        [LocalizedDescription("ChartType_PartOfCrown_Grid", typeof(Strings))]
        PartOfCrown_Grid,

        [LocalizedDescription("ChartType_Crown_Key", typeof(Strings))]
        Crown_Key,

        [LocalizedDescription("ChartType_PartOfCrownWithGrid", typeof(Strings))]
        PartOfCrownWithGrid,

        [LocalizedDescription("ChartType_RemovePartialProsthesis", typeof(Strings))]
        RemovePartialProsthesis,

        [LocalizedDescription("ChartType_RemoveProsthesis", typeof(Strings))]
        RemoveProsthesis,

        [LocalizedDescription("ChartType_RemoveCrown", typeof(Strings))]
        RemoveCrown,

        [LocalizedDescription("ChartType_RemoveRadixAnchor", typeof(Strings))]
        RemoveRadixAnchor,

        [LocalizedDescription("ChartType_RetiniranTooth", typeof(Strings))]
        RetiniranTooth,

        [LocalizedDescription("ChartType_OutRetiniranTooth", typeof(Strings))]
        OutRetiniranTooth
    }
}
