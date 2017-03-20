namespace DentalSoft.Data.Models.Status.TeethStatus
{
    using DentalSoft.Common.CustomAttributes;

    public enum ColorType
    {
         [LocalizedDescription("ColorType_Blue", typeof(Strings))]
        Blue,

          [LocalizedDescription("ColorType_DarkBlue", typeof(Strings))]
        DarkBlue,

         [LocalizedDescription("ColorType_Black", typeof(Strings))]
        Black,

         [LocalizedDescription("ColorType_Red", typeof(Strings))]
        Red,

        [LocalizedDescription("ColorType_Green", typeof(Strings))]
        Green,

         [LocalizedDescription("ColorType_Purple", typeof(Strings))]
        Purple,

         [LocalizedDescription("ColorType_Grey", typeof(Strings))]
         Grey
    }
}
