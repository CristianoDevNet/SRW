using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SRW.Views
{
    public partial class SettingsWindow : HandyControl.Controls.Window
    {
        public ObservableCollection<TimeEntry> Times { get; set; } = new();

        private AppSettings _appSettings = new();

        public SettingsWindow()
        {
            InitializeComponent();

            lvTimes.ItemsSource = Times;

            btnAddTime.Click += BtnAddTime_Click;

            LoadSettings();
        }

        private void LoadSettings()
        {
            _appSettings = SettingsManager.Load();

            chkEnabled.IsChecked = _appSettings.Enabled;

            txtAdvanceMinutes.Text = _appSettings.AdvanceMinutes.ToString();
            txtCustomMessage.Text = _appSettings.CustomMessage ?? "";

            Times.Clear();

            var sortedTimes = _appSettings.TimesList
                .OrderBy(t => TimeSpan.Parse(t));

            foreach (var t in sortedTimes)
            {
                Times.Add(new TimeEntry { Time = t });
            }
        }

        private void BtnAddTime_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtHour.Text, out int hour) || hour < 0 || hour > 23)
            {
                MessageBox.Show("Informe uma hora válida (0-23).", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtMinute.Text, out int minute) || minute < 0 || minute > 59)
            {
                MessageBox.Show("Informe um minuto válido (0-59).", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newTime = $"{hour:D2}:{minute:D2}";

            if (Times.Any(t => t.Time == newTime))
            {
                MessageBox.Show("Esse horário já foi adicionado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Times.Add(new TimeEntry { Time = newTime });

            var sorted = Times.OrderBy(t => TimeSpan.Parse(t.Time)).ToList();
            Times.Clear();
            foreach (var item in sorted)
                Times.Add(item);

            txtHour.Clear();
            txtMinute.Clear();
        }

        private void BtnRemoveTime_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is TimeEntry timeEntry)
            {
                Times.Remove(timeEntry);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _appSettings.Enabled = chkEnabled.IsChecked ?? false;

            if (int.TryParse(txtAdvanceMinutes.Text, out int adv))
                _appSettings.AdvanceMinutes = adv;
            else
                _appSettings.AdvanceMinutes = 5;

            string customMessage = txtCustomMessage.Text.Trim();

            if (!customMessage.Contains("{minutos}"))
            {
                MessageBox.Show("A mensagem personalizada deve conter o marcador {minutos}.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _appSettings.CustomMessage = customMessage;

            _appSettings.TimesList = Times
                .OrderBy(t => TimeSpan.Parse(t.Time))
                .Select(t => t.Time)
                .ToList();

            SettingsManager.Save(_appSettings);

            SettingsChangedNotifier.Notify();

            MessageBox.Show("Configurações salvas com sucesso!", "Salvar", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class TimeEntry
    {
        public string Time { get; set; } = "";

        public override bool Equals(object? obj)
        {
            if (obj is TimeEntry other)
                return Time == other.Time;
            return false;
        }

        public override int GetHashCode() => Time.GetHashCode();

        public override string ToString() => Time;
    }
}
