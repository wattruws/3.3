using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Globalization;

namespace katalog;

public partial class AddScreen : UserControl
{
    public event Action<CollectionItem>? ItemSaved;
    public event Action? CancelRequested;

    public AddScreen()
    {
        InitializeComponent();
    }

    private void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        var item = CreateItem(out var error);
        if (item is null)
        {
            ErrorText.Text = error;
            return;
        }

        ItemSaved?.Invoke(item);
    }

    private CollectionItem? CreateItem(out string error)
    {
        var title = TitleInput.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(title) || title.Contains(';') || title.Contains('\n'))
        {
            error = "Podaj tytuł bez średnika i znaków nowej linii.";
            return null;
        }

        if (!int.TryParse(YearInput.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var year) ||
            year < 1)
        {
            error = "Podaj prawidłowy rok wydania.";
            return null;
        }

        if (!int.TryParse(RatingInput.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var rating) ||
            rating < 1 || rating > 10)
        {
            error = "Ocena musi być liczbą od 1 do 10.";
            return null;
        }

        error = string.Empty;
        return new CollectionItem(title, year, rating);
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        CancelRequested?.Invoke();
    }
}
