using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Interactivity;
using System;
using System.IO;
using System.Collections.Generic;

namespace katalog;

public partial class ListScreen : UserControl
{
    private readonly string _dataFilePath;

    public event Action<CollectionItem>? ItemSelected;
    public event Action? AddRequested;

    public ListScreen()
        : this(Array.Empty<CollectionItem>(), Path.Combine("dane", "pozycje.txt"))
    {
    }

    public ListScreen(IReadOnlyList<CollectionItem> items, string dataFilePath)
    {
        InitializeComponent();
        _dataFilePath = dataFilePath;
        ShowItems(items);
    }

    private void ShowItems(IReadOnlyList<CollectionItem> items)
    {
        ItemsPanel.Children.Clear();

        foreach (var item in items)
        {
            var button = new Button
            {
                Content = $"{item.Title} ({item.Year})",
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(12)
            };
            button.Click += (_, _) => ItemSelected?.Invoke(item);
            ItemsPanel.Children.Add(button);
        }

        if (items.Count == 0)
        {
            ItemsPanel.Children.Add(new TextBlock { Text = "Brak pozycji w kolekcji." });
        }
    }

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        AddRequested?.Invoke();
    }

    private void OnCountClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            var lines = File.ReadAllLines(_dataFilePath);
            CountText.Text = $"Pozycji w pliku: {lines.Length}";
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException)
        {
            CountText.Text = $"Nie udało się odczytać pliku: {exception.Message}";
        }
    }
}
