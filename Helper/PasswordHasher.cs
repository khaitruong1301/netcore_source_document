using System;
using System.Security.Cryptography;

public class PasswordHasher
{
    public static string HashPassword(string password)
    {
        // Tạo salt ngẫu nhiên
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        // Sử dụng Rfc2898DeriveBytes để tạo hash
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        // Kết hợp salt và hash
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        // Chuyển đổi thành chuỗi base64
        string hashedPassword = Convert.ToBase64String(hashBytes);
        return hashedPassword;
    }
    public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
{
    // Convert the stored hashed password from base64 to byte array
    byte[] hashBytes = Convert.FromBase64String(storedHashedPassword);

    // Extract the salt from the stored hashed password
    byte[] salt = new byte[16];
    Array.Copy(hashBytes, 0, salt, 0, 16);

    // Hash the entered password using the same salt and algorithm
    var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
    byte[] hash = pbkdf2.GetBytes(20);

    // Compare the entered password hash with the stored hash
    for (int i = 0; i < 20; i++)
    {
        if (hashBytes[i + 16] != hash[i])
        {
            return false;
        }
    }

    return true;
}

}
