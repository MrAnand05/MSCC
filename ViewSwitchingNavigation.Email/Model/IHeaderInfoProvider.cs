// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.


namespace ViewSwitchingNavigation.Email.Model
{
    /// <summary>
    /// Provides an easy way to recognize a class that exposes a HeaderInfo that can be used to bind to a header from XAML.
    /// </summary>
    /// <typeparam name="T">The HeaderInfo type</typeparam>
    public interface IHeaderInfoProvider<T>
    {
        T HeaderInfo { get; }
    }
}