/*MIT License

Copyright(c) 2018 Vili Volčini / viliwonka

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
The object used for querying. This object should be persistent - re-used for querying.
Contains internal array for pooling, so that it doesn't generate (too much) garbage.
The array never down-sizes, only up-sizes, so the more you use this object, less garbage will it make over time.

Should be used only by 1 thread,
which means each thread should have it's own KDQuery object in order for querying to be thread safe.

KDQuery can query different KDTrees.
*/


using System.Collections.Generic;
using UnityEngine;
using System;

namespace DataStructures.ViliWonka.KDTree {

    public partial class KDQuery {

        protected KDQueryNode[] queueArray;  // queue array
        protected Heap.MinHeap<KDQueryNode>  minHeap; //heap for k-nearest
        protected int count = 0;             // size of queue
        protected int queryIndex = 0;        // current index at stack
        private readonly KDQueryNode[] pool;
        private int lastPoolIndex;

        /// <summary>
        /// Returns initialized node from stack that also acts as a pool
        /// The returned reference to node stays in stack
        /// </summary>
        /// <returns>Reference to pooled node</returns>
        private KDQueryNode PushGetQueue() {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.PushGetQueue");
#endregion
            KDQueryNode node = null;

            if (count < queueArray.Length) {

                if (queueArray[count] == null)
                    queueArray[count] = node = pool[++lastPoolIndex % pool.Length];
                else
                    node = queueArray[count];
            }
            else {

                // automatic resize of pool
                Array.Resize(ref queueArray, queueArray.Length * 2);
                node = queueArray[count] = pool[++lastPoolIndex % pool.Length];
            }

            count++;
			
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion

            return node;
        }

        protected void PushToQueue(KDNode node, Vector3 tempClosestPoint) {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.PushToQueue");
#endregion
            var queryNode = PushGetQueue();
            queryNode.node = node;
            queryNode.tempClosestPoint = tempClosestPoint;
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
        }

        protected void PushToHeap(KDNode node, Vector3 tempClosestPoint, Vector3 queryPosition) {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.PushToHeap");
#endregion
            var queryNode = PushGetQueue();
            queryNode.node = node;
            queryNode.tempClosestPoint = tempClosestPoint;

            float sqrDist = Vector3.SqrMagnitude(tempClosestPoint - queryPosition);
            queryNode.distance = sqrDist;
            minHeap.PushObj(queryNode, sqrDist);
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
        }

        protected int LeftToProcess {

            get {
                return count - queryIndex;
            }
        }

        // just gets unprocessed node from stack
        // increases queryIndex
        protected KDQueryNode PopFromQueue() {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.PopFromQueue");
#endregion
            var node = queueArray[queryIndex];
            queryIndex++;
			
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion

            return node;
        }

        protected KDQueryNode PopFromHeap() {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.PopFromHeap");
#endregion
            KDQueryNode heapNode = minHeap.PopObj();

            queueArray[queryIndex]= heapNode;
            queryIndex++;
			
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion

            return heapNode;
        }

        protected void Reset() {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.Reset");
#endregion
            count = 0;
            queryIndex = 0;
            minHeap.Clear();
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
        }

        public KDQuery(int queryNodesContainersInitialSize = 2048) {
#region Profiling Begin
            UnityEngine.Profiling.Profiler.BeginSample("KDQuery.Base.Construct");
#endregion
            queueArray = new KDQueryNode[queryNodesContainersInitialSize];
            minHeap = new Heap.MinHeap<KDQueryNode>(queryNodesContainersInitialSize);
            
            pool = new KDQueryNode[queryNodesContainersInitialSize];
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = new KDQueryNode()
                {
                    poolIndex = i
                };
            }
            lastPoolIndex = 0;
#region Profiling End
			UnityEngine.Profiling.Profiler.EndSample();
#endregion
        }

    }

}