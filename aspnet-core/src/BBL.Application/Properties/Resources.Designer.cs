﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BBL.Properties {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("BBL.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string CampoInvalido {
            get {
                return ResourceManager.GetString("CampoInvalido", resourceCulture);
            }
        }
        
        internal static string CampoObrigatorio {
            get {
                return ResourceManager.GetString("CampoObrigatorio", resourceCulture);
            }
        }
        
        internal static string NaoPermitidoExcluir {
            get {
                return ResourceManager.GetString("NaoPermitidoExcluir", resourceCulture);
            }
        }
        
        internal static string newUserPasswordBody {
            get {
                return ResourceManager.GetString("newUserPasswordBody", resourceCulture);
            }
        }
        
        internal static string newUserPasswordSubject {
            get {
                return ResourceManager.GetString("newUserPasswordSubject", resourceCulture);
            }
        }
        
        internal static string recoveryPasswordBody {
            get {
                return ResourceManager.GetString("recoveryPasswordBody", resourceCulture);
            }
        }
        
        internal static string recoveryPasswordSubject {
            get {
                return ResourceManager.GetString("recoveryPasswordSubject", resourceCulture);
            }
        }
        
        internal static string subjectAtualizacaoFase {
            get {
                return ResourceManager.GetString("subjectAtualizacaoFase", resourceCulture);
            }
        }
        
        internal static string atualizacaoFaseProjeto {
            get {
                return ResourceManager.GetString("atualizacaoFaseProjeto", resourceCulture);
            }
        }
    }
}
