//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		RegistrationEvent.cs
//	Description:		Represents an arrival or departure event for each patron.
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
    /// An object that represents an arrival/departure.
    /// Contains a reference to the associated patron, the time of the event,
    /// and the EventType. Implements IComparable in order to be used
    /// with a PriorityQueue.
    /// </summary>
    /// <seealso cref="System.IComparable" />
    class RegistrationEvent : IComparable
    {
        #region Properties
        public EventType Type { get; set; }    // Signifies arrival/departure.
        public DateTime Time { get; set; }     // Time of the event.
        public Registrant Patron { get; set; } // Associated registrant.
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// Sets the type to ENTER and the time to the current time.
        /// </summary>
        public RegistrationEvent()
        {
            Type = EventType.ENTER;
            Time = DateTime.Now;
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="type">The type (ENTER/LEAVE).</param>
        /// <param name="time">The time of the event.</param>
        public RegistrationEvent(EventType type, DateTime time)
        {
            Type = type;
            Time = time;
        }
        #endregion

        #region Interface Method Implementations
        /// <summary>
        /// Compares the current instance with another object of the same type 
        /// and returns an integer that indicates whether the current instance precedes, 
        /// follows, or occurs in the same position in the sort order as the other object.
        /// Uses the event time as the comparator.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. 
        /// The return value has these meanings: Value Meaning Less than zero This instance 
        /// precedes <paramref name="obj" /> in the sort order. Zero This instance occurs 
        /// in the same position in the sort order as <paramref name="obj" />. Greater than 
        /// zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        /// <exception cref="ArgumentException">Cannot compare with RegistrationEvent.</exception>
        public int CompareTo(object obj)
        {
            // Throw an exception if the passed object is not a RegistrationEvent.
            if (!(obj is RegistrationEvent))
                throw new ArgumentException("Cannot compare with RegistrationEvent.");

            // Cast the obj as a RegistrationEvent and return the comparison of the Time properties.
            RegistrationEvent e = (RegistrationEvent)obj;
            return e.Time.CompareTo(Time);
        }
        #endregion
    }
}
