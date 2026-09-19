using RandomLibrary;
using System.Text;
namespace RandomMAUI;

public partial class VariationsPage : ContentPage
{
    public static readonly List<string> pickerOptions = ["Numbers"];// TODO: "From preset"
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
    private void OnUniqueOneRowToggled(object sender, ToggledEventArgs e)
    {
        if (!swUniqueOneRow.IsToggled) swUniqueAllRows.IsToggled = false;
    }
    private void OnUniqueAllRowsToggled(object sender, ToggledEventArgs e)
    {
        if (swUniqueAllRows.IsToggled) swUniqueOneRow.IsToggled = true;
    }
    private void OnGenerateClicked(object sender, EventArgs e)
    {
        if (modePicker.SelectedIndex == 0)
		{
            if (!(int.TryParse(minEntry.Text, out int min) && int.TryParse(maxEntry.Text, out int max) && int.TryParse(amountEntry.Text, out int amount) && int.TryParse(rowsEntry.Text, out int rows)))
            {
                DisplayAlertAsync("Error", "Please enter valid integer values for min, max, amount, and rows.", "OK");
                return;
            }
            if(min > max)
            {
                DisplayAlertAsync("Error", "Min cannot be greater than max.", "OK");
                return;
            }
            if(swUniqueOneRow.IsToggled && amount > (max - min + 1))
            {
                DisplayAlertAsync("Error", "Not enough unique numbers available for the given parameters.", "OK");
                return;
            }
            if(swUniqueAllRows.IsToggled && amount * rows > (max - min + 1))
            {
                DisplayAlertAsync("Error", "Not enough unique numbers available for the given parameters.", "OK");
                return;
            }
            int[,] result;
            if (swUniqueAllRows.IsToggled)
            {
                result = RandomGenerator.GenerateNumericVariations(min, max, amount, rows, NumericVariationOptions.UniqueInAllVariations);
            }
            else if(swUniqueOneRow.IsToggled)
            {
                result = RandomGenerator.GenerateNumericVariations(min, max, amount, rows, NumericVariationOptions.UniqueInOneVariation);
            }
            else
            {
                result = RandomGenerator.GenerateNumericVariations(min, max, amount, rows, NumericVariationOptions.WithRepetition);
            }
            resultLabel.Text = FormattedResult(result);
        }
        SavePreferences();
    }
    private static string FormattedResult(int[,] result)
    {
        StringBuilder sb = new();
        for (int i = 0; i < result.GetLength(0); i++)
        {
            sb.Append($"Row {i + 1}: ");
            for (int j = 0; j < result.GetLength(1); j++)
            {
                sb.Append(result[i, j]);
                if (j < result.GetLength(1) - 1) sb.Append(", ");
            }
            if (i < result.GetLength(0) - 1) sb.AppendLine();
        }
        return sb.ToString();
    }
}