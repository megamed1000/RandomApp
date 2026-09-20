using RandomLibrary;
using System.Text;
namespace RandomMAUI;

public partial class ProbabilityPage : ContentPage
{
    public static readonly List<string> pickerOptions = ["Numbers"];// TODO: "From preset"
    public ProbabilityPage()
    {
        InitializeComponent();
    }
    private void OnGenerateClicked(object sender, EventArgs e)
    {
        int[] results;
        resultLabel.Text = "OOF";
        if (int.TryParse(minEntry.Text, out int nn)) resultLabel.Text = "TO OK";
        if (int.TryParse(minEntry.Text, out int min) && int.TryParse(maxEntry.Text, out int max) && double.TryParse(probabilityEntry.Text, out double percentage))
        {
            results = [.. RandomGenerator.GenerateNumericProbability(min, max, percentage/100)];
            resultLabel.Text = StringOperations.FormattedArray1D(results);
        }
    }
    private static string FormattedResult(int[] result)
    {
        StringBuilder sb = new();
        for (int i = 0; i < result.Length; i++)
        {
            sb.Append(result[i]);
            if (i < result.Length - 1) sb.Append(", ");
        }
        return sb.ToString();
    }
}