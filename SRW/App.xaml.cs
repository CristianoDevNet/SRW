using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using HandyControl.Themes;
using Hardcodet.Wpf.TaskbarNotification;
using SRW.Views;

namespace SRW
{
    public partial class App : Application
    {
        private OverlayWindow? _overlay;
        
        private AppSettings _settings = new();
        
        private TaskbarIcon? _trayIcon;
        
        private string _lastMessage = "";

        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        
        [DllImport("user32.dll")] static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        
        [DllImport("user32.dll")] static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
        
        [DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr hwnd);

        const uint MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
           
            public RECT rcMonitor;
            
            public RECT rcWork;
            
            public uint dwFlags;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
         
            _settings = SettingsManager.Load();
            
            _trayIcon = (TaskbarIcon)FindResource("TrayIcon");
            
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    try { MonitorSCUM(); }
                    catch (Exception ex) { Debug.WriteLine("Erro no monitoramento: " + ex.Message); }
                    
                    Thread.Sleep(1000);
                }
            })
            {
                IsBackground = true
            };
            
            thread.SetApartmentState(ApartmentState.STA);
            
            thread.Start();
        }

        private void MonitorSCUM()
        {
            var proc = Process.GetProcessesByName("SCUM").FirstOrDefault();
            
            if (proc == null || proc.MainWindowHandle == IntPtr.Zero)
            {
                HideOverlay();
            
                return;
            }

            var hwnd = proc.MainWindowHandle;

            // Debug extra para entender foco
            Debug.WriteLine($"[SRW DEBUG] IsTopmost: {IsTopmost(hwnd)}");

            if (!IsWindowVisible(hwnd) || !IsFullscreen(hwnd) || !IsTopmost(hwnd))
            {
                HideOverlay();
                
                return;
            }

            _settings = SettingsManager.Load();
            
            if (!_settings.Enabled || _settings.TimesList.Count == 0)
            {
                HideOverlay();
            
                return;
            }

            var now = DateTime.Now;
            
            bool overlayAtivado = false;

            foreach (var timeStr in _settings.TimesList)
            {
                if (TimeSpan.TryParse(timeStr, out var targetTime))
                {
                    var targetDateTime = now.Date + targetTime;
            
                    if (targetDateTime < now)
                        targetDateTime = targetDateTime.AddDays(1);

                    var diff = targetDateTime - now;
                    
                    var totalMinutes = diff.TotalMinutes;
                    
                    var totalSeconds = (int)Math.Ceiling(diff.TotalSeconds);

                    Debug.WriteLine($"[SRW DEBUG] Hora atual: {now:HH:mm:ss} | Alvo: {targetDateTime:HH:mm:ss} | Diferença: {totalMinutes:F2} min");

                    if (totalMinutes >= 0 && totalMinutes <= _settings.AdvanceMinutes)
                    {
                        overlayAtivado = true;

                        Debug.WriteLine($"[SRW DEBUG] Mensagem será ativada. Minutos restantes: {totalMinutes:F2} min");

                        string msg;

                        if (totalMinutes <= 1)
                        {
                            if (totalSeconds % 10 == 0)
                            {
                                msg = $"Faltam {totalSeconds} segundo(s) para o reset.";
                    
                                Debug.WriteLine("ATUALIZANDO MSG (10s): " + msg);
                                
                                UpdateOverlay(hwnd, msg);
                            }
                        }
                        else
                        {
                            int minutosRestantes = (int)Math.Ceiling(totalMinutes);
                            
                            msg = _settings.CustomMessage.Replace("{minutos}", minutosRestantes.ToString());
                            
                            Debug.WriteLine("ATUALIZANDO MSG (minutos): " + msg);
                            
                            UpdateOverlay(hwnd, msg);
                        }
                    }
                }
            }

            if (!overlayAtivado)
            {
                HideOverlay();
            }
        }

        private void UpdateOverlay(IntPtr hwnd, string message)
        {
            Dispatcher.Invoke(() =>
            {
                if (_overlay == null)
                {
                    _overlay = new OverlayWindow();
                  
                    _overlay.Show();
                }
                else if (!_overlay.IsVisible)
                {
                    _overlay.Show(); // Garante que reapareça após ter sido ocultado
                }

                if (GetWindowRect(hwnd, out RECT rect))
                {
                    _overlay.Left = rect.Left;
                 
                    _overlay.Top = rect.Top + 20;
                    
                    _overlay.Width = rect.Right - rect.Left;
                    
                    _overlay.Height = 100;
                }

                _overlay.Topmost = true;

                if (message != _lastMessage)
                {
                    _overlay.SetMessage(message);
                    _lastMessage = message;
                }
            });
        }

        private void HideOverlay()
        {
            Dispatcher.Invoke(() =>
            {
                if (_overlay != null)
                {
                    _overlay.Hide();
                    
                    _lastMessage = "";
                }
            });
        }

        private bool IsFullscreen(IntPtr hwnd)
        {
            if (!GetWindowRect(hwnd, out RECT rect))
                return false;

            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            
            MONITORINFO mi = new() { cbSize = Marshal.SizeOf<MONITORINFO>() };
            
            if (!GetMonitorInfo(monitor, ref mi))
                return false;

            return rect.Left == mi.rcMonitor.Left &&
                   rect.Top == mi.rcMonitor.Top &&
                   rect.Right == mi.rcMonitor.Right &&
                   rect.Bottom == mi.rcMonitor.Bottom;
        }

        private bool IsTopmost(IntPtr hwnd)
        {
            return GetForegroundWindow() == hwnd;
        }

        // ==== Menu Tray Handlers ====

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            
            win.Show();
            
            win.Activate();
        }

        private void OnAboutClick(object sender, RoutedEventArgs e)
        {
            var win = new AboutWindow { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            
            win.Show();
            
            win.Activate();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            _overlay?.Close();
            
            _trayIcon?.Dispose();
            
            Shutdown();
        }

        private void OnTrayIconDoubleClick(object sender, RoutedEventArgs e)
        {
            OnSettingsClick(sender, e);
        }
    }
}
