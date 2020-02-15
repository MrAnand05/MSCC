// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace ViewSwitchingNavigation.Email.Views
{
    //[ViewExport("InboxView",RegionName = RegionNames.MainContentRegion)]
    //commented by anand
    [Export("StudentView")]
    public partial class StudentView : UserControl
    {
        public StudentView()
        {
            InitializeComponent();
        }
    }
}
