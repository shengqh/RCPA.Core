using System.Collections.Generic;

namespace RCPA
{
  public class Annotation : IAnnotation
  {
    private Dictionary<string, object> _annotations = new Dictionary<string, object>();
    #region IAnnotation Members

    public Dictionary<string, object> Annotations
    {
      get { return _annotations; }
    }

    #endregion
  }
}
