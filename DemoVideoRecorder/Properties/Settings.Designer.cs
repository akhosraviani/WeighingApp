﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _03_Onvif_Network_Video_Recorder.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1, 1")]
        public global::System.Drawing.Point AppWindowLocation {
            get {
                return ((global::System.Drawing.Point)(this["AppWindowLocation"]));
            }
            set {
                this["AppWindowLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("800, 800")]
        public global::System.Drawing.Size AppWindowSize {
            get {
                return ((global::System.Drawing.Size)(this["AppWindowSize"]));
            }
            set {
                this["AppWindowSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Maximized")]
        public global::System.Windows.Forms.FormWindowState AppWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["AppWindowState"]));
            }
            set {
                this["AppWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("172.20.200.1")]
        public string CameraIP1 {
            get {
                return ((string)(this["CameraIP1"]));
            }
            set {
                this["CameraIP1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("172.20.200.2")]
        public string CameraIP2 {
            get {
                return ((string)(this["CameraIP2"]));
            }
            set {
                this["CameraIP2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("172.20.200.9")]
        public string CameraIP3 {
            get {
                return ((string)(this["CameraIP3"]));
            }
            set {
                this["CameraIP3"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("172.20.200.10")]
        public string CameraIP4 {
            get {
                return ((string)(this["CameraIP4"]));
            }
            set {
                this["CameraIP4"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM3")]
        public string BascolPort {
            get {
                return ((string)(this["BascolPort"]));
            }
            set {
                this["BascolPort"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=.;Initial Catalog=AshaMES_PASCO_V03;Persist Security Info=True;User I" +
            "D=sa;Password=@sh@3rp")]
        public string AshaConnectionString {
            get {
                return ((string)(this["AshaConnectionString"]));
            }
        }
    }
}