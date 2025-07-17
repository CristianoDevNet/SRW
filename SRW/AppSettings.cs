using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SRW;

public class AppSettings : INotifyPropertyChanged
{
    private bool _enabled;
    
    private int _advanceMinutes = 5;
    
    private List<string> _timesList = new();
    
    private string _customMessage = "Faltam {minutos} minuto(s) para o reset.";

    public bool Enabled
    {
        get => _enabled;
        set { _enabled = value; OnPropertyChanged(); }
    }

    public int AdvanceMinutes
    {
        get => _advanceMinutes;
        set { _advanceMinutes = value; OnPropertyChanged(); }
    }

    public List<string> TimesList
    {
        get => _timesList;
        set { _timesList = value; OnPropertyChanged(); }
    }

    public string CustomMessage
    {
        get => _customMessage;
        set { _customMessage = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}