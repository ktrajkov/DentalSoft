using DentalSoft.Common.CustomAttributes;
namespace DentalSoft.Data.Models.DailyPlannings
{
    public enum StatusType
    {      
        [LocalizedDescription("StatusType_Booked", typeof(Strings))]
        Booked,
        [LocalizedDescription("StatusType_Done", typeof(Strings))]
        Done,
        [LocalizedDescription("StatusType_Informed", typeof(Strings))]
        Informed,
        [LocalizedDescription("StatusType_Uninformed", typeof(Strings))]
        Uninformed,
        [LocalizedDescription("StatusType_Unbooked", typeof(Strings))]
        Unbooked,
        [LocalizedDescription("StatusType_Reminder", typeof(Strings))]
        Reminder,
        [LocalizedDescription("StatusType_Completed", typeof(Strings))]
        Completed,
    }

}
