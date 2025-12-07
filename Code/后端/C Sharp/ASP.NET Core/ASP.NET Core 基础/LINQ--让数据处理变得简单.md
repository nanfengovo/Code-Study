#  简单算法引入：
统计一个字符串中每个字母出现的频率（忽略大小写），然后安装从高到低的顺序输出出现频率高于2次的单词和其出现的频率
## 实现
### 不使用Linq
```
namespace LINQ1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             统计一个字符串中每个字母出现的频率（忽略大小写），然后按照从高到低的顺序输出出现频率高于2次的单词和其出现的频率
             */
            GetStringCountAndFrequency("AAaa1122CCcVDDDSSS32");
        }

        /// <summary>
        /// 不借助Linq
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static void GetStringCountAndFrequency(string str)
        {
            string frequency = "";
            //1、过滤字母
            string s = FilterString(str);
            //2、忽略大小写
            s = s.ToLower();
            //3、统计每个字母出现的次数
            Dictionary<char, double> dict = new Dictionary<char, double>();
            foreach (char c in s)
            {
                if (dict.ContainsKey(c))
                {
                    dict[c]++;
                }
                else
                {
                    dict[c] = 1.0;
                }
            }
            //4、筛选出现频率高于2次的字母
            Dictionary<char, double> resDict = new Dictionary<char, double>();
            Dictionary<char,string> newDict = new Dictionary<char,string>();
            foreach (var dic in dict)
            {
                if(dic.Value >= 2)
                {
                    resDict.Add(dic.Key, dic.Value);
                    frequency = Math.Round(dic.Value / str.Length * 100, 2) + "%";
                    newDict.Add(dic.Key,frequency);
                }
            }
            //5、按照出现频率从高到低排序
            var sortedDict = from pair in newDict orderby pair.Value descending select pair;
            foreach (var item in sortedDict)
            {
                Console.WriteLine($"字母：{item.Key}，出现频率：{item.Value}");
            }

        }

        static string FilterString(string str)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    //过滤空
                    string str1 = str.Trim();
                    //过滤非字母
                    string str2 = "";
                    foreach(char c in str1)
                    {
                        if (char.IsLetter(c))
                        {
                            str2 += c.ToString();
                        }
                    }
                    return str2;
                }
                Console.WriteLine("输入的字符串不能为空！！！");
                throw new ArgumentNullException();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
```

### 使用Linq
```
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

```

# 委托 →lambda→LINQ
## 委托
1、委托是可以指向方法的类型，调用委托变量时执行的就是变量指向的方法
2、.NET 中定义了泛型委托Action（无返回值）和Func(有返回值)，所以一般不用自定义委托类型
# LINQ 原理
