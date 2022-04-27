using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Layout;

using System.Text;
using Avalonia.VisualTree;
using System.Linq;

namespace LibVLCSharp.Avalonia.UCanvas
{
    public class OverlayUserControl
    {
        public UserControl userControl { get; set; }
        
        public Action<object> pointerEntered;
        public Action<object> pointerExited;

        public OverlayUserControl(UserControl uc, Action<object> mouseenter, Action<object> mouseleave)
        {
            userControl = uc;
            
            pointerEntered = mouseenter;
            pointerExited = mouseleave;
            userControl.PointerEnter += Control_PointerEnter;
            userControl.PointerLeave += Control_PointerLeave;
        }

        public void Control_PointerEnter(object sender, PointerEventArgs e)
        {
            Debug.WriteLine("In Control_PointerEnter");
            if (pointerEntered != null)
            {
                pointerEntered(sender);
            }
        }

        public void Control_PointerLeave(object sender, PointerEventArgs e)
        {
            Debug.WriteLine("In Control_PointerLeave");
            if (pointerExited != null)
            {
                pointerExited(sender);
            }

        }
    }

    // The panel container of OverlayUserControl
    public class OverlayPanel: Panel
    {
        public double FirstXPos, FirstYPos, FirstArrowXPos, FirstArrowYPos;                
        private Point _panelTopLeft;
        private OverlayUserControl _overlayUC;
        private bool _isMoveable;
        public OverlayPanel()
        {            
            this.AddHandler(PointerPressedEvent, OverlayPanel_PointerPressed, RoutingStrategies.Tunnel);
            this.AddHandler(PointerReleasedEvent, OverlayPanel_PointerReleased, RoutingStrategies.Tunnel);            
            //PointerEnter += OverlayPanel_PointerEnter;

            _panelTopLeft = new Point(0,0);
            _isMoveable = false;
        }

        public OverlayUserControl OverlayUC
        {
            get => _overlayUC;
            set => _overlayUC = value;
        }
        public Point PanelTopLeft
        {
            get => _panelTopLeft;
            set => _panelTopLeft = value;
        }

        public bool IsMoveable
        {
            get => _isMoveable;
            set => _isMoveable = value;
        }

        /*
        private void OverlayPanel_PointerEnter(object? sender, PointerEventArgs e)
        {
            //var rect = (sender as OverlayPanel).Bounds;
            //Debug.WriteLine($"Bounds: {rect.X}, {rect.Y}");
        }
        */

        private void OverlayPanel_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var tmp = OverlappedWindow.GetInstance();
            var root = (tmp.RootCanvas as IControl).GetVisualDescendants().OfType<OverlayPanel>();
            

            //Debug.WriteLine("Control Pointer Pressed");
            if ((sender as OverlayPanel).IsMoveable)
            {
                // set the control to move at top
                foreach (var child in root)
                {
                    child.ZIndex = 0;
                }
                (sender as OverlayPanel).ZIndex = 1;

                this.Cursor = new Cursor(StandardCursorType.SizeAll);

                // Resets margins
                (sender as OverlayPanel).Margin = new Thickness(0);

                // Resets V/H alignments to Top/Left
                (sender as OverlayPanel).VerticalAlignment = VerticalAlignment.Top;
                (sender as OverlayPanel).HorizontalAlignment = HorizontalAlignment.Left;

                // Resets W/H
                (sender as OverlayPanel).Width = (sender as OverlayPanel).OverlayUC.userControl.Width;
                (sender as OverlayPanel).Height = (sender as OverlayPanel).OverlayUC.userControl.Height;

                FirstXPos = e.GetPosition(sender as Control).X;
                FirstYPos = e.GetPosition(sender as Control).Y;
                FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
                FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
                
                tmp.StartDrag(sender as OverlayPanel);
            }
            
        }

        private void OverlayPanel_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            //Debug.WriteLine("Control Pointer Released");

            if ((sender as OverlayPanel).IsMoveable)
            {

                this.Cursor = Cursor.Default;

                var tmp = OverlappedWindow.GetInstance();

                var X = e.GetPosition((sender as OverlayPanel).Parent).X - FirstXPos;
                var Y = e.GetPosition((sender as OverlayPanel).Parent).Y - FirstYPos;

                (sender as OverlayPanel).PanelTopLeft = (sender as OverlayPanel).PanelTopLeft.WithX(X);
                (sender as OverlayPanel).PanelTopLeft = (sender as OverlayPanel).PanelTopLeft.WithY(Y);

                tmp.EndDrag();
            }
        }
        
    }
}
