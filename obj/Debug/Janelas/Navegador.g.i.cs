﻿#pragma checksum "..\..\..\Janelas\Navegador.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B8E7BE42BE773EBEFA6E38684D723A5B3D5E158F"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

using BarberSystem.Janelas;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BarberSystem.Janelas {
    
    
    /// <summary>
    /// Navegador
    /// </summary>
    public partial class Navegador : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Janelas\Navegador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WebBrowser Browser;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\Janelas\Navegador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAtras;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\Janelas\Navegador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFrente;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Janelas\Navegador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGo;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Janelas\Navegador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSearch;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\Janelas\Navegador.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRefresh;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BarberSystem;component/janelas/navegador.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Janelas\Navegador.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Browser = ((System.Windows.Controls.WebBrowser)(target));
            return;
            case 2:
            this.btnAtras = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\Janelas\Navegador.xaml"
            this.btnAtras.Click += new System.Windows.RoutedEventHandler(this.btnAtras_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnFrente = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\Janelas\Navegador.xaml"
            this.btnFrente.Click += new System.Windows.RoutedEventHandler(this.btnFrente_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnGo = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\Janelas\Navegador.xaml"
            this.btnGo.Click += new System.Windows.RoutedEventHandler(this.btnGo_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.txtSearch = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.btnRefresh = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\Janelas\Navegador.xaml"
            this.btnRefresh.Click += new System.Windows.RoutedEventHandler(this.btnRefresh_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

