using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Newtonsoft;

namespace KsHistoryData
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var dataCount = ConfigurationManager.AppSettings["dataNumberOneTime"];
                //var url = "http://www.woying.com/kaijiang/jxk3ls/100.html";
                var url = $"http://soa.woying.com/draw/gaopin/Index?lotteryId=120&count={dataCount}";
                var html = HTMLHelper.GetHttpData(url);

                //var dataList = KsDataHelper.GetData(html);
                var dataList = JsonHelper.DeserializeJsonToList<DataModel>(html);


                var top1 = MysqlHelper.ExecuteScalar("select * from historydata ORDER BY serial DESC limit 1");
                var newItems = new List<DataModel>();
                if (top1 != null)
                {
                    var serial = top1.ToString();
                    if (dataList.Select(x => x.WareIssue).ToList().Contains(serial))
                    {
                        var index = dataList.Select(x => x.WareIssue).ToList().IndexOf(serial);
                        if (index > 0)
                        {
                            newItems = dataList.Take(index).ToList();
                        }
                    }
                    else
                    {
                        newItems = dataList;
                    }
                }
                else
                {
                    newItems = dataList;
                }
                if (newItems.Any())
                {
                    foreach (var item in newItems)
                    {
                        Console.Write(item.WareIssue);
                        Console.Write(item.WareResult);
                        Console.WriteLine();
                    }
                    MysqlHelper.ExecuteNonQuery(GetBatchInsertSql(newItems));
                }
                else
                {
                    Console.WriteLine("0");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }


            Console.ReadKey();
        }

        private static string GetBatchInsertSql(IEnumerable<DataModel> dataList)
        {
            return dataList.Aggregate("", (current, item) => current + $"insert into historydata select '{item.WareIssue}','{item.WareResult}';");
        }
    }

}
