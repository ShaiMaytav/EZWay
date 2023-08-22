using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper 
{
    public static void OpenURLLink(string urlLink)
    {
        Application.OpenURL(urlLink);
    }

    public static string ReverseString(string _string)
    {
        char[] charArray = _string.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
}
