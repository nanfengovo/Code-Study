namespace Linq常用扩展方法
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}
