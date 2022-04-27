using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaVLCMultipleControls.Views
{
    public partial class ImageControl : UserControl
    {
        public ImageControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
