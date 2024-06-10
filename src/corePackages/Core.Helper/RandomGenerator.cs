namespace Core.Helper;

public static class RandomGenerator
{
    /// <summary>
    /// Rastgele lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_- karakterleri içeren string değer döndürecek
    /// </summary>
    /// <param name="length">karakter sayısı</param>
    /// <returns>lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_- karakterleri içeren string değer döndürecek</returns>
    public static string CreateMixRandom(int length = 14)
    {
        var validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        return Generator(length, validChars);
    }
    /// <summary>
    /// Rastgele lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789 karakterleri içeren string değer döndürecek
    /// </summary>
    /// <param name="length">karakter sayısı</param>
    /// <returns>lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789 karakterleri içeren string değer döndürecek</returns>
    public static string CreateRandomCode(int length = 6)
    {
        var validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        return Generator(length, validChars);
    }

    private static string Generator(int length, string validChars)
    {
        var random = new Random();

        var chars = new char[length];
        for (var i = 0; i < length; i++)
        {
            chars[i] = validChars[random.Next(0, validChars.Length)];
        }

        return new string(chars);
    }
    /// <summary>
    /// Rastgele lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz karakterleri içeren string değer döndürecek
    /// </summary>
    /// <param name="length">karakter sayısı</param>
    /// <returns>lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz karakterleri içeren string değer döndürecek</returns>
    public static string CreateCharRandom(int length = 14)
    {
        var validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return Generator(length, validChars);
    }
    /// <summary>
    /// Rastgele lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 karakterleri içeren string değer döndürecek
    /// </summary>
    /// <param name="length">karakter sayısı</param>
    /// <returns>lenght kadar ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 karakterleri içeren string değer döndürecek</returns>
    public static string CreateCharAndNumberRandom(int length = 14)
    {
        var validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return Generator(length, validChars);
    }
    /// <summary>
    /// Rastgele lenght kadar !@#$%^&*?_- karakterleri içeren string değer döndürecek
    /// </summary>
    /// <param name="length">karakter sayısı</param>
    /// <returns>lenght kadar !@#$%^&*?_- karakterleri içeren string değer döndürecek</returns>
    public static string CreateSymbolRandom(int length = 14)
    {
        var validChars = "!@#$%^&*?_-";
        return Generator(length, validChars);
    }
    /// <summary>
    /// Rastgale lenght kadar belirli aralıkta int değer döndürecek
    /// </summary>
    /// <param name="min">minimum başlangıç değeri</param>
    /// <param name="max">maximum bitiş değeri</param>
    /// <returns>lenght kadar belirli aralıkta int değer döndürecek</returns>
    public static int CreateNumberRandom(int min = 100000, int max = 999999)
    {
        var random = new Random();
        return random.Next(min, max);
    }
    public static DateTime CreateDate()
    {
        DateTime dateTime = DateTime.Now;
        Random random = new Random();
        Double hours = Convert.ToDouble(random.Next(0, 24));
        return dateTime.AddHours(hours);
    }
}
