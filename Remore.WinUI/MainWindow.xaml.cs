using Microsoft.UI.Windowing;
using Microsoft.UI;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;
using Remore.WinUI.Helpers;
using Windows.Graphics;
using Windows.UI.ViewManagement;
using WinRT.Interop;
using Microsoft.UI.Xaml.Media;

namespace Remore.WinUI;

public sealed partial class MainWindow : WindowEx
{
    private Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue;

    private UISettings settings;

    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Title = "AppDisplayName".GetLocalized();

        var titleBar = this.AppWindow.TitleBar;
        titleBar.ExtendsContentIntoTitleBar = true;
        titleBar.ButtonBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        titleBar.ButtonForegroundColor = ((SolidColorBrush)App.Current.Resources["WindowCaptionForeground"]).Color;
        AppTitleBar.Loaded += AppTitleBar_Loaded;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        // Theme change code picked from https://github.com/microsoft/WinUI-Gallery/pull/1239
        dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        settings = new UISettings();
        settings.ColorValuesChanged += Settings_ColorValuesChanged; // cannot use FrameworkElement.ActualThemeChanged event
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        SetDragRegionForCustomTitleBar(this.AppWindow);
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        SetDragRegionForCustomTitleBar(this.AppWindow);
    }

    private void Settings_ColorValuesChanged(UISettings sender, object args)
    {
        // This calls comes off-thread, hence we will need to dispatch it to current app's thread
        dispatcherQueue.TryEnqueue(() =>
        {
            TitleBarHelper.ApplySystemThemeToCaptionButtons();
        });
    }


    [DllImport("Shcore.dll", SetLastError = true)]
    internal static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    internal enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    private double GetScaleAdjustment()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
    {
        if (AppWindowTitleBar.IsCustomizationSupported() && appWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            double scaleAdjustment = GetScaleAdjustment();

            Windows.Graphics.RectInt32[] dragRects = new[]
            {
                new RectInt32(0, 0, (int)AppTitleBar.ActualWidth, (int)AppTitleBar.ActualHeight)
            };

            appWindow.TitleBar.SetDragRectangles(dragRects);
        }
    }
}
