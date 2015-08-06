using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.GZip;

namespace RCPA.Utils
{
  public static class HashUtils
  {
    private static FileStream GetFileStream(string pathName)
    {
      return new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    public static string GetStreamHash(Stream oFileStream, HashAlgorithm algorithm)
    {
      var arrbytHashValue = algorithm.ComputeHash(oFileStream);
      var strHashData = BitConverter.ToString(arrbytHashValue);
      return strHashData.Replace("-", "").ToLower();
    }

    public static string GetFileHash(string fileName, HashAlgorithm algorithm)
    {
      using (var oFileStream = GetFileStream(fileName))
      {
        return GetStreamHash(oFileStream, algorithm);
      }
    }

    public static string GetSHA1Hash(string fileName)
    {
      return GetFileHash(fileName, new SHA1CryptoServiceProvider());
    }

    /// <summary>
    /// Calculate MD5 value of file.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="forceCalculation">ignore cache file and re-calculate MD5 value</param>
    /// <param name="cacheMD5File">read/write cache MD5 file</param>
    /// <returns></returns>
    public static string DoGetMD5Hash(string fileName, Func<string, Stream> getStream, bool forceCalculation = false, bool cacheMD5File = true)
    {
      if (cacheMD5File)
      {
        var md5file = fileName + ".md5";
        if (forceCalculation || !File.Exists(md5file))
        {
          using (var stream = getStream(fileName))
          {
            var result = GetStreamHash(stream, new MD5CryptoServiceProvider());
            try
            {
              File.WriteAllText(md5file, result);
            }
            catch (Exception) { }

            return result;
          }
        }
        else
        {
          return File.ReadAllText(md5file).Trim();
        }
      }
      else
      {
        using (var stream = getStream(fileName))
        {
          return GetStreamHash(stream, new MD5CryptoServiceProvider());
        }
      }
    }

    /// <summary>
    /// Calculate MD5 value of file.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="forceCalculation">ignore cache file and re-calculate MD5 value</param>
    /// <param name="cacheMD5File">read/write cache MD5 file</param>
    /// <returns></returns>
    public static string GetMD5Hash(string fileName, bool forceCalculation = false, bool cacheMD5File = true)
    {
      return DoGetMD5Hash(fileName, m => new FileInfo(m).OpenRead(), forceCalculation, cacheMD5File);
    }

    public static string GetGzippedMD5Hash(string fileName, bool forceCalculation = false, bool cacheMD5File = true)
    {
      return DoGetMD5Hash(fileName, m => new GZipInputStream(File.OpenRead(m)), forceCalculation, cacheMD5File);
    }

    public static string GetZippedMD5Hash(string fileName, bool forceCalculation = false, bool cacheMD5File = true)
    {
      using (ZipArchive zipArchive = ZipFile.Open(fileName, ZipArchiveMode.Read))
      {
        if (zipArchive.Entries.Count == 0)
        {
          throw new Exception(string.Format("No file found in {0}, calculation MD5 failed.", fileName));
        }

        using (Stream stream = zipArchive.Entries[0].Open())
        {
          return HashUtils.GetStreamHash(stream, new MD5CryptoServiceProvider());
        }
      }
    }

    /// <summary>
    /// Get MD5 hash value of a file. If the file is compressed, the MD5 hash of decompressed file will be returned. Only gzip and zip compression mode supported.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="forceCalculation"></param>
    /// <param name="cacheMD5File"></param>
    /// <returns></returns>
    public static string GetDecompressedMD5Hash(string file, bool forceCalculation = false, bool cacheMD5File = true)
    {
      string md5;
      if (file.ToLower().EndsWith(".gz"))
      {
        md5 = HashUtils.GetGzippedMD5Hash(file);
      }
      else if (file.ToLower().EndsWith(".zip"))
      {
        md5 = HashUtils.GetZippedMD5Hash(file);

      }
      else
      {
        md5 = HashUtils.GetMD5Hash(file);
      }
      return md5;
    }
  }
}
