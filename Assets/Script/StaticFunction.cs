using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StaticFunction : MonoBehaviour
{
    public static int TrueRandom(int minValue, int maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be less than maxValue.");
        }

        // Generate a random 32-bit integer using RandomNumberGenerator
        int randomNumber;
        byte[] randomBytes = new byte[4];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            randomNumber = BitConverter.ToInt32(randomBytes, 0);
        }

        // Map the random number to the desired range
        return Math.Abs(randomNumber % (maxValue - minValue)) + minValue;
    }
    public static float TrueRandom(float minValue, float maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be less than maxValue.");
        }

        // Generate a random 32-bit integer using RandomNumberGenerator
        float randomNumber;
        byte[] randomBytes = new byte[4];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            randomNumber = BitConverter.ToInt32(randomBytes, 0);
        }

        // Map the random number to the desired range
        return Math.Abs(randomNumber % (maxValue - minValue)) + minValue;
    }
}
