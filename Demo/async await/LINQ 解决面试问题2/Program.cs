namespace LINQ_解决面试问题2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //统计一个字符串中每个字母出现的频率（忽略大小写），然后按从高到低的顺序输出出现频率高于两次的单词和其出现的频率

            Console.WriteLine("请输入字符串！！");
            var str = Console.ReadLine();

            //过滤字母
            var str1 = str.Where(x => char.IsLetter(x));
            //忽略大小写
            var str2  = str1.Select(c => char.ToLower(c));
            //字母个数
            int sum = str2.Count();
            //出现次数高于2次
            Dictionary<char,int> dic = new Dictionary<char, int>();
            int count = 0;
            foreach (var item in str2)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    // 字母首次出现，初始化次数为1
                    dic.Add(item, 1);
                }
            }

            // 步骤5：筛选出现次数>2次的字母，并计算频率（保留2位小数）
            // 用匿名类型存储：字母、次数、频率，方便后续排序
            var filteredList = new List<(char Letter, int Count, double Frequency)>();
            foreach (var kvp in dic)
            {
                if (kvp.Value > 2) // 筛选：出现次数高于2次
                {   
                    // 计算频率：(出现次数 / 总字母数) * 100，保留2位小数
                    double frequency = Math.Round((double)kvp.Value / sum * 100, 2);
                    filteredList.Add((kvp.Key, kvp.Value, frequency));
                }
            }

            // 步骤6：按频率从高到低排序（频率相同则按字母升序，可选优化）
            var sortedList = filteredList.OrderByDescending(x => x.Frequency)
                                         .ThenBy(x => x.Letter);

            foreach (var item in sortedList)
            {
                Console.WriteLine($"{item.Letter}\t{item.Count}\t\t{item.Frequency}%");
            }

            //var res = str
            //                .Where(x => char.IsLetter(x))
            //                .Select(c => char.ToLower(c))
            //                .GroupBy(c => c)
            //                .Select(g => new { g.Key, Count = g.Count() })
            //                .OrderByDescending(x => x.Count)
            //                .Where(x => x.Count > 2);

            //foreach (var item in res)
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}
