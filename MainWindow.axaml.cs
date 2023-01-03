using Avalonia.Controls;
using CAEManager.ViewModels;

namespace CAEManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(new Services.CAEService());
    }
}