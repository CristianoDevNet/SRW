using System;

namespace SRW
{
    public static class SettingsChangedNotifier
    {
        public static event Action? SettingsChanged;

        public static void Notify()
        {
            SettingsChanged?.Invoke();
        }
    }
}