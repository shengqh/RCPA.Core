using System;
using System.Collections.Generic;
using System.Text;

namespace RCPA.Utils
{
  public class ByteConvert : IByteConvert
  {
    private IByteConvert convert;

    public ByteConvert(bool isBigEndian, int precision)
    {
      if (precision != 32 && precision != 64)
      {
        throw new ArgumentException("Precision should be 32 or 64.", "precision");
      }

      if (isBigEndian)
      {
        if (32 == precision)
        {
          convert = new ConvertBigEndian32();
        }
        else
        {
          convert = new ConvertBigEndian64();
        }
      }
      else
      {

        if (32 == precision)
        {
          convert = new ConvertLittleEndian32();
        }
        else
        {
          convert = new ConvertLittleEndian64();
        }
      }
    }

    #region IByteConvert Members

    public double[] ByteToArray(byte[] dataBytes)
    {
      return convert.ByteToArray(dataBytes);
    }

    public void ValidateLength(byte[] dataBytes)
    {
      convert.ValidateLength(dataBytes);
    }

    public void ValidateLength(byte[] dataBytes, int expectLength)
    {
      convert.ValidateLength(dataBytes, expectLength);
    }

    #endregion
  }

  internal abstract class AbstractByteConvert : IByteConvert
  {
    protected AbstractByteConvert(int[] indeies)
    {
      this.Indeies = indeies;
      this.Offsets = new int[indeies.Length];
      for (int i = 0; i < indeies.Length; i++)
      {
        Offsets[i] = i * 8;
      }
    }

    protected int BaseLength { get { return Indeies.Length; } }

    protected int[] Indeies { get; set; }

    protected int[] Offsets { get; set; }

    #region IByteConvert Members

    public abstract double[] ByteToArray(byte[] dataBytes);

    public void ValidateLength(byte[] dataBytes)
    {
      if (null == dataBytes)
      {
        throw new ArgumentNullException("dataBytes");
      }

      if (dataBytes.Length % BaseLength != 0)
      {
        throw new ArgumentException("Byte data array had length " + dataBytes.Length + " which isn't a multiple of 8.", "dataBytes");
      }
    }

    public void ValidateLength(byte[] dataBytes, int expectLength)
    {
      ValidateLength(dataBytes);

      int actualLength = dataBytes.Length / BaseLength;
      if (actualLength != expectLength)
      {
        throw new ArgumentException(MyConvert.Format("Expected data length {0} is not equals to actual length {1}.", expectLength, actualLength), "dataBytes");
      }
    }

    #endregion
  }

  internal abstract class AbstractConvertEndian32 : AbstractByteConvert
  {
    protected AbstractConvertEndian32(int[] indeies) : base(indeies) { }

    #region IByteConvert Members

    public override double[] ByteToArray(byte[] dataBytes)
    {
      ValidateLength(dataBytes);

      double[] result = new double[dataBytes.Length / BaseLength];

      for (int i = 0; i <= (dataBytes.Length - BaseLength); i += BaseLength)
      {
        int myInt = dataBytes[i + Indeies[0]] & 0xff;
        for (int j = 1; j < BaseLength - 1; j++)
        {
          myInt = myInt | ((dataBytes[i + Indeies[j]] & 0xff) << Offsets[j]);
        }
        myInt = myInt | ((dataBytes[i + Indeies[BaseLength - 1]]) << Offsets[BaseLength - 1]);

        result[i / BaseLength] = (double)MathUtils.Int32BitsToSingle(myInt);
      }
      return result;
    }

    #endregion
  }

  internal abstract class AbstractConvertEndian64 : AbstractByteConvert
  {
    protected AbstractConvertEndian64(int[] indeies) : base(indeies) { }

    #region IByteConvert Members

    public override double[] ByteToArray(byte[] dataBytes)
    {
      ValidateLength(dataBytes);

      double[] result = new double[dataBytes.Length / BaseLength];

      for (int i = 0; i <= (dataBytes.Length - BaseLength); i += BaseLength)
      {
        long myLong = dataBytes[i + Indeies[0]] & 0xff;
        for (int j = 1; j < BaseLength - 1; j++)
        {
          myLong = myLong | (((long)(dataBytes[i + Indeies[j]] & 0xff)) << Offsets[j]);
        }
        myLong = myLong | ((long)(dataBytes[i + Indeies[BaseLength - 1]]) << Offsets[BaseLength - 1]);

        result[i / BaseLength] = BitConverter.Int64BitsToDouble(myLong);
      }
      return result;
    }

    #endregion
  }

  internal class ConvertBigEndian32 : AbstractConvertEndian32
  {
    public ConvertBigEndian32() : base(new int[] { 3, 2, 1, 0 }) { }
  }
  
  internal class ConvertBigEndian64 : AbstractConvertEndian64
  {
    public ConvertBigEndian64() : base(new int[] { 7, 6, 5, 4, 3, 2, 1, 0 }) { }
  }

  internal class ConvertLittleEndian32 : AbstractConvertEndian32
  {
    public ConvertLittleEndian32() : base(new int[] { 0, 1, 2, 3 }) { }
  }

  internal class ConvertLittleEndian64 : AbstractConvertEndian64
  {
    public ConvertLittleEndian64() : base(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }) { }
  }
}
