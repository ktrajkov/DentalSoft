﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DentalSoft.Services {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DentalSoft.Services.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Полето &quot;Стоматолог&quot; е задължително!.
        /// </summary>
        internal static string ContactEntityFilled_DentistIsRequired {
            get {
                return ResourceManager.GetString("ContactEntityFilled_DentistIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Съществува диагноза с такъв код.
        /// </summary>
        internal static string DiagnosisCheckForUniqueness_CodeShouldBeUnique {
            get {
                return ResourceManager.GetString("DiagnosisCheckForUniqueness_CodeShouldBeUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ПП.
        /// </summary>
        internal static string MedicalCheckup {
            get {
                return ResourceManager.GetString("MedicalCheckup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Въведи стоматолог.
        /// </summary>
        internal static string PersonalData_DentistIsRequired {
            get {
                return ResourceManager.GetString("PersonalData_DentistIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Въведи име.
        /// </summary>
        internal static string PersonalData_FirstNameIsRequired {
            get {
                return ResourceManager.GetString("PersonalData_FirstNameIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Въведи фамилия.
        /// </summary>
        internal static string PersonalData_LastNameIsRequired {
            get {
                return ResourceManager.GetString("PersonalData_LastNameIsRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Съществува пациент с такова име и фамилия.
        /// </summary>
        internal static string PersonalDataCheckForUniqueness_FirstNameAndLastNameShouldBeUnique {
            get {
                return ResourceManager.GetString("PersonalDataCheckForUniqueness_FirstNameAndLastNameShouldBeUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Съществува лечение с такъв код.
        /// </summary>
        internal static string TreatmentCheckForUniqueness_CodeShouldBeUnique {
            get {
                return ResourceManager.GetString("TreatmentCheckForUniqueness_CodeShouldBeUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Съществува лечение с такова описание.
        /// </summary>
        internal static string TreatmentCheckForUniqueness_DescriptionShouldBeUnique {
            get {
                return ResourceManager.GetString("TreatmentCheckForUniqueness_DescriptionShouldBeUnique", resourceCulture);
            }
        }
    }
}
