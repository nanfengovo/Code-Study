using Microsoft.Extensions.DependencyInjection;

namespace DI1
{
    public class Program
    {

        private static readonly IService _service;



        static void Main(string[] args)
        {
            //IService service = new ServiceA();
            //service.Serve();

            //_service.Serve();

            ServiceCollection service = new ServiceCollection();
            service.AddTransient<ServiceA>();
            using (ServiceProvider sp = service.BuildServiceProvider())
            {
                var t = sp.GetService<ServiceA>();
                t.Serve();
            }
                
            
            

        }
    }

    public interface IService
    {
        void Serve();
    }

    public class ServiceA : IService
    {
        public void Serve()
        {
            Console.WriteLine("Service A Called");
        }
    }

    public class ServiceB : IService
    {
        public void Serve()
        {
            Console.WriteLine("Service B Called");
        }
    }
}
