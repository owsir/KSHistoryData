using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KsHistoryData
{
    public class KsDataHelper
    {
        public static List<DataModel> GetData(string html)
        {
            var result=new List<DataModel>();
            var datas = "";
            Regex reg = new Regex(@"<div\s+id=""box"">([\S\s]*?)<div\s+class=""kj_gongg_bottom"">", RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(html);

            foreach (Match match in matches)
            {
                Regex reg2 = new Regex(@"<table(.+?)bgcolor=""#cccccc"">([\S\s]*?)<div\s+class=""kj_gongg_bottom"">", RegexOptions.Multiline);
                MatchCollection matches2 = reg2.Matches(match.Value);
                datas = matches2[0].Value;
            }

            Regex reg3 = new Regex(@"<tr>([\S\s]*?)</tr>\s+</tbody>\s+</table>\s+</td>\s+</tr>", RegexOptions.Multiline);
            MatchCollection matches3 = reg3.Matches(datas);

            var index = 0;
            foreach (Match match in matches3)
            {
                index++;
                Regex reg4 = new Regex(@"<td>([\S\s]*?)</td>", RegexOptions.Multiline);
                Regex reg5 = new Regex(@"<td\s([\S\s]*?)>([\S\s]*?)</td>", RegexOptions.Multiline);

                MatchCollection matches4 = reg4.Matches(match.Value);
                MatchCollection matches5 = reg5.Matches(match.Value);

                string regexstr = @"<[^>]*>";
                var searilValue = matches5[0].Value;
                if (index == 1)
                {
                    searilValue = matches5[3].Value;
                }

                var serial = Regex.Replace(searilValue, regexstr, string.Empty, RegexOptions.IgnoreCase);
                var value = "";
                foreach (Match matchItem in matches4)
                {
                    var str = Regex.Replace(matchItem.Value, regexstr, string.Empty, RegexOptions.IgnoreCase);
                    value += str.Trim();
                }

                result.Add(new DataModel()
                {
                    WareIssue = serial.Trim(),
                    WareResult = value.Trim()
                });
            }
            return result;
        }
    }
}
