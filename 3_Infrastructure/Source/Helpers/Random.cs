namespace Infrastructure.Helpers;

public class RandomNumeric()
{
  private static string RandomNumericString(Random random, int length)
  {
    const string chars = "0123456789";
    char[] randomChars = new char[length];

    for (int i = 0; i < length; i++)
    {
      randomChars[i] = chars[random.Next(chars.Length)];
    }

    return new string(randomChars);
  }
  public static string Generate(int length)
  {
    Random random = new();
    return RandomNumericString(random, length);
  }
}

public class RandomString()
{
  private static string RandomStrings(Random random, int length)
  {
    const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghifjklmnopqrstuvwxyz";
    char[] randomChars = new char[length];

    for (int i = 0; i < length; i++)
    {
      randomChars[i] = chars[random.Next(chars.Length)];
    }

    return new string(randomChars);
  }
  public static string Generate(int length)
  {
    Random random = new();
    return RandomStrings(random, length);
  }
}
