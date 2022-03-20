﻿namespace RCPA.Seq
{
  public interface IAccessNumberParser : IStringParser<string>
  {
    string FormatName { get; }

    string RegexPattern { get; }

    bool TryParse(string obj, out string value);
  }
}
