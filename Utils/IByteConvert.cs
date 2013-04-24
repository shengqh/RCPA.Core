using System;
using System.Collections.Generic;
using System.Text;

namespace RCPA.Utils
{
  public interface IByteConvert
  {
    /// <summary>
    /// Convert data bytes to double array
    /// </summary>
    /// <param name="dataBytes">source data bytes</param>
    /// <returns>double array</returns>
    double[] ByteToArray(byte[] dataBytes);

    /// <summary>
    /// Validate length of dataBytes.
    /// </summary>
    /// <param name="dataBytes">source data bytes</param>
    /// <exception cref="ArgumentException">when length of dataBytes not suit for convert</exception>
    void ValidateLength(byte[] dataBytes);

    /// <summary>
    /// Validate length of dataBytes with
    /// </summary>
    /// <param name="dataBytes"></param>
    /// <param name="expectLength"></param>
    /// <exception cref="ArgumentException">when length of dataBytes not suit for convert or not equals with expect length</exception>
    void ValidateLength(byte[] dataBytes, int expectLength);
  }
}
