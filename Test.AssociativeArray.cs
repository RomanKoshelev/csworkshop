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
            Console.WriteLine("Add Random 1k ...");
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
            Console.WriteLine("Add Random 10k waiting for exception...");
            const int N = 1000;
            var arr = new AssociativeArray(N);
            Random rnd = new Random();

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
                    Debug.Assert(i>=N, "Must be enough room");
                    ok = true;
                }
            }
            Debug.Assert(ok, "Test faild");
            Console.WriteLine("OK.");
        }                

        static public void PerformanceNoCollisions()
        {
            Console.WriteLine("Performance with no collisions on 100k...");
            const int K = 100;
            const int N = 100000;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var k=0; k<K; ++k){
                var arr = new AssociativeArray(N);
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
            Console.WriteLine("Performance with all collisions on 100k...");
            const int K = 100;
            const int N = 100000;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var k=0; k<K; ++k){
                var arr = new AssociativeArray(N);
                for(var i=0; i<N; ++i)
                {
                    var key = 42;
                    var val = i;
                    arr.Set(key, val);
                }                
            }
            stopWatch.Stop();
            Console.WriteLine("Milliseconds: {0,2:N0}", stopWatch.Elapsed.TotalMilliseconds/K);
        }
        static public void RemoveElements()
        {
            Console.WriteLine("Add and remove 10k items...");
            const int N = 10000;
            var arr = new AssociativeArray(N);
            for(var i=0; i<N; ++i)
            {
                var key = (i+i*i);
                var val = i;
                arr.Set(key, val);
                Debug.Assert(arr.Get(key)==val);
            }                
            for(var i=0; i<N; ++i)
            {
                var key = (i+i*i);
                arr.Remove(key);
                Debug.Assert(arr.Get(key)==null);
            }
            Console.WriteLine("OK.");
        }
        static public void RemoveAddElements()
        {
            Console.WriteLine("Remove and add 4 items...");
            const int N = 4;
            int[] keys = {1, 2, 8, 12};

            var arr = new AssociativeArray(N);
            arr.Print();
            // Add
            for(var i=0; i<N; ++i)
            {
                var key = keys[i];
                var val = i;
                arr.Set(key, val);
                Debug.Assert(arr.Get(key)==val);
            }
            arr.Print();
            // Remove
            for(var i=0; i<N; ++i)
            {
                var key = keys[i];
                arr.Remove(key);
                Debug.Assert(arr.Get(key)==null);
            }
            arr.Print();
            // Add
            for(var i=0; i<N; ++i)
            {
                var key = keys[i];
                var val = i;
                arr.Set(key, val);
                Debug.Assert(arr.Get(key)==val);
            }
            arr.Print();
            Console.WriteLine("OK.");
        }
        static public void RandomRemoveAddElements()
        {
            Console.WriteLine("Random remove and add 100k items...");
            const int N = 10;
            var keys = new int [N];
            Random rnd = new Random(123);
            for(var i=0; i<N; ++i) {
                keys[i]=rnd.Next(0,N/2) + i*N;
            }
            var arr = new AssociativeArray(N, 17);
            arr.Print();
            // Add
            for(var i=0; i<N; ++i) {
                arr.Set(keys[i], keys[i] + 42);
            }
            arr.Print();
            for(var i=0; i<N; ++i) {
                Debug.Assert(arr.Get(keys[i]) == keys[i] + 42);
            }
            // Remove
            for(var i=0; i<N; ++i) {
                arr.Remove(keys[i]);
            }
            arr.Print();
            for(var i=0; i<N; ++i) {
                Debug.Assert(arr.Get(keys[i])==null);
            }
            // Add
            for(var i=0; i<N; ++i) {
                arr.Set(keys[i], keys[i] + i);
            }
            arr.Print();
            for(var i=0; i<N; ++i) {
                Debug.Assert(arr.Get(keys[i])==keys[i] + i);
            }
            Console.WriteLine("OK.");
        }
        static public void RunAll()
        {
            Console.WriteLine("Running tests:");
            AddPseudoRandom1000();
            AddRandom10kCatchingException();
            PerformanceNoCollisions();
            PerformanceAllCollisions();
            RemoveElements();
            RemoveAddElements();
            RandomRemoveAddElements();
            Console.WriteLine("All tests passed.");
        }
    }
}

// ======================================================================================
// EOF
// ======================================================================================
