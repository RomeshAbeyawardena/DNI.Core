using DNI.Shared.Services;
using DNI.Shared.Services.Abstraction;
using DNI.Shared.Services.Extensions;
using DNI.Shared.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;
using DNI.Shared.Shared.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DNI.Shared.App
{
    public static class Program
    {
        public static async Task Main()
        {
            //var serviceCollection = new ServiceCollection();

            ////serviceCollection.RegisterServiceBroker<ServiceBroker>();

            //var booleanSwitch = Switch.Create<string, bool>()
            //    .CaseWhen("YES", true, "yes", "1")
            //    .CaseWhen("NO", false, "no", "0");

            //var case1 = booleanSwitch.Case("YES");
            //var case2 = booleanSwitch.Case("NO");
            //var case3 = booleanSwitch.Case("1");
            //var case4 = booleanSwitch.Case("0");
            //var case5 = booleanSwitch.Case("A");

            //var mClassArray = new [] { new MClass { K1 = 12456, V1 = "htfhd" } };

            //var test = booleanSwitch["YES"];

            //var myDict = DictionaryBuilder.Create<int, string>()
            //    .AddRange(mClassArray, mClass => mClass.K1, mClass => mClass.V1)
            //    .ToDictionary();

            //var myList = ListBuilder.Create<int>()
            //    .AddRange(mClassArray, mClass => mClass.K1);

            //FluentTry.Create()
            //    .Try(() => throw new FieldAccessException())
            //    .Catch<FieldAccessException>(OnCatch)
            //    .Invoke();

            // await FluentTry.CreateAsync()
            //        .Try(async() => {  await Task.Delay(2000); throw new TimeoutException(); })
            //        .Try(async() => await Task.Delay(2000))
            //        .Try(async() => await Task.Delay(2000))
            //        .Catch<TimeoutException>(OnCatch)
            //        .InvokeAsync();

            //var results = await FluentTry.CreateAsync<int, int>()
            //    .Try(async (a) => { var result = a + 5; 
            //        if(result < 10) 
            //            throw new ArithmeticException(); 
            //        return await Task.FromResult(result); })
            //    .Try(async (a) => await Task.FromResult(a + 10))
            //    .Try(async (a) => await Task.FromResult(a + 15))
            //    .Catch<ArithmeticException>(OnCatch, true)
            //    .InvokeAsync(5);

            //DisposableHelper
            //    .Use<MyDisposable>(myDisposable => { myDisposable.CallMe(); }, "I" );

            //await DisposableHelper
            //    .UseAsync<MyDisposable>(async(myDisposable) => await myDisposable.CallMeAsync(), "IAsync");

            //var number = await DisposableHelper
            //    .UseAsync<int, MyDisposable>(async(myDisposable) => await myDisposable.GetNumberAsync(), "IAsync");
            //Console.WriteLine("Number returned: {0}", number);
            //var number1 = DisposableHelper
            //    .Use<int, MyDisposable>(myDisposable => myDisposable.GetNumber(), null, "I");
            //Console.WriteLine("Number returned: {0}", number1);

            var hashingProvider = new HashingProvider();

            var hash1 = hashingProvider.HashBytes("SHA512", "Hello World".GetBytes(Encoding.ASCII));
            var hash2 = hashingProvider.HashBytes("SHA512", "Hello World".GetBytes(Encoding.ASCII));

            if (!hash1.SequenceEqual(hash2))
                throw new InvalidOperationException();

            var derivedBytes = hashingProvider.PasswordDerivedBytes("My Password", "MySecureSalt1234567890101".GetBytes(Encoding.ASCII), KeyDerivationPrf.HMACSHA512, 100000, 32);

            var derivedBytes2 = hashingProvider.PasswordDerivedBytes("My Password", "MySecureSalt1234567890101".GetBytes(Encoding.ASCII), KeyDerivationPrf.HMACSHA512, 100000, 32);

            Console.WriteLine("{0}", BitConverter.ToString(derivedBytes.ToArray()));
            Console.WriteLine("{0}", BitConverter.ToString(derivedBytes2.ToArray()));
        }

        public static void OnCatch(Exception ex)
        {
            Console.WriteLine(ex);
        }

        class MClass
        {
            public int K1 { get; set; }
            public string V1 { get; set; }
        }

        public class ServiceBroker : ServiceBrokerBase
        {
            public ServiceBroker()
            {
                Assemblies = new [] { DefaultAssembly, GetAssembly<ServiceBroker>() };
            }
        }

        public class MyDisposable : IDisposable
        {
            public string Name { get; }

            public void Dispose()
            {
                Dispose(true);
            }

            public int GetNumber()
            {
                return 5;
            }

            public async Task<int> GetNumberAsync()
            {
                return await Task.FromResult(GetNumber());
            }

            public void CallMe()
            {
                Console.WriteLine($"{ Name } CallMe was invoked.");
            }

            public async Task CallMeAsync()
            {
                await Task.Delay(1000);
                Console.WriteLine($"{ Name } CallMe was invoked async.");
            }

            protected virtual void Dispose(bool gc)
            {
                if(gc)
                    Console.WriteLine($"Then { Name } was disposed");
            }

            public MyDisposable(string name)
            {
                Name = name;
            }
        }
    }
}
