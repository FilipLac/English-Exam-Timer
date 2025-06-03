using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.ApplicationModel;
using WinRT.Interop;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace English_Exam_Timer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        private Window? m_window;
        public static Window? MainAppWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new Window
            {
                Content = new MainPage()
            };
            MainAppWindow = m_window; // <-- Přidáno
            m_window.Activate();
            SetWindowIcon(@"Assets\icon.ico");
        }

        public void SetWindowIcon(string iconRelativePath)
        {
            if (m_window == null)
                return;

            try
            {
                IntPtr hWnd = WindowNative.GetWindowHandle(m_window);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
                AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

                string fullPath = System.IO.Path.Combine(Package.Current.InstalledLocation.Path, iconRelativePath);
                appWindow.SetIcon(fullPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Chyba při nastavování ikony: {ex.Message}");
            }
        }
    }
}
