using System.Text.RegularExpressions;

namespace RCPA.Seq
{
  /// <summary>
  /// 根据指定氨基酸进行序列重置。即把该氨基酸与序列中其前面（forward为true）或者后面氨基酸进行交换。
  /// </summary>
  public class PseudoSequenceBuilder
  {
    private Regex findReg;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="aminoacids">需要颠倒的氨基酸列表</param>
    /// <param name="forward">与前置还是后置氨基酸交换，true为前置。</param>
    public PseudoSequenceBuilder(string aminoacids, bool forward)
    {
      if (forward)
      {
        findReg = new Regex(MyConvert.Format(@"(\S)([{0}])", aminoacids));
      }
      else
      {
        findReg = new Regex(MyConvert.Format(@"([{0}])(\S)", aminoacids));
      }
    }

    public void Build(Sequence seq)
    {
      seq.SeqString = findReg.Replace(seq.SeqString, "$2$1");
    }
  }
}
