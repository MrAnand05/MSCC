// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Controls.Primitives;

namespace ViewSwitchingNavigation.Controls
{
    /// <summary>
    /// Interaction logic for InfoTipToggleButton.xaml
    /// </summary>
    public partial class InfoTipToggleButton : ToggleButton
    {
        public InfoTipToggleButton()
        {
            this.InitializeComponent();

            this.IsThreeState = false;
            this.Checked += InfoTipToggleButton_Checked;
            this.Unchecked += InfoTipToggleButton_Checked;
        }

        public static readonly DependencyProperty PopupProperty =
            DependencyProperty.Register("PopupProperty", typeof(Popup), typeof(InfoTipToggleButton));

        public Popup Popup
        {
            get { return (Popup)this.GetValue(PopupProperty); }
            set { this.SetValue(PopupProperty, value); }
        }

        void InfoTipToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.Popup != null)
            {
                if (this.IsChecked.HasValue && this.IsChecked.Value)
                {
                    this.Popup.PlacementTarget = this;
                    this.Popup.Placement = PlacementMode.Bottom;
                    this.Popup.IsOpen = true;
                }
                else
                {
                    this.Popup.IsOpen = false;
                }
            }

        }
    }
}
