using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Avalonia.Layout;
using Avalonia.Interactivity;
using Avalonia.Input;
using System.Diagnostics;
using Avalonia.VisualTree;
using System.Linq;

namespace LibVLCSharp.Avalonia.UCanvas
{    
    public class OverlappedWindow: Window
    {
        OverlayPanel MovingObject;
        bool draggingStarted;
        public static OverlappedWindow _this;

        public static readonly DirectProperty<OverlappedWindow, Maybe<Canvas>> RootCanvasProperty =
           AvaloniaProperty.RegisterDirect<OverlappedWindow, Maybe<Canvas>>(
               nameof(RootCanvas),
               o => o.RootCanvas,
               (o, v) => o.RootCanvas = v.GetValueOrDefault(),
               defaultBindingMode: BindingMode.TwoWay);

        private readonly BehaviorSubject<Maybe<Canvas>> rootCanvas = new(Maybe<Canvas>.None);
        

        public OverlappedWindow()
        {            
            SystemDecorations = SystemDecorations.None;
            TransparencyLevelHint = WindowTransparencyLevel.Transparent;
            Background = Brushes.Transparent;
            SizeToContent = SizeToContent.WidthAndHeight;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            CanResize = false;
            ShowInTaskbar = false;
            ZIndex = Int32.MaxValue;
            Opacity = 1;

            Opened += OverlappedWindow_Opened;

            // main RootCanvas            
            RootCanvas = new Canvas();
            RootCanvas.Width = this.Width;
            RootCanvas.Height = this.Height;
            RootCanvas.Background = Brushes.Transparent;
            RootCanvas.Name = "RootCanvas";
            RootCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            RootCanvas.VerticalAlignment = VerticalAlignment.Stretch;
            
            RootCanvas.PointerMoved += RootCanvas_PointerMoved;            
            RootCanvas.PropertyChanged += RootCanvas_PropertyChanged;

            MovingObject = null;
            draggingStarted = false;
            _this = this;

            

        }

        private void RootCanvas_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            var rootChild = this.Presenter?.Child;            
            UpdateOverlayChildsPositions(rootChild);
        }        

        public static OverlappedWindow GetInstance()
        {
            return _this;
        }
        
        public void StartDrag(OverlayPanel movingObject)
        {
            MovingObject = movingObject;
            draggingStarted = true;
        }

        public void EndDrag()
        {            
            MovingObject = null;
            draggingStarted = false;
        }
        private void RootCanvas_PointerMoved(object? sender, PointerEventArgs e)
        {
            //Debug.WriteLine("ROOT Pointer Moved");
            if (draggingStarted)
            {                
                Canvas.SetLeft(MovingObject, e.GetPosition(MovingObject.Parent).X - MovingObject.FirstXPos);
                Canvas.SetTop(MovingObject, e.GetPosition(MovingObject.Parent).Y - MovingObject.FirstYPos);                
            }            
        }

        private void OverlappedWindow_Opened(object sender, EventArgs e)
        {
            Content = RootCanvas;
        }
        
        public Canvas RootCanvas
        {
            get => rootCanvas.Value.GetValueOrDefault();
            set => rootCanvas.OnNext(value);
        }

        /*
        public void SetVerticalAlign(Panel c, VerticalAlignment va)
        {
            switch (va)
            {
                case VerticalAlignment.Top:
                    Canvas.SetTop(c, 0);
                    break;
                case VerticalAlignment.Bottom:
                    Canvas.SetBottom(c, 0);
                    break;
                case VerticalAlignment.Center:
                    var b = c.Bounds.Height / 2;
                    Canvas.SetTop(c, ((MaxHeight) / 2) - b);
                    break;
                case VerticalAlignment.Stretch:
                    Canvas.SetTop(c, 0);
                    c.Height = MaxHeight;
                    break;
            }
        }

        public void SetHorizontalAlign(Panel c, HorizontalAlignment ha)
        {
            switch (ha)
            {
                case HorizontalAlignment.Left:
                    Canvas.SetLeft(c, 0);
                    break;
                case HorizontalAlignment.Right:
                    Canvas.SetRight(c, 0);
                    break;
                case HorizontalAlignment.Center:
                    var b = c.Bounds.Width / 2;
                    Canvas.SetLeft(c, ((MaxWidth) / 2) - b);
                    break;
                case HorizontalAlignment.Stretch:
                    Canvas.SetLeft(c, 0);
                    c.Width = MaxWidth;
                    break;
            }
        }
        */

        public void AddUserControlToOverlay(
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
            bool isMoveable = false)
        {

            OverlayUserControl uc = new OverlayUserControl(userControl, mouseEnter, mouseLeave);

            OverlayPanel c = new OverlayPanel();
            
            
            if (va != null)
            {
                c.VerticalAlignment = (VerticalAlignment)va;
            }
            else
            {
                c.VerticalAlignment = VerticalAlignment.Top;
            }

            if (ha != null)
            {
                c.HorizontalAlignment = (HorizontalAlignment)ha;
            }
            else
            {
                c.HorizontalAlignment = HorizontalAlignment.Left;
            }
            

            //Margins
            c.Margin = new Thickness(marginLeft, marginTop, marginRight, marginBottom);

            c.ZIndex = 0;
            c.Background = Brushes.Transparent;
            //c.Height = userControl.Height;
            //c.Width = userControl.Width;                                
            c.Opacity = opacity;
            c.IsMoveable = isMoveable;
            
            c.OverlayUC = uc;
            c.Children.Add(uc.userControl);
            
            RootCanvas.Children.Add((IControl)c);

        }

        public void UpdateOverlayChildsPositions(IControl rootChild)
        {
            var childs = rootChild.GetVisualDescendants().OfType<OverlayPanel>();
            foreach (var child in childs)
            {
                //var topLeft = new Point();
                var topLeft = child.PanelTopLeft;

                if (child?.IsArrangeValid == true)
                {
                    switch (child.HorizontalAlignment)
                    {
                        case HorizontalAlignment.Left:
                            //topLeft = topLeft.WithX(0);
                            if (child.PanelTopLeft.X > Bounds.Width)
                                topLeft = topLeft.WithX(Bounds.Width - child.Bounds.Width);
                            else
                                topLeft = topLeft.WithX(child.PanelTopLeft.X);
                            break;
                        case HorizontalAlignment.Right:
                            //Canvas.SetRight(child, 0);
                            topLeft = topLeft.WithX(Bounds.Width - child.Bounds.Width);
                            break;

                        case HorizontalAlignment.Center:
                            //var b = child.Width / 2;
                            //Canvas.SetLeft(child, ((Bounds.Width) / 2) - b);
                            topLeft = topLeft.WithX((Bounds.Width - child.Bounds.Width) / 2);
                            break;

                        case HorizontalAlignment.Stretch:
                            Canvas.SetLeft(child, 0);
                            child.Width = Bounds.Width;
                            break;
                    }

                    switch (child.VerticalAlignment)
                    {
                        case VerticalAlignment.Top:
                            //topLeft = topLeft.WithY(0);
                            if (child.PanelTopLeft.Y > Bounds.Height)
                                topLeft = topLeft.WithY(Bounds.Height - child.Bounds.Height);
                            else
                                topLeft = topLeft.WithY(child.PanelTopLeft.Y);
                            break;
                        case VerticalAlignment.Bottom:
                            //Canvas.SetBottom(child, 0);
                            topLeft = topLeft.WithY(Bounds.Height - child.Bounds.Height);
                            break;

                        case VerticalAlignment.Center:
                            //var b = child.Height / 2;
                            //Canvas.SetTop(child, ((Bounds.Height) / 2) - b);
                            topLeft = topLeft.WithY((Bounds.Height - child.Bounds.Height) / 2);
                            break;

                        case VerticalAlignment.Stretch:
                            Canvas.SetTop(child, 0);
                            child.Height = Bounds.Height;
                            break;
                    }
                }
                Canvas.SetTop(child, topLeft.Y);
                Canvas.SetLeft(child, topLeft.X);
            }
        }
    }
}
