using System;
using System.Diagnostics;
using System.Collections.Generic;

// ======================================================================================
// Associative arrary
// ======================================================================================

namespace CSWorkshop
{
    class AssociativeArrayException : Exception
    {
        public AssociativeArrayException(string message) : base(message)
        {
        }
    }
    class AssociativeArray
    {
        // ===============================================
        // Private
        // ===============================================

        private int[] Indecies;
        private Record[] Records;
        private int Size;
        private int HashShift;

        private class Record {
            public int Key;
            public int Val;
            public int Next;
            public int Prev;
        }
        private int Hash(int key)
        {
            return (key+HashShift) % Size;
        }
        private int FreeIdx;

        // ===============================================
        // Public
        // ===============================================

        public AssociativeArray(int size, int hashShift=0)
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
            HashShift = hashShift;
        }
        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("Hash->RecId  RecId Key   Val Prv Next FreeHead");
            for(var i=0; i<Size; ++i) {
                Console.WriteLine(String.Format("[{0,3} ->{1,3}]  [{2,3}: {3,3}{4}{5,3} {6,3} {7,3}] {8}", 
                    Hash(i),
                    Indecies[i],
                    i,
                    Records[i].Key,
                    Records[i].Key>=0? " ->": "   ",
                    Records[i].Val,
                    Records[i].Prev,
                    Records[i].Next,
                    FreeIdx == i? '*': ' '
                ));
            }
            Console.WriteLine();
        }
        public void Set(int key, int val)
        {
            var hash = Hash(key);
            var idx = Indecies[hash];
            
            if (idx>=0){
                if(Records[idx].Key == key){
                    // Update first or single key
                    Records[idx].Val = val;
                    return;
                }
                while(Records[idx].Next!=-1){
                    idx=Records[idx].Next;
                    if(Records[idx].Key==key){
                        // Update collided key
                        Records[idx].Val = val;
                        return;
                    }
                }
                // Add collided key
                if(FreeIdx<0){
                    throw new AssociativeArrayException("Not enough room");
                }

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
            if(FreeIdx<0){
                throw new AssociativeArrayException("Not enough room");
            }
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
        public void Remove(int key)
        {
            var hash = Hash(key);
            var idx = Indecies[hash];
            Debug.Assert(idx>=0, string.Format("Unknown key {0}", key));
            while(idx!=-1)
            {
                if(Records[idx].Key==key)
                {
                    // Clear data
                    Records[idx].Key =  -1;
                    Records[idx].Val =   0;

                    // Remove from data list
                    var prev = Records[idx].Prev;
                    var next = Records[idx].Next;
                    if(prev!=-1){
                        Records[prev].Next = next;
                    }
                    if(next!=-1){
                        Records[next].Prev = prev;
                    }

                    // Remove from index
                    if (prev == -1) {
                        Debug.Assert(Indecies[hash] == idx);
                        Indecies[hash] = next;
                    }

                    // Insert to free list
                    if(FreeIdx==-1)
                    {
                        Records[idx].Prev = -1;
                        Records[idx].Next = -1;
                    }
                    else{
                        Records[idx].Next = FreeIdx;
                        Records[idx].Prev = -1;
                        Records[FreeIdx].Prev = idx;
                    }
                    FreeIdx = idx;
                    idx=next;
                } 
                else{
                    idx=Records[idx].Next;
                }
            }
        }
    }
}

// ======================================================================================
// EOF
// ======================================================================================
