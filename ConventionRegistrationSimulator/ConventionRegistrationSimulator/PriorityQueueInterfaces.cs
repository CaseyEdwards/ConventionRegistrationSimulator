//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		PriorityQueueInterfaces.cs
//	Description:		Contains interface definitions for use in the PriorityQueue class.
//	Course     :		CSCI 2210-201 - Data Structures
//	Author     :		Casey Edwards, zcee10@etsu.edu
//	Created    :		Thursday, November 3rd, 2016
//	Copyright  :		Casey Edwards 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace ConventionRegistrationSimulator
{
    #region IContainer
    /// <summary>
    /// Interface describing a container class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainer<T>
    {
        /// <summary>
        /// Remove all objects.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the container is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the container is empty; otherwise, <c>false</c>.
        /// </returns>
        bool IsEmpty();

        /// <summary>
        /// Gets or sets the number of entries.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; set; }
    }
    #endregion

    #region IPriorityQueue
    /// <summary>
    /// Interface description for a PriorityQueue class.
    /// Implements IContainer, where the container must hold
    /// objects that themselves implement IComparable.
    /// </summary>
    /// <typeparam name="T">The type of object to hold.</typeparam>
    /// <seealso cref="ConventionRegistrationSimulator.IContainer{T}" />
    public interface IPriorityQueue<T> : IContainer<T>
                            where T : IComparable
    {
        /// <summary>
        /// Enqueues the specified item based on priority.
        /// </summary>
        /// <param name="item">The item to hold.</param>
        void Enqueue(T item);

        /// <summary>
        /// Dequeues an item from the container.
        /// </summary>
        void Dequeue();

        /// <summary>
        /// Return a reference to the top item in the queue.
        /// </summary>
        /// <returns>A reference to the front of the queue.</returns>
        T Peek();
    }
    #endregion
}
