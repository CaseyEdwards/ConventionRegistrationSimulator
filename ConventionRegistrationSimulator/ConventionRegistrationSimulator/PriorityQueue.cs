//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		PriorityQueue.cs
//	Description:		PriorityQueue object implementation.
//	Course     :		CSCI 2210-201 - Data Structures
//	Author     :		Casey Edwards, zcee10@etsu.edu
//	Created    :		Thursday, November 3rd, 2016
//	Copyright  :		Casey Edwards 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace ConventionRegistrationSimulator
{
    /// <summary>
    /// Priority Queue class.
    /// Places items in the queue based on comparative priority.
    /// Same priority nodes are treated FIFO.
    /// </summary>
    /// <typeparam name="T">Type of the objects to hold.</typeparam>
    /// <seealso cref="ConventionRegistrationSimulator.IPriorityQueue{T}" />
    class PriorityQueue<T> : IPriorityQueue<T>
        where T : IComparable
    {
        #region Properties
        private Node top; // Link to the top node.
        public int Count { get; set; } // Count of nodes in the queue.
        #endregion

        #region Interface Method Implementations
        /// <summary>
        /// Enqueues the specified item according to priority.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            if (Count == 0) // Add the node directly if the queue is empty.
                top = new Node(item, null);
            else
            {
                Node current = top;
                Node previous = null;

                // Step through the nodes until a lower priority node is found.
                while (current != null && current.Item.CompareTo(item) >= 0)
                {
                    previous = current;
                    current = current.Next;
                }

                // Found a place to insert the node.
                Node newNode = new Node(item, current);

                // If there is a previous node, set it to link to this new one.
                if (previous != null)
                    previous.Next = newNode;
                else
                    top = newNode;
            }
            Count++; // Increment the count.
        }

        /// <summary>
        /// Removes the front node from the queue.
        /// </summary>
        public void Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cannot dequeue from empty queue.");
            else
            {
                Node oldNode = top;
                top = top.Next; // Set the top reference to the next item.
                Count--;        // Decrement the count.
                oldNode = null; // Allow the old top node to be garbage collected.
            }
        }

        /// <summary>
        /// Clears the queue of nodes.
        /// </summary>
        public void Clear()
        {
            // Set top to null and Count to zero.
            top = null;
            Count = 0;
        }

        /// <summary>
        /// Returns a reference to the value stored in the top of the queue.
        /// </summary>
        /// <returns>The object in the top of the queue.</returns>
        /// <exception cref="InvalidOperationException">Cannot retrieve value from an empty queue.</exception>
        public T Peek()
        {
            if (!IsEmpty())
                return top.Item; // Return the value, not the node itself.
            else
                throw new InvalidOperationException("Cannot retrieve value from an empty queue.");
        }

        /// <summary>
        /// Determines whether this queue is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this queue is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        {
            return Count == 0;
        }
        #endregion

        #region Private Node Class        
        /// <summary>
        /// Node class used for enforcing the PriorityQueue.
        /// Holds data and a reference to the next node.
        /// </summary>
        /// <seealso cref="ConventionRegistrationSimulator.IPriorityQueue{T}" />
        private class Node
        {
            public T Item { get; set; } // Data in this node
            public Node Next { get; set; } // Reference to the next node

            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="value">The value to store.</param>
            /// <param name="link">Link to the next node.</param>
            public Node(T value, Node link)
            {
                Item = value;
                Next = link;
            }
        }
        #endregion
    }
}
