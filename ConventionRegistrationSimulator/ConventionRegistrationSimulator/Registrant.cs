//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		Registrant.cs
//	Description:		Represents a patron to the registration event.
//	Course     :		CSCI 2210-201 - Data Structures
//	Author     :		Casey Edwards, zcee10@etsu.edu
//	Created    :		Thursday, November 3rd, 2016
//	Copyright  :		Casey Edwards 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace ConventionRegistrationSimulator
{
    /// <summary>
    /// A patron of a convention.
    /// Generates an incremental ID number and holds references to both
    /// an arrival and departure event.
    /// </summary>
    class Registrant
    {
        #region Properties
        private static int Counter = 0; // Used for ID assignment.
        public string ID { get; private set; } // Identification number.
        public RegistrationEvent Arrival { get; set; } // The patron's arrival event.
        public RegistrationEvent Departure { get; set; } // The patron's departure event.
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor.
        /// Sets the ID field to the string value of Counter, increments the Counter,
        /// and associates the patron with an arrival event.
        /// </summary>
        /// <param name="arrival">The arrival event.</param>
        public Registrant(RegistrationEvent arrival)
        {
            ID = $"{Counter++:000#}";
            Arrival = arrival;
        }
        #endregion

        #region ToString
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ID;
        }
        #endregion
    }
}
