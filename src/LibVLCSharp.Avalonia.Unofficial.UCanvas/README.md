# LibVLCSharp.Avalonia.Unofficial.UCanvas

The **Unofficial.Canvas** [Avalonia](https://github.com/AvaloniaUI/Avalonia) views for [LibVLCSharp](https://github.com/radiolondra/libvlcsharp/tree/3.x/README.md).

This package contains the views that allows to display multiple draggable controls over the scene of a video played with [LibVLCSharp](../LibVLCSharp/README.md) in an Avalonia app.

This package depends on [LibVLCSharp](https://github.com/radiolondra/libvlcsharp/tree/3.x/README.md) as well as [Avalonia](https://github.com/AvaloniaUI/Avalonia).

Refer to [LibVLCSharp.Avalonia.Unofficial](https://github.com/radiolondra/libvlcsharp/tree/3.x/src/LibVLCSharp.Avalonia.Unofficial) folder for a *version handling the VideoView content property only*.

Supported frameworks:

- netstandard2.0

Supported platforms:

- Windows
- MacOS
- Linux

### Usage

This **Unofficial.Canvas project** allows to show multiple moveable Avalonia UserControls over a video played with LibVLCSharp.

To do this, it **exposes 2 methods**:

- **AddXAMLContentIfAny(bool isMoveable)** 
  VideoView allows to define its own *content property* in XAML. 
  This method adds the VideoView content to the video scene.
  `IsMoveable` parameter defines whether the user will be able to drag the content over the scene.

- **AddUserControlToOverlay(  
      UserControl userControl,  
      Action<object> mouseEnter = null,  
      Action<object> mouseLeave = null,  
      VerticalAlignment? va = null,  
      HorizontalAlignment? ha = null,  
      double marginLeft = 0,  
      double marginTop = 0,  
      double marginRight = 0,  
      double marginBottom = 0,  
      double opacity = 1,  
      bool isMoveable = false  
  )**

This method adds whatever custom UserControl to the video scene with custom parameters. 
The parameters are pretty self explanatory.

For more information about usage, explore the test project in the [LibVLCSharp.Avalonia.Unofficial.UCanvas.Samples](https://github.com/radiolondra/libvlcsharp/tree/3.x/samples/LibVLCSharp.Avalonia.Unofficial.UCanvas.Samples) folder.