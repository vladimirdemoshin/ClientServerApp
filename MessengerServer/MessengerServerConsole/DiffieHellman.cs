using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace ServerConsole
{
    static class DiffieHellman
    {
        private static int MILLER_RABIN_CERTAINTY = 10;
        public static BigInteger RandomPrimeBigInteger(BigInteger maxValue)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[maxValue.ToByteArray().LongLength];
            BigInteger result;
            do
            {
                do
                {
                    rng.GetBytes(bytes);
                    result = BigInteger.Abs(new BigInteger(bytes));
                } while (result > maxValue);
            } while (!DiffieHellman.IsProbablePrime(result, MILLER_RABIN_CERTAINTY));
            return result;
        }
        public static bool IsProbablePrime(BigInteger source, int certainty)
        {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;
            BigInteger d = source - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;
            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= source - 2);
                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }
                if (x != source - 1)
                    return false;
            }
            return true;
        }
        public static String encryptRS4(String key, String message)
        {
            String binaryMessage = toBinary(message);
            String binaryKey = toBinary(key);
            while (binaryKey.Length < binaryMessage.Length)
            {
                binaryKey += 0;
            }
            return XOR(binaryKey, binaryMessage);
        }
        public static String decryptRS4(BigInteger key, String encryptedMessage)
        {
            String binaryKey = toBinary(key.ToString());
            while (binaryKey.Length < encryptedMessage.Length)
            {
                binaryKey += 0;
            }
            return binaryToString(XOR(binaryKey, encryptedMessage));
        }
        private static string toBinary(String input)
        {
            String binaryOutput = "";
            for (int i = 0; i < input.Length; i++)
            {
                String binaryChar = Convert.ToString(input[i], 2);
                while (binaryChar.Length < 16)
                    binaryChar = "0" + binaryChar;
                binaryOutput += binaryChar;
            }
            return binaryOutput;
        }
        public static String XOR(String key, String message)
        {
            String output = "";
            for (int i = 0; i < message.Length; i++)
            {
                if (key[i] == message[i])
                    output += '0';
                else output += '1';
            }
            return output;
        }
        private static String binaryToString(String binaryInput) // в символе будет по 2 байта , длина входа кратна 16
        {
            String output = "";
            for (int i = 0; i < binaryInput.Length; i += 16)
            {
                int number = 0;
                String binaryNumber = binaryInput.Substring(i,16);
                for (int j = binaryNumber.Length - 1; j >= 0; j--)
                {
                    int digit = binaryNumber[j] - '0';
                    number += digit * (int)Math.Pow(2, binaryNumber.Length - 1 - j);
                }
                output += (char)number;
            }
            return output;
        }
    }
}
