using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CSWorkshop
{

    // ==================================================================================
    // Associative arrary
    // ==================================================================================

    class AssociativeArray
    {
        // ===============================================
        // Private
        // ===============================================

        // TODO: Use int **
        private int[] Indecies;
        private Record[] Records;
        private int Size;

        private class Record {
            public int Key;
            public int Val;
            public int Next;
            public int Prev;
        }
        private int Hash(int key)
        {
            return key % Size;
        }
        private int FreeIdx;

        // ===============================================
        // Public
        // ===============================================

        public void Print()
        {
            Console.WriteLine();
            for(var i=0; i<Size; ++i) {
                Console.WriteLine(String.Format("[{1,3}]  [{0,3}: {2,3}{6}{3,3} {4,3} {5,3}] {7}", 
                    i,
                    Indecies[i],
                    Records[i].Key,
                    Records[i].Val,
                    Records[i].Prev,
                    Records[i].Next,
                    Records[i].Key>=0? " ->": "   ",
                    FreeIdx == i? '*': ' '
                ));
            }
            Console.WriteLine();
        }
        public AssociativeArray(int size)
        {
            Debug.Assert(size>0);
            Size = size;

            Indecies = new int[size];
            Records = new Record[size];
            for(var i=0; i<Size; ++i){
                Indecies[i] = -1;
                Records[i] = new Record(){
                    Key = -1,
                    Val = 0,
                    Prev = i-1,
                    Next = i+1,
                };
            }
            Records[Size-1].Next = -1;
            FreeIdx = 0;
        }
        public void Set(int key, int val)
        {
            var hash = Hash(key);
            var idx = Indecies[hash];
            
            if (idx>=0){
                if(Records[idx].Key == key){
                    // Update first or single key
                    Console.Write("!");
                    Records[idx].Val = val;
                    return;
                }
                while(Records[idx].Next!=-1){
                    idx=Records[idx].Next;
                    if(Records[idx].Key==key){
                        // Update collided key
                        Console.Write("#");
                        Records[idx].Val = val;
                        return;
                    }
                }
                // Add collided key
                Console.Write("+");
                Debug.Assert(FreeIdx>=0);

                var nextFreeIdx = Records[FreeIdx].Next;
                Records[idx].Next = FreeIdx;
                Records[FreeIdx].Key = key;
                Records[FreeIdx].Val = val;
                Records[FreeIdx].Next = -1;
                Records[FreeIdx].Prev = idx;

                FreeIdx = nextFreeIdx;
                if(FreeIdx>=0){
                    Records[FreeIdx].Prev = -1;
                }
                return;
            }
            // Add new key
            Console.Write(".");
            Debug.Assert(FreeIdx>=0);
            idx = FreeIdx;
            FreeIdx = Records[FreeIdx].Next;
            if(FreeIdx>=0){
                Records[FreeIdx].Prev = -1;
            }

            Indecies[hash] = idx;
            Records[idx].Key = key;
            Records[idx].Val = val;
            Records[idx].Prev = -1;
            Records[idx].Next = -1;
        }
        public int? Get(int key)
        {
            var idx = Indecies[Hash(key)];
            if(idx<0){
                return null;
            }
            if(Records[idx].Key == key){
                return Records[idx].Val;
            }
            while(Records[idx].Next!=-1){
                idx=Records[idx].Next;
                if(Records[idx].Key==key){
                    return Records[idx].Val;
                }
            }
            return null;
        }
    }


    // ==================================================================================
    // Program
    // ==================================================================================

    class Test
    {
        static public void Run()
        {
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
                Console.WriteLine();
            }
            // arr.Print();

            for(var i=0; i<N; ++i)
            {
                var key = i+i*i;
                var val = i;
                Debug.Assert(arr.Get(key)==val);
            }

            Debug.Assert(arr.Get(11111)==null);
            

            Console.WriteLine("\nOK");
            Console.WriteLine("\n!!!");
        }
    }
}

// ======================================================================================
// EOF
// ======================================================================================
