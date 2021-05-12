// Decompiled with JetBrains decompiler
// Type: Virafa.Crypto
// Assembly: Virafa, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 62191C1D-B899-4272-822B-5B931A797A4B
// Assembly location: C:\Users\hesam\Desktop\Virafa.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utilities
{
  public class Crypto
  {
    private static byte[] _salt = Encoding.ASCII.GetBytes("kahfghuw@#$@$JKSDHF#*47874");

    public static string EncryptStringAES(string plainText, string sharedSecret)
    {
      if (string.IsNullOrEmpty(plainText))
        throw new ArgumentNullException(nameof (plainText));
      if (string.IsNullOrEmpty(sharedSecret))
        throw new ArgumentNullException(nameof (sharedSecret));
      string str = (string) null;
      RijndaelManaged rijndaelManaged = (RijndaelManaged) null;
      try
      {
        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sharedSecret, Crypto._salt);
        rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          memoryStream.Write(BitConverter.GetBytes(rijndaelManaged.IV.Length), 0, 4);
          memoryStream.Write(rijndaelManaged.IV, 0, rijndaelManaged.IV.Length);
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream))
              streamWriter.Write(plainText);
          }
          str = Convert.ToBase64String(memoryStream.ToArray());
        }
      }
      finally
      {
        if (rijndaelManaged != null)
          rijndaelManaged.Clear();
      }
      return str;
    }

    public static string DecryptStringAES(string cipherText, string sharedSecret)
    {
      if (string.IsNullOrEmpty(cipherText))
        throw new ArgumentNullException(nameof (cipherText));
      if (string.IsNullOrEmpty(sharedSecret))
        throw new ArgumentNullException(nameof (sharedSecret));
      RijndaelManaged rijndaelManaged = (RijndaelManaged) null;
      string str = (string) null;
      try
      {
        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sharedSecret, Crypto._salt);
        using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
        {
          rijndaelManaged = new RijndaelManaged();
          rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
          rijndaelManaged.IV = Crypto.ReadByteArray((Stream) memoryStream);
          ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader streamReader = new StreamReader((Stream) cryptoStream))
              str = streamReader.ReadToEnd();
          }
        }
      }
      finally
      {
        if (rijndaelManaged != null)
          rijndaelManaged.Clear();
      }
      return str;
    }

    private static byte[] ReadByteArray(Stream s)
    {
      byte[] buffer1 = new byte[4];
      if (s.Read(buffer1, 0, buffer1.Length) != buffer1.Length)
        throw new SystemException("Stream did not contain properly formatted byte array");
      byte[] buffer2 = new byte[BitConverter.ToInt32(buffer1, 0)];
      if (s.Read(buffer2, 0, buffer2.Length) != buffer2.Length)
        throw new SystemException("Did not read byte array properly");
      return buffer2;
    }
  }
}
