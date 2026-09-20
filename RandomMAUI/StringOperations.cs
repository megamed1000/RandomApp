using System.Text;
namespace RandomMAUI;
public class StringOperations
{
    public static string FormattedArray1D(int[] result)
    {
        StringBuilder sb = new();
        for (int i = 0; i < result.Length; i++)
        {
            sb.Append(result[i]);
            if (i < result.Length - 1) sb.Append(", ");
        }
        return sb.ToString();
    }
    public static string FormattedArray2D(int[,] result)
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
