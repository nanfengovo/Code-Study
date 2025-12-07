namespace Linq原理
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //找出大于10的
            int[] nums = new int[] { 3, 5, 3453, 33, 2, 9, 35 };
            #region 1、不使用linq
            //解法一：
            int j = 0;
            foreach (var item in nums)
            {
                if (item >= 10)
                {
                    j++;
                }
            }
            int[] newNum = new int[j];
            int i = 0;
            foreach (var item in nums)
            {
                if(item >= 10)
                {
                    newNum[i] = item;
                    i++;
                }
            }
            foreach (var item in newNum)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("----------------------------------------------------------");
            //解法2
            List<int> intList = new List<int>();
            foreach (var item in nums)
            {
                if (item >= 10)
                {
                    intList.Add(item);
                }
            }
            foreach (var item in intList)
            {
                Console.WriteLine(item);
            }
            #endregion
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("使用linq");
            #region 使用linq
            var f = nums.Where(x => x >= 10);
            foreach (var item in f)
            {
                Console.WriteLine(item);
            }
            #endregion
        }
    }
}
