using System.Text;

namespace RandomApp;

public partial class VariationsPage : ContentPage
{  
    public static readonly List<string> pickerOptions = ["Numbers", "From preset"];
	readonly Random r = new();
    public VariationsPage()
	{
        InitializeComponent();
		swUniqueOneRow.IsToggled =Preferences.Get("UniqueOneRow", false);
        swUniqueAllRows.IsToggled = Preferences.Get("UniqueAllRows", false);
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
        Preferences.Set("UniqueAllRows", swUniqueAllRows.IsToggled);
		Preferences.Set("UniqueOneRow", swUniqueOneRow.IsToggled);
        if (modePicker.SelectedIndex == 0)
		{
			if (swUniqueOneRow.IsToggled)
			{
				if (swUniqueAllRows.IsToggled)
				{
                    
                    List<int> availableNumbers = Enumerable.Range(min, max - min + 1).ToList();
                    StringBuilder sb = new();
                    for (int rn = 1; rn <= rows; rn++)//row number
                    {
                        sb.Append($"Row {rn}: ");

                        

                        
                        for (int ri = 0; ri < amount; ri++)//row index
                        {
                            int index = r.Next(availableNumbers.Count);
                            int result = availableNumbers[index];
                            availableNumbers.RemoveAt(index);
                            sb.Append(result);
                            if (ri + 1 < amount) sb.Append(", ");
                        }
                        if (rn < rows) sb.Append('\n');
                    }
                    resultLabel.Text = sb.ToString();
                }
				else
				{
                    StringBuilder sb = new();
                    
                    

                    for (int rn = 1; rn <= rows; rn++)//row number
                    {
                        sb.Append($"Row {rn}: ");
                        
                        List<int> availableNumbers = Enumerable.Range(min, max - min + 1).ToList();
                        for (int ri = 0; ri < amount; ri++)//row index
                        {
                            int index = r.Next(availableNumbers.Count);
                            int result = availableNumbers[index];
                            availableNumbers.RemoveAt(index);
                            sb.Append(result);
                            if (ri + 1 < amount) sb.Append(", ");
                        }
                        if (rn < rows) sb.Append('\n');
                    }
                    resultLabel.Text = sb.ToString();
                }
			}
			else
			{
                StringBuilder sb = new();
                for (int rn = 1; rn <= rows; rn++)//row number
                {
                    sb.Append($"Row {rn}: ");
                    int[] results = r.GetItems<int>(Enumerable.Range(min, max - min + 1).ToArray(), amount);
                    for (int ri = 0; ri < amount; ri++)//row index
                    {
                        int result = results[ri];//excludes max + 1
                        sb.Append(result);
                        if (ri + 1 < amount) sb.Append(", ");
                    }
                    if (rn < rows) sb.Append('\n');
                }
                resultLabel.Text = sb.ToString();
            }
		}
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