using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace katalog;

public partial class MainWindow : Window
{
    private static readonly string DataFilePath = Path.Combine("dane", "pozycje.txt");

    public MainWindow()
    {
        InitializeComponent();
        ShowList();
    }

    private void ShowList()
    {
        try
        {
            var screen = new ListScreen(ReadItems(), DataFilePath);
            screen.ItemSelected += ShowDetails;
            screen.AddRequested += ShowAddForm;
            ScreenHost.Content = screen;
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException or FormatException)
        {
            ShowError($"Nie udało się wczytać pliku {DataFilePath}: {exception.Message}");
        }
    }

    private List<CollectionItem> ReadItems()
    {
        var items = new List<CollectionItem>();
        var lines = File.ReadAllLines(DataFilePath, Encoding.UTF8);

        for (var index = 0; index < lines.Length; index++)
        {
            if (string.IsNullOrWhiteSpace(lines[index]))
            {
                continue;
            }

            var fields = lines[index].Split(';');
            if (fields.Length != 3 ||
                !int.TryParse(fields[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var year) ||
                !int.TryParse(fields[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var rating))
            {
                throw new FormatException($"Nieprawidłowy format w wierszu {index + 1}.");
            }

            items.Add(new CollectionItem(fields[0], year, rating));
        }

        return items;
    }

    private void ShowDetails(CollectionItem item)
    {
        var screen = new DetailsScreen(item);
        screen.BackRequested += ShowList;
        ScreenHost.Content = screen;
    }

    private void ShowAddForm()
    {
        var screen = new AddScreen();
        screen.CancelRequested += ShowList;
        screen.ItemSaved += SaveItem;
        ScreenHost.Content = screen;
    }

    private void SaveItem(CollectionItem item)
    {
        try
        {
            var line = $"{item.Title};{item.Year};{item.Rating}{Environment.NewLine}";
            File.AppendAllText(DataFilePath, line, new UTF8Encoding(false));
            ShowList();
        }
        catch (Exception exception) when (
            exception is IOException or UnauthorizedAccessException)
        {
            ShowError($"Nie udało się zapisać pozycji: {exception.Message}");
        }
    }

    private void ShowError(string message)
    {
        ScreenHost.Content = new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Avalonia.Thickness(24)
        };
    }
}