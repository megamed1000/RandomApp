using System.Text;

namespace RandomApp;

public partial class VariationsPage : ContentPage
{  
    public static readonly List<string> pickerOptions = ["Numbers", "From preset"];
	readonly Random r = new();
    public VariationsPage()
	{
        InitializeComponent();
		swUniqueOneRow.IsToggled =Preferences.Default.Get("UniqueOneRow", false);
        swUniqueAllRows.IsToggled = Preferences.Default.Get("UniqueAllRows", false);
    }

	public void OnGenerateClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(minEntry.Text, out int min))
        {
            return;
        }
        if (!int.TryParse(maxEntry.Text, out int max))
        {
            return;
        }
        if (!int.TryParse(amountEntry.Text, out int amount))
        {
            return;
        }
        if (!int.TryParse(rowsEntry.Text, out int rows))
        {
            return;
        }
        Preferences.Default.Set("UniqueAllRows", swUniqueAllRows.IsToggled);
		Preferences.Default.Set("UniqueOneRow", swUniqueOneRow.IsToggled);
        if (modePicker.SelectedIndex == 0)
		{
            StringBuilder sb = new();
            int[,] result = GenerateRows(min, max, amount, rows, swUniqueOneRow.IsToggled, swUniqueAllRows.IsToggled);
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < amount; j++)
                {
                    sb.Append(result[i, j]);
                    if (j < amount - 1) sb.Append(", ");
                }
                if (i < rows - 1) sb.AppendLine();
            }
            resultLabel.Text = sb.ToString();
        }
	}
    private int[,] GenerateRows(int min, int max, int amount, int rows, bool uniqueInOneRow, bool uniqueAmongAllRows)
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
    private void OnUniqueOneRowToggled(object sender, ToggledEventArgs e)
    {
		if (!swUniqueOneRow.IsToggled) swUniqueAllRows.IsToggled = false;
    }
    private void OnUniqueAllRowsToggled(object sender, ToggledEventArgs e)
    {
		if(swUniqueAllRows.IsToggled) swUniqueOneRow.IsToggled = true; 
    }
}