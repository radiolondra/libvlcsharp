using Avalonia.Controls;
using Avalonia.Input;
using AvaVLCMultipleControls.ViewModels;


namespace AvaVLCMultipleControls.Views
{
    public partial class MainWindow : Window
    {        
        public MainWindowViewModel? viewModel;
        

        public MainWindow()
        {
            InitializeComponent();
            
            viewModel = new MainWindowViewModel();
            DataContext = viewModel;            

            if (!Avalonia.Controls.Design.IsDesignMode)
            {               
                Opened += MainWindow_Opened;
            }

        }
        
        private void MainWindow_Opened(object? sender, System.EventArgs e)
        {                        
            var tmp = PlayerControl.GetInstance();
            tmp.SetPlayerHandle();
            // ----------------------------------------            

            if (tmp._videoViewer != null)
            {
                // Add custom controls
                // -------------------

                // Add control defined in VideoView XAML as content (if any)
                tmp._videoViewer.AddXAMLContentIfAny(false);

                // Add some user controls
                tmp._videoViewer.OverlayWindow.AddUserControlToOverlay(
                    new ImageControl(),
                    myPointerEnteredEvent,
                    myPointerExitedEvent,
                    Avalonia.Layout.VerticalAlignment.Bottom,
                    Avalonia.Layout.HorizontalAlignment.Right,
                    0, 0, 0, 0, 0.8, false);


                tmp._videoViewer.OverlayWindow.AddUserControlToOverlay(
                    new PlayerControls(),
                    null,
                    null,
                    Avalonia.Layout.VerticalAlignment.Top,
                    Avalonia.Layout.HorizontalAlignment.Stretch,
                    0, 10, 0, 0, 0.8, true);


                tmp._videoViewer.OverlayWindow.AddUserControlToOverlay(
                    new ImageControl(),
                    myPointerEnteredEvent,
                    myPointerExitedEvent,
                    Avalonia.Layout.VerticalAlignment.Center,
                    Avalonia.Layout.HorizontalAlignment.Center,
                    0, 0, 0, 0, 0.8, false);



                tmp._videoViewer.OverlayWindow.AddUserControlToOverlay(
                    new ImageControl(),
                    null,
                    null,
                    Avalonia.Layout.VerticalAlignment.Bottom,
                    Avalonia.Layout.HorizontalAlignment.Left,
                    0, 0, 0, 0, 0.8, true);

                tmp._videoViewer.OverlayWindow.AddUserControlToOverlay(
                    new ImageControl(),
                    null,
                    null,
                    null,
                    null,
                    50, 80, 0, 0, 0.8, true);
            }
        }

        private void myPointerEnteredEvent(object sender)
        {            
            ((UserControl)sender).Opacity = 0.8;
        }

        private void myPointerExitedEvent(object sender)
        {
            ((UserControl)sender).Opacity = 0;
        }
    }
    
}
