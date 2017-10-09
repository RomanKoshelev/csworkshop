using System;
using System.Diagnostics;

// ======================================================================================
// Test Associative arrary
// ======================================================================================

namespace CSWorkshop
{
    class Test
    {
        static public void AddRandom1000()
        {
            Console.WriteLine("AddRandom1000()...");
            const int N = 1000;
            var arr = new AssociativeArray(N);

            // arr.Print();
            for(var s=0; s<3; ++s){
                for(var i=0; i<N; ++i)
                {
                    var key = i+i*i;
                    var val = i;
                    arr.Set(key, val);
                    Debug.Assert(arr.Get(key)==val);
                }
                // Console.WriteLine();
            }
            // arr.Print();

            for(var i=0; i<N; ++i)
            {
                var key = i+i*i;
                var val = i;
                Debug.Assert(arr.Get(key)==val);
            }

            Debug.Assert(arr.Get(11111)==null);
            
            Console.WriteLine("OK.");
        }                

        static public void RunAll()
        {
            Console.WriteLine("Running tests:");
            AddRandom1000();
            Console.WriteLine("All tests passed.");
        }
    }
}

// ======================================================================================
// EOF
// ======================================================================================
