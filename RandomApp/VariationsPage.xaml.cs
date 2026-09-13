using System.Text;
namespace RandomApp;

public partial class VariationsPage : ContentPage
{  
    public static readonly List<string> pickerOptions = ["Numbers", "From preset"];
	readonly Random r = new();
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
    public void OnGenerateClicked(object sender, EventArgs e)
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
            int[,] result = GenerateNumbersRows(min, max, amount, rows, swUniqueOneRow.IsToggled, swUniqueAllRows.IsToggled);
            resultLabel.Text = FormattedResult(result);
        }
        SavePreferences();
	}
    private int[,] GenerateNumbersRows(int min, int max, int amount, int rows, bool uniqueInOneRow, bool uniqueAmongAllRows)
    {
        int[,] result = new int[rows, amount];

        if (min > max) throw new ArgumentException("Min cannot be greater than max.");
        if (uniqueAmongAllRows)
        {
            if (amount * rows > max - min + 1) throw new ArgumentException("Not enough unique numbers available for the given parameters.");

            List<int> availableNumbers = [.. Enumerable.Range(min, max - min + 1)];
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < amount; j++)
                {
                    int index = r.Next(availableNumbers.Count);
                    result[i, j] = availableNumbers[index];
                    availableNumbers.RemoveAt(index);
                }
            }
        }
        else if (uniqueInOneRow)
        {
            if (amount > max - min + 1) throw new ArgumentException("Not enough unique numbers available for the given parameters.");         
            for (int i = 0; i < rows; i++)
            {
                List<int> availableNumbers = [.. Enumerable.Range(min, max - min + 1)];
                for (int j = 0; j < amount; j++)
                {
                    int index = r.Next(availableNumbers.Count);
                    result[i, j] = availableNumbers[index];
                    availableNumbers.RemoveAt(index);
                }
            }
        }
        else
        {   
            for (int i = 0; i < rows; i++)
            {
                for(int j = 0; j < amount; j++)
                {
                    result[i, j] = r.Next(min, max + 1);
                }
            }
        }
        return result;
    }
    static string FormattedResult(int[,] result)
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