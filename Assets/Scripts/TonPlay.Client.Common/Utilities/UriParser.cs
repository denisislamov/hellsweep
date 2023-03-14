using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public static class UriParser
{
    // UriManager.Instance.Parse(Application.absoluteURL);
    // ServerEnvironment.Token = UriManager.Instance.QueryParsed["token"];

    /// <summary>
    /// Parse URL and return dictionary with query params
    /// </summary>
    /// <param name="url">Url for parsing. Use Application.absoluteURL in our case</param>
    /// <returns>Dictionary with query params (key/value). Use "token" in our case</returns>
    public static Dictionary<string, string> Parse(string url)
    {
        var request =  UnityWebRequest.Get(url);
        
        var fullQuery = request.uri.Query;
        if (fullQuery.Length <= 0)
        {
            return null;
        } 
        fullQuery = fullQuery.Substring(1);
        
        var result = new List<string>();
        result = fullQuery.Split('&').ToList();
        
        var queryParsed = new Dictionary<string, string>();
        foreach (var s in result.Select(r => r.Split('=')))
        {
            queryParsed.Add(s[0], s[1]);
        }
        return queryParsed;
    }
}
