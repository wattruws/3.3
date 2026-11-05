using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace katalog;

public partial class DetailsScreen : UserControl
{
    public event Action? BackRequested;

    public DetailsScreen()
    {
        InitializeComponent();
    }

    public DetailsScreen(CollectionItem item) : this()
    {
        TitleText.Text = item.Title;
        YearText.Text = $"Rok wydania: {item.Year}";
        RatingText.Text = $"Ocena: {item.Rating}/10";
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke();
    }
}
