﻿using Uno.UI;
using Windows.UI.Xaml.Controls;

namespace LibVLCSharp.Uno
{
    /// <summary>
    /// Video view
    /// </summary>
    public partial class VideoView : VideoViewWrapper<LibVLCSharp.VideoView>
    {
        /// <summary>
        /// Creates the underlying video view and set the <see cref="Border.Child"/> property value
        /// </summary>
        /// <returns>the created underlying video view</returns>
        protected override LibVLCSharp.VideoView CreateUnderlyingVideoView()
        {
            var underlyingVideoView = new LibVLCSharp.VideoView(ContextHelper.Current);
            Border!.Child = underlyingVideoView;
            return underlyingVideoView;
        }
    }
}
