using System.Threading.Tasks;

namespace WhenALL
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var t1 = File.ReadAllTextAsync(@"E:\Test\1.txt");
            //var t3 = File.ReadAllTextAsync(@"E:\Test\12.2.txt");
            //var t2 = File.ReadAllTextAsync(@"E:\Test\2.txt");

            //var strs = await Task.WhenAll(t1, t2, t3);
            //Console.WriteLine(strs[0]);
            //Console.WriteLine(strs[1]);
            //Console.WriteLine(strs[2]);

            var files = Directory.GetFiles(@"E:\Test");
            var countTasks = new Task<int> [files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                string filename = files[i];
                var t = ReadCharsCount(filename);
                countTasks[i] = t;
            }
            var counts = await Task.WhenAll(countTasks);
            int c = counts.Sum();
            Console.WriteLine(c);
        }

        static async Task<int> ReadCharsCount(string filename)
        {
            string s = await File.ReadAllTextAsync(filename);
            return s.Length;
        }
    }
}
