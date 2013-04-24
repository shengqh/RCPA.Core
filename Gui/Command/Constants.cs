using System;

namespace RCPA.Gui.Command
{
  public class Constants
  {
    public static String sqhContact = "Quanhu Sheng (shengqh@gmail.com)";

    public static String GetSQHTitle(String title, String version)
    {
      return FormatTitle(title, version, sqhContact);
    }

    public static String FormatTitle(String title, String version,
                                     String authorContact)
    {
      return title + " - " + version + " - " + authorContact + " - RCPA/SIBS";
    }

    public static String GetSqhVanderbiltTitle(String title, String version)
    {
      return title + " - " + version + " - Quanhu SHENG (quanhu.sheng@vanderbilt.edu/shengqh@gmail.com) - CQS/VUMC";
    }

  }
}