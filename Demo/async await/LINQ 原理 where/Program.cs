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
