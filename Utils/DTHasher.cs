using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace IranianExperts
{
  public static class DTHasher
  {
    private static System.IO.FileStream GetFileStream(string pathName)
    {
      return (new System.IO.FileStream(pathName, System.IO.FileMode.Open,
                System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite));
    }

    public static string GetFileHash(string fileName, HashAlgorithm algorithm)
    {
      using (var oFileStream = GetFileStream(fileName))
      {
        var arrbytHashValue = algorithm.ComputeHash(oFileStream);

        var strHashData = BitConverter.ToString(arrbytHashValue);

        return strHashData.Replace("-", "");
      }
    }

    public static string GetSHA1Hash(string fileName)
    {
      return GetFileHash(fileName, new SHA1CryptoServiceProvider());
    }

    public static string GetMD5Hash(string fileName)
    {
      return GetFileHash(fileName, new MD5CryptoServiceProvider());
    }
  }
}
