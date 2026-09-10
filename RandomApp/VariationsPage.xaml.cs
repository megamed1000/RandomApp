namespace RandomApp;

public partial class VariationsPage : ContentPage
{
	public VariationsPage()
	{
		InitializeComponent();
		Random r = new Random();
    }
	public void OnGenerateClicked(object sender, EventArgs e)
    {
		Random r = new Random();
		int min = int.Parse(minEntry.Text);
		int max = int.Parse(maxEntry.Text) + 1;
		int amount = int.Parse(amountEntry.Text);
		resultLabel.Text = string.Empty;
		for(int i = 0; i < amount; i++)
		{
			int result = r.Next(min, max);
			resultLabel.Text += result;
			if (i + 1 < amount) resultLabel.Text += ", ";
		}
    }
}