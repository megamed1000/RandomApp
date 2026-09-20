using RandomLibrary;
namespace RandomMAUI;
public partial class VariationsPage : ContentPage
{
    public VariationsPage()
	{
		InitializeComponent();
        GetPreferences();
    }
    void GetPreferences()
    {
        swUniqueOneRow.IsToggled = Preferences.Default.Get("UniqueOneRow", false);
        swUniqueAllRows.IsToggled = Preferences.Default.Get("UniqueAllRows", false);
    }
    void SavePreferences()
    {
        Preferences.Default.Set("UniqueOneRow", swUniqueOneRow.IsToggled);
        Preferences.Default.Set("UniqueAllRows", swUniqueAllRows.IsToggled);
    }
    private void OnUniqueOneRowToggled(object? sender, ToggledEventArgs e)
    {
        if (!swUniqueOneRow.IsToggled) swUniqueAllRows.IsToggled = false;
    }
    private void OnUniqueAllRowsToggled(object? sender, ToggledEventArgs e)
    {
        if (swUniqueAllRows.IsToggled) swUniqueOneRow.IsToggled = true;
    }
    private void OnGenerateClicked(object? sender, EventArgs e)
    {
        bool error = false;
        string message = string.Empty;
        if (!(int.TryParse(minEntry.Text, out int min) && int.TryParse(maxEntry.Text, out int max) && int.TryParse(amountEntry.Text, out int amount) && int.TryParse(rowsEntry.Text, out int rows)))
        {
            DisplayAlertAsync("Wrong format", "Please enter valid integer values for min, max, amount, and rows", "Ok");
            return;
        }
        else
        {
            if (min > max)
            {
                error = true;
                message += "\nMin cannot be greater than max.";
            }
            if (swUniqueOneRow.IsToggled && amount > (max - min + 1))
            {
                error = true;
                message += "\nNot enough unique numbers available for the given parameters.";
            }
            if (swUniqueAllRows.IsToggled && amount * rows > (max - min + 1))
            {
                error = true;
                message += "\n\"Not enough unique numbers available for the given parameters.";
            }
            if (amount < 1 || rows < 1)
            {
                error = true;
                message += "\nAmount and rows must be at least 1.";
            }
        }
        if (error)
        {
            DisplayAlertAsync("Wrong input", message, "Ok");
            return;
        }
        int[,] result;
        if (swUniqueAllRows.IsToggled)
        {
            result = RandomGenerator.GenerateNumericVariations(min, max, amount, rows, NumericVariationOptions.UniqueInAllVariations);
        }
        else if (swUniqueOneRow.IsToggled)
        {
            result = RandomGenerator.GenerateNumericVariations(min, max, amount, rows, NumericVariationOptions.UniqueInOneVariation);
        }
        else
        {
            result = RandomGenerator.GenerateNumericVariations(min, max, amount, rows, NumericVariationOptions.WithRepetition);
        }
        resultLabel.Text = StringOperations.FormattedArray2D(result);
        SavePreferences();
    }
}