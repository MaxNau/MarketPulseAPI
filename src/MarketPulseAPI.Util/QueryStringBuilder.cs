using System.Net;
using System.Text;

namespace MarketPulseAPI.Util
{
    public static class QueryStringBuilder
    {
        public static string Build(params (string? key, string? value)[] parameters)
        {
            var queryString = new StringBuilder();
            bool isFirst = true;

            foreach (var (key, value) in parameters)
            {
                if (!string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(key))
                {
                    if (isFirst)
                    {
                        queryString.Append('?');
                        isFirst = false;
                    }
                    else
                    {
                        queryString.Append('&');
                    }

                    queryString.Append($"{key}={WebUtility.UrlEncode(value)}");
                }
            }

            return queryString.ToString();
        }
    }
}
