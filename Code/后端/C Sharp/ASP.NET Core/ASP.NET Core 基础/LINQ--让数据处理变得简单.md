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
# LINQ where 原理
## 自己封装where
```
namespace LINQ_原理_where
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //找出大于10的
            int[] nums = new int[] { 3, 5, 3453, 33, 2, 9, 35 };
            var res = MyWhere2(nums, a => a >= 10);
            foreach (var item in res)
            {
                Console.WriteLine(item);
            }
        }

        static IEnumerable<int> MyWhere1(IEnumerable<int> items, Func<int,bool> f)
        {
            List<int> result = new List<int>();
            foreach (int i in items)
            {
                if (f(i))
                {
                    result.Add(i);
                }
            }
            return result;
        }

        static IEnumerable<int> MyWhere2(IEnumerable<int> items, Func<int, bool> f)
        {
            foreach (int i in items)
            {
                if (f(i))
                {
                    yield return i;
                }
            }
        }
    }
}
```
# Linq中常见的扩展方法
> 实现了IEnumerable 接口的方法都可以用 linq

## Where方法
> 每一个数据都会经过predicate的测试，如果针对一个元素，predicate执行的返回值为true，那么这个元素就会放到返回值中
> where参数是一个lambda表达式格式的匿名方法，方法的参数e表示当前判断的元素对象。参数的名字不一定非要叫e，不过一般lambda表达式中的变量名长度都不长

## Count  --获取总的数量 （返回int）
数量
## Any  -- 判断是否有一条（返回bool）
是否有一条，如果有返回true

## Count 和Any 判断是否存在

> 使用Any更优，因为count 需要全部遍历，Any找到一条就返回了

## 获取一条数据的方法
### Single  --有且只有一条满足要求的数据(如果一条都没有或者有多条都会报错)
### SingleOrDefault --最多只有1条满足要求的数据（多条会报错，0条则返回默认值，默认值是null）
### First --至少有一条，返回第一条（一条都没有报错）
### FirstOrDefault --返回第一条或者默认值


## 排序
### Order()  --对数据进行正序排序
### OrderByDescending() -- 倒序排序

### 多规则排序
> 可以在Order(),OrderByDescending()后继续写ThenBy()、ThenByDescending(),

## 限制结果集，获取部分数据
### Skip(n)  --跳过n条数据
### Take（n） -- 获取n条

## 聚合函数
Max(),Min(),Average(),Sum(),Count()

## 分组
### GroupBy() 方法参数是分组条件表达式

## 投影
> 把集合中的每一项转换为另外一种类型

### Select() 

# LINQ 查询语法
> 使用Where,OrderBy、Select等扩展方法进行数据查询的写法叫“LINQ方法语法”。还有一种查询语法的写法

# LINQ解决面试问题
> 面试尽量避免使用正则表达式，LINQ等这些高级的类库

## 1、找出最大值
```
            //最大值
            int i = 5;
            int j = 8;
            int k = 6;

            //LINQ
            int[] nums = new int[] { i, j, k };
            var max = nums.Max();
            Console.WriteLine(max);

            //MAX
            int max2 = Math.Max(i, (Math.Max(k, j)));
            Console.WriteLine(max2);

            //三元
            int max3 = i > j ? (i > k ? i : k) : (j > k ? j : k);
            Console.WriteLine(max3);
```

## 2、有一个用逗号分隔的表示成绩的字符串，如“61，90，100，99，18，22，38，66，80，93” 计算这些成绩的平均分
```
            /*有一个用逗号分隔的表示成绩的字符串，如“61,90,100,99,18,22,38,66,80,93” 计算这些成绩的平均分
             */
            Console.WriteLine("请输入只有数字的字符串！！！");
            var str = Console.ReadLine();
            var str1 = str.Trim().Split(',');
            var intnum = str1.Select(s => Convert.ToInt32(s));
            var a = intnum.Average();
            double avg = str.Trim().Split(',').Select(s => Convert.ToInt32(s)).Average();
            Console.WriteLine(avg);
```

## 3、统计一个字符串中每个字母出现的频率（忽略大小写），然后按从高到低的顺序输出出现频率高于两次的单词和其出现的频率
```
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
```