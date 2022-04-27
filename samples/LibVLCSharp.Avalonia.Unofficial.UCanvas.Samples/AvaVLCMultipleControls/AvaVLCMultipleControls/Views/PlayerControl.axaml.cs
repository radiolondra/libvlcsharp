using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvaVLCMultipleControls.ViewModels;
using LibVLCSharp.Avalonia.UCanvas;
using System;
using System.Diagnostics;

namespace AvaVLCMultipleControls.Views
{
    public partial class PlayerControl : UserControl
    {
        private static PlayerControl _this;        
        public VideoView? _videoViewer;
        public PlayerControlModel viewModel;
        private Panel _panelContent;

        public PlayerControl()
        {
            InitializeComponent();
            
            _this = this;

            viewModel = new PlayerControlModel();
            DataContext = viewModel;

            _videoViewer = this.Get<VideoView>("VideoViewer");
            _panelContent = this.Get<Panel>("PanelContent");            

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static PlayerControl GetInstance()
        {
            return _this;
        }

        public void SetPlayerHandle()
        {
            if (_videoViewer != null && viewModel.MediaPlayer != null)
            {
                _videoViewer.MediaPlayer = viewModel.MediaPlayer;
                _videoViewer.MediaPlayer.Hwnd = _videoViewer.hndl.Handle;
            }
            
        }

        public void Content_PointerEnter(object sender, PointerEventArgs e)
        {
            //Debug.WriteLine("In Content_PointerEnter");
            _panelContent.Opacity = 0.8;
        }

        public void Content_PointerLeave(object sender, PointerEventArgs e)
        {
            //Debug.WriteLine("In Content_PointerLeave");
            _panelContent.Opacity = 0;
        }
    }
}
