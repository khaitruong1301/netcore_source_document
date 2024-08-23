using System;
using System.Text.Json;

public static class JSON
{
    // Phương thức Parse: Giải nén chuỗi JSON thành đối tượng
    public static T Parse<T>(string jsonString)
    {
        try
        {
            // Sử dụng System.Text.Json để giải nén
            return JsonSerializer.Deserialize<T>(jsonString);
        }
        catch (JsonException ex)
        {
            // Xử lý lỗi khi chuỗi không phải là JSON hợp lệ
            Console.WriteLine("JSON Parse Error: " + ex.Message);
            return default;
        }
    }

    // Phương thức Stringify: Nén đối tượng thành chuỗi JSON
    public static string Stringify(object obj)
    {
        try
        {
            // Sử dụng System.Text.Json để nén đối tượng thành JSON
            return JsonSerializer.Serialize(obj);
        }
        catch (Exception ex)
        {
            // Xử lý lỗi khi đối tượng không thể chuyển đổi thành JSON
            Console.WriteLine("JSON Stringify Error: " + ex.Message);
            return null;
        }
    }
}
