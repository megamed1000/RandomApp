using RandomLibrary;
namespace RandomMAUI;
public partial class ProbabilityPage : ContentPage
{
    public ProbabilityPage()
    {
        InitializeComponent();
    }
    private void OnGenerateClicked(object sender, EventArgs e)
    {
        int[] results;
        if (int.TryParse(minEntry.Text, out int min) && int.TryParse(maxEntry.Text, out int max) && double.TryParse(probabilityEntry.Text, out double percentage))
        {
            if (percentage < 0 || percentage > 100)
            {
                DisplayAlertAsync("Incorrect format", "probability must be between 0 and 100", "Ok");
                return;
            }
            results = [.. RandomGenerator.GenerateNumericProbability(min, max, percentage / 100)];
            resultLabel.Text = StringOperations.FormattedArray1D(results);
        }
        else DisplayAlertAsync("Incorrect format", "", "Ok");
    }
}