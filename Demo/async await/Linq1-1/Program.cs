namespace Linq1_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            统计一个字符串中每个字母出现的频率（忽略大小写），然后按照从高到低的顺序输出出现频率高于2次的单词和其出现的频率
            */
            Console.WriteLine("请输入字符串");
            var s = Console.ReadLine() ?? string.Empty;
            var item = s.Where(c => char.IsLetter(c))//过滤非字母
                          .Select(c => char.ToLower(c))//忽略大小写
                          .GroupBy(c => c)//分组
                          .Where(g => g.Count() > 2)//筛选出现频率高于2次的字母
                          .OrderByDescending(g => g.Count())//按照出现频率从高到低排序
                          .Select(g => new {Char = g.Key,Count = g.Count()});
        }
    }
}
