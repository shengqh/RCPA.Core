using System.Collections.Generic;

namespace RCPA
{
  public interface IAnnotation
  {
    Dictionary<string, object> Annotations { get; }
  }
}