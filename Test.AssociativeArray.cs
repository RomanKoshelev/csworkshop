using System;
using System.Diagnostics;

// ======================================================================================
// Test Associative arrary
// ======================================================================================

namespace CSWorkshop
{
    class Test
    {
        static public void AddPseudoRandom1000()
        {
            Console.WriteLine("Add Random 1000 ...");
            const int N = 1000;
            var arr = new AssociativeArray(N);

            for(var s=0; s<3; ++s){
                for(var i=0; i<N; ++i)
                {
                    var key = i+i*i;
                    var val = i;
                    arr.Set(key, val);
                    Debug.Assert(arr.Get(key)==val);
                }
            }

            for(var i=0; i<N; ++i)
            {
                var key = i+i*i;
                var val = i;
                Debug.Assert(arr.Get(key)==val);
            }

            Debug.Assert(arr.Get(11111)==null);
            
            Console.WriteLine("OK.");
        }                

        static public void AddRandom10kCatchingException()
        {
            Console.WriteLine("Add Random 10,000 waiting for exception...");
            const int N = 1000;
            var arr = new AssociativeArray(N);
            Random rnd= new Random();

            var ok = false;
            for(var i=0; i<10*N; ++i)
            {
                var key = rnd.Next();
                var val = i;
                try{
                    arr.Set(key, val);
                    Debug.Assert(arr.Get(key)==val);
                }
                catch (AssociativeArrayException) {
                    ok = true;
                }
            }
            Console.WriteLine(ok? "OK.": "FAIL");
        }                

        static public void PerformanceNoCollisions()
        {
            Console.WriteLine("Performance with no collisions ...");
            const int K = 10;
            const int N = 100000;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var arr = new AssociativeArray(N);
            for (var k=0; k<K; ++k){
                for(var i=0; i<N; ++i)
                {
                    var key = i;
                    var val = i;
                    arr.Set(key, val);
                }                
            }
            stopWatch.Stop();
            Console.WriteLine("Milliseconds: {0,2:N0}", stopWatch.Elapsed.TotalMilliseconds/K);
        }
        static public void PerformanceAllCollisions()
        {
            Console.WriteLine("Performance with all collisions ...");
            const int K = 10;
            const int N = 100000;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var arr = new AssociativeArray(N);
            for (var k=0; k<K; ++k){
                for(var i=0; i<N; ++i)
                {
                    var key = i%2;
                    var val = i;
                    arr.Set(key, val);
                }                
            }
            stopWatch.Stop();
            Console.WriteLine("Milliseconds: {0,2:N0}", stopWatch.Elapsed.TotalMilliseconds/K);
        }
        static public void RunAll()
        {
            Console.WriteLine("Running tests:");
            AddPseudoRandom1000();
            AddRandom10kCatchingException();
            PerformanceNoCollisions();
            PerformanceAllCollisions();
            Console.WriteLine("All tests passed.");
        }
    }
}

// ======================================================================================
// EOF
// ======================================================================================
