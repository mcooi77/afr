﻿#pragma checksum "C:\Users\mcooi\OneDrive\Documents\Penn State Offline\CS\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "51DABDD99980F350757B5E09D3F287DA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SerialSample
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.DeviceListSource = (global::Windows.UI.Xaml.Data.CollectionViewSource)(target);
                }
                break;
            case 2:
                {
                    this.MyDataPtsSource = (global::Windows.UI.Xaml.Data.CollectionViewSource)(target);
                }
                break;
            case 3:
                {
                    this.status = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4:
                {
                    this.pageTitle = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.textBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6:
                {
                    this.ComputeTreatmentBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 148 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.ComputeTreatmentBtn).Click += this.ComputeTreatmentBtn_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.TimeElapsedLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.TimeElapsed = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.TreatmentInitalTimeLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.TreatmentInitalTime = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.textBlock6 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.textBlock7 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.textBlock8 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 14:
                {
                    this.PatientParklandResultBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 137 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientParklandResultBox).TextChanged += this.PatientParklandResultBox_TextChanged;
                    #line default
                }
                break;
            case 15:
                {
                    this.RecommendedRateEightHrs = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 138 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.RecommendedRateEightHrs).TextChanged += this.RecommendedRateEightHrs_TextChanged;
                    #line default
                }
                break;
            case 16:
                {
                    this.RecommendedRateFinalHrs = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 139 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.RecommendedRateFinalHrs).TextChanged += this.RecommendedRateFinalHrs_TextChanged;
                    #line default
                }
                break;
            case 17:
                {
                    this.textBlock3 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 18:
                {
                    this.textBlock4 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 19:
                {
                    this.textBlock5 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 20:
                {
                    this.textBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 21:
                {
                    this.textBlock1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 22:
                {
                    this.textBlock2 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 23:
                {
                    this.PatientNameBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 119 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientNameBox).TextChanged += this.PatientNameBox_TextChanged;
                    #line default
                }
                break;
            case 24:
                {
                    this.PatientWeightEnteredBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 120 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientWeightEnteredBox).TextChanged += this.PatientWeightEnteredBox_TextChanged;
                    #line default
                }
                break;
            case 25:
                {
                    this.PatientBurnEnteredBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 121 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientBurnEnteredBox).TextChanged += this.PatientBurnEnteredBox_TextChanged;
                    #line default
                }
                break;
            case 26:
                {
                    this.PatientPercentBurnTxt = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 111 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientPercentBurnTxt).TextChanged += this.PatientPercentBurnTxt_TextChanged;
                    #line 111 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientPercentBurnTxt).GotFocus += this.PatientPercentBurnTxt_GotFocus;
                    #line 111 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientPercentBurnTxt).CharacterReceived += this.PatientPercentBurnTxt_CharacterReceived;
                    #line default
                }
                break;
            case 27:
                {
                    this.TitleBurned = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 28:
                {
                    this.EnterBurnBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 113 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.EnterBurnBtn).Click += this.EnterBurnBtn_Click;
                    #line default
                }
                break;
            case 29:
                {
                    this.PatientBurnInputErrorTxt = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 30:
                {
                    this.WeightInputErrorTxt = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 31:
                {
                    this.PatientWeightTxt = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 102 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientWeightTxt).TextChanged += this.PatientWeight_TextChanged;
                    #line 102 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientWeightTxt).GotFocus += this.PatientWeightTxt_GotFocus;
                    #line 102 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.PatientWeightTxt).CharacterReceived += this.PatientWeightTxt_CharacterReceived;
                    #line default
                }
                break;
            case 32:
                {
                    this.TitleWeight = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 33:
                {
                    this.EnterWeightBtn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 104 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.EnterWeightBtn).Click += this.EnterWeightBtn_Click;
                    #line default
                }
                break;
            case 34:
                {
                    global::Windows.UI.Xaml.Controls.ListView element34 = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 62 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)element34).SelectionChanged += this.ListView_SelectionChanged_1;
                    #line default
                }
                break;
            case 35:
                {
                    this.DynamicListView = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 79 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.DynamicListView).SelectionChanged += this.ListView_SelectionChanged;
                    #line default
                }
                break;
            case 36:
                {
                    this.comPortInput = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 38 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.comPortInput).Click += this.comPortInput_Click;
                    #line default
                }
                break;
            case 37:
                {
                    this.closeDevice = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 39 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.closeDevice).Click += this.closeDevice_Click;
                    #line default
                }
                break;
            case 38:
                {
                    this.ConnectDevices = (global::Windows.UI.Xaml.Controls.ListBox)(target);
                    #line 43 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBox)this.ConnectDevices).SelectionChanged += this.ConnectDevices_SelectionChanged;
                    #line default
                }
                break;
            case 39:
                {
                    this.rcvdText = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 40:
                {
                    this.textBlock9 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

