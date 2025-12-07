namespace LINQ_解决面试题
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*有一个用逗号分隔的表示成绩的字符串，如“61,90,100,99,18,22,38,66,80,93” 计算这些成绩的平均分
             */
            Console.WriteLine("请输入只有数字的字符串！！！");
            var str = Console.ReadLine();
            var str1 = str.Trim().Split(',');
            var intnum = str1.Select(s => Convert.ToInt32(s));
            var a = intnum.Average();
            double avg = str.Trim().Split(',').Select(s => Convert.ToInt32(s)).Average();
            Console.WriteLine(avg);
        }
    }
}
