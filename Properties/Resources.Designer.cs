//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace signversioned.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("signversioned.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
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
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Error: file {0} not found! ähnelt.
        /// </summary>
        internal static string Error0NotFound {
            get {
                return ResourceManager.GetString("Error0NotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Error: Signer and/or time server settings not found!
        ///       Please call &apos;signversioned.exe params&apos; once. ähnelt.
        /// </summary>
        internal static string ErrorIssuerAndOrTimeServerSettingsNotFound {
            get {
                return ResourceManager.GetString("ErrorIssuerAndOrTimeServerSettingsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Error: Moving {0} to {1} failed! ähnelt.
        /// </summary>
        internal static string ErrorMoving0To1Failed {
            get {
                return ResourceManager.GetString("ErrorMoving0To1Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Error: Signing {0} failed! ähnelt.
        /// </summary>
        internal static string ErrorSigning0Failed {
            get {
                return ResourceManager.GetString("ErrorSigning0Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Error: file signtool.exe not found in path! ähnelt.
        /// </summary>
        internal static string ErrorSigntoolExeNotFoundInPath {
            get {
                return ResourceManager.GetString("ErrorSigntoolExeNotFoundInPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Moving {0} to {1} successful! ähnelt.
        /// </summary>
        internal static string Moving0To1Successful {
            get {
                return ResourceManager.GetString("Moving0To1Successful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Please key in additional parameters (enter for none):
        /// ähnelt.
        /// </summary>
        internal static string NPleaseKeyInAdditionalParametersEnterForNoneN {
            get {
                return ResourceManager.GetString("NPleaseKeyInAdditionalParametersEnterForNoneN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Please key in the time server:
        /// ähnelt.
        /// </summary>
        internal static string NPleaseKeyInTheTimeServerN {
            get {
                return ResourceManager.GetString("NPleaseKeyInTheTimeServerN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die Please key in the signer&apos;s name:
        /// ähnelt.
        /// </summary>
        internal static string PleaseKeyInTheSignerSNameN {
            get {
                return ResourceManager.GetString("PleaseKeyInTheSignerSNameN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die 
        ///Usage: signversioned.exe &lt;target&gt;
        ///       signversioned.exe &apos;params&apos; (to set signtool.exe parameters) ähnelt.
        /// </summary>
        internal static string UsageSignversionedExeTarget {
            get {
                return ResourceManager.GetString("UsageSignversionedExeTarget", resourceCulture);
            }
        }
    }
}
