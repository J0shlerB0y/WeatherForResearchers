using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text; 

namespace UseCases.Services;

public class RandomStringGenerator : IStringGenerator
{
    private readonly char[] AllowedChars;

    public RandomStringGenerator(IConfiguration configuration)
    {
        AllowedChars = (configuration["AllowedChars"] ?? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789|!<>%_+-~^&*").ToCharArray();
    }

    public string GenerateString(int length = 16, char[]? allowedChars = null)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "The string length must be positive.");
        }

        char[] chars = allowedChars ?? AllowedChars;
        if (chars == null || chars.Length == 0)
        {
            throw new ArgumentException("The set of allowed characters cannot be empty.", nameof(allowedChars));
        }

        int charsetLength = chars.Length;
        StringBuilder result = new StringBuilder(length);
        byte[] randomBytes = new byte[length];

        RandomNumberGenerator.Fill(randomBytes);

        for (int i = 0; i < length; i++)
        {
            int index = randomBytes[i] % charsetLength;
            result.Append(chars[index]);
        }

        return result.ToString();
    }
}