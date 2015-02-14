using System;
using System.Collections.Generic;
using MantiCore.Bundle;
using MantiCore.Dependant;

namespace Manti
{
    class Program
    {
        [BundleInformation("test1"), DependsOn]
        class TestBundle {}

        [BundleInformation("test2"), DependsOn]
        class Test1Bundle { }

        [BundleInformation("test3"), DependsOn("test2", "test1")]
        class Test2Bundle { }

        [BundleInformation("test4"), DependsOn("test3")]
        class Test3Bundle { }

        [BundleInformation("test5"), DependsOn("test1", "test3")]
        class Test4Bundle { }

        [BundleInformation("test6"), DependsOn("test5")]
        class Test5Bundle { }
        [BundleInformation("test7"), DependsOn("test5")]
        class Test6Bundle { }
        [BundleInformation("test8"), DependsOn("test9")]
        class Test7Bundle { }
        [BundleInformation("test9"), DependsOn("test10")]
        class Test8Bundle { }
        [BundleInformation("test10"), DependsOn("test8")]
        class Test9Bundle { }

        static void Main(string[] args)
        {
            var listD = new List<DependsOn>();
            listD.Add(DependsOn.GetDependancies(typeof(TestBundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test1Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test2Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test3Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test4Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test5Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test6Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test7Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test8Bundle)));
            listD.Add(DependsOn.GetDependancies(typeof(Test9Bundle)));

            var dependancyGraph = new DependancyGraph(listD);

            List<string> startup;
            Console.WriteLine("Starting Up");
            /*
             * test1, test2
             * test3
             * test4, test5
             * test6, test7
             */ 
            while ((startup = dependancyGraph.GetNextStartupGroup()).Count > 0)
            {
                Console.WriteLine(string.Join(", ", startup));
            }
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
