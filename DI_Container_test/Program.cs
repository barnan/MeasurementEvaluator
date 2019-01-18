using Autofac;
using System;

namespace DI_Container_test
{
    public interface IOutput
    {
        void Write(string content);
    }

    public class ConsoleOutput1 : IOutput, IDisposable
    {
        public string Name { get; }

        public ConsoleOutput1()
        {
            Name = nameof(ConsoleOutput1);
        }

        public void Write(string content)
        {
            Console.WriteLine($"{Name}: {content}");
        }

        public void Dispose()
        {
            Console.WriteLine($"{Name} Disposed.");
        }
    }

    public class ConsoleOutput2 : IOutput, IDisposable
    {
        public string Name { get; }

        public ConsoleOutput2()
        {
            Name = nameof(ConsoleOutput2);
        }

        public void Write(string content)
        {
            Console.WriteLine($"{Name}: {content}");
        }

        public void Dispose()
        {
            Console.WriteLine($"{Name} Disposed.");
        }
    }

    public interface IDateWriter
    {
        void WriteDate();
    }

    public class TodayWriter1 : IDateWriter, IDisposable
    {
        public string Name { get; }
        private IOutput _output;

        public TodayWriter1(IOutput output)
        {
            _output = output;
            Name = nameof(TodayWriter1);
        }

        public void WriteDate()
        {
            _output.Write(DateTime.Today.ToShortDateString());
        }

        public void Dispose()
        {
            Console.WriteLine($"{Name} Disposed.");
        }
    }


    public class TodayWriter2 : IDateWriter, IDisposable
    {
        public string Name { get; }
        public IOutput Output { get; set; }

        public TodayWriter2()
        {
            Name = nameof(TodayWriter2);
        }

        public void WriteDate()
        {
            Output.Write(DateTime.Today.ToShortDateString());
        }


        public void WriteParam(int param1, int param2 = 5, params int[] param3)
        {
            ;
        }


        public void Dispose()
        {
            Console.WriteLine($"{Name} Disposed.");
        }
    }



    public class Program
    {
        private static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();

            // tipus szerint regisztrálva, a konténer hozza létre az objektumot: 
            // (csak konkrét típust lehet regisztrűlni, interface-t, abstract class-t nem)
            //builder.RegisterType<ConsoleOutput1>().As<IOutput>();

            // a konrkét objektumot közvetlenül regisztáljuk be:
            //ConsoleOutput2 co2 = new ConsoleOutput2();
            //builder.RegisterInstance(co2).As<IOutput>();

            // egy kifejezéssel példányosítva regisztráljuk be:
            //builder.Register(c => new ConsoleOutput1()).As<IOutput>();

            builder.RegisterType<ConsoleOutput1>().As<IOutput>().UsingConstructor();

            // így a interface-ként és ConsolOutputként is tudjuk resolve-olni:
            builder.RegisterType<ConsoleOutput1>().AsSelf().As<IOutput>();

            // azzal a konstruktorral hívja, amibe a legtöbb paramétert be tudja injektálni
            //builder.RegisterType<TodayWriter1>().As<IDateWriter>();

            //opcionális property injection, resolvolni service-t (interface-t) lehet:
            builder.Register(c => new TodayWriter2() { Output = c.ResolveOptional<IOutput>() }).As<IDateWriter>();


            Container = builder.Build();

            WriteDate();

            Console.ReadKey();
        }


        public static void WriteDate()
        {
            IDateWriter writer;

            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                writer = scope.Resolve<IDateWriter>();
                writer.WriteDate();

                //((TodayWriter2)writer).WriteParam(1, param3: new [] { 2, 3, 4}); -> nevestített és default paraméterek.
            }


        }


    }


}
