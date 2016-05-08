﻿#pragma checksum "..\..\AuthorWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4C5576295CFBFAD009989CF4FA702A9B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace GUI {
    
    
    /// <summary>
    /// AuthorWindow
    /// </summary>
    public partial class AuthorWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 18 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label statusLabel;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid authorDataGrid;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button chooseAuthorButton;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deleteAuthorButton;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label authorPublicationLabel;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label authorListLabel;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeButton;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid authorPublicationDataGrid;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label authorPublicationCountLabel;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\AuthorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label authorListCountLabel;
        
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
            System.Uri resourceLocater = new System.Uri("/GUI;component/authorwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AuthorWindow.xaml"
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
            this.statusLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.authorDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 20 "..\..\AuthorWindow.xaml"
            this.authorDataGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.authorDataGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.chooseAuthorButton = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\AuthorWindow.xaml"
            this.chooseAuthorButton.Click += new System.Windows.RoutedEventHandler(this.chooseAuthorButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.deleteAuthorButton = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\AuthorWindow.xaml"
            this.deleteAuthorButton.Click += new System.Windows.RoutedEventHandler(this.deleteAuthorButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.authorPublicationLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.authorListLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.closeButton = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\AuthorWindow.xaml"
            this.closeButton.Click += new System.Windows.RoutedEventHandler(this.closeButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.authorPublicationDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 9:
            this.authorPublicationCountLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.authorListCountLabel = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

