namespace StackOverFlowDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BadGuy badGuy = new BadGuy();
            badGuy.BadMethod();
        }
    }

    class BadGuy
    {
        public void BadMethod()
        {
            //var i = 100;
            BadMethod();
        }
    }
}
