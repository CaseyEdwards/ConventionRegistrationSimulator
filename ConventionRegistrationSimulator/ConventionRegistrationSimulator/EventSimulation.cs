//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		EventSimulation.cs
//	Description:		Object representing a registration event simulation.
//	Course     :		CSCI 2210-201 - Data Structures
//	Author     :		Casey Edwards, zcee10@etsu.edu
//	Created    :		Thursday, November 3rd, 2016
//	Copyright  :		Casey Edwards 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;

namespace ConventionRegistrationSimulator
{
    /// <summary>
    /// Simulates an event registration.
    /// Generates and tracks patrons, arrival times, and window wait times.
    /// Uses random number generators utilizing statistical distributions to
    /// reflect real-life fluctuations. Allows for the setting of the expected
    /// number of patrons, number of hours registration is open, the number of
    /// open registration windows, and the expected average wait.
    /// </summary>
    class EventSimulation
    {
        #region Properties
        public bool SimulationComplete { get; set; }  // Flags true when the simulation finishes.
        private Random R = new Random();              // Random number generator.
        private PriorityQueue<RegistrationEvent> PQ = // Priority Queue for arrival and departure events.
                new PriorityQueue<RegistrationEvent>();
        private List<Queue<Registrant>> windowList =  // List of registration window queues.
                new List<Queue<Registrant>>();
        private DateTime TimeWeOpen = new DateTime(   // Opening time.
            DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 6, 0, 0);
        private int HoursOpen,           // Number of hours open.
                    NumPatrons,          // Number of expected patrons.
                    NumWindows,          // Number of open registration windows.
                    MaxLineLength = 0,   // Length of the longest line.
                    NumDepartures = 0;   // Number of handled departure events.
        private TimeSpan shortest = new TimeSpan(0, 1000000, 0), // Shortest wait time.
                         longest = new TimeSpan(0, 0, 0),        // Longest wait time.
                         totalTime = new TimeSpan(0, 0, 0),      // Wait time total.
                         expectedAverageWait;                    // Expected average wait time.
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// Sets the number of patrons to 1000, the windows to 5,
        /// the hours open to 10, and the expected wait time to 4m30s.
        /// </summary>
        public EventSimulation()
        {
            NumPatrons = Poisson(1000);
            NumWindows = 5;
            HoursOpen = 10;
            expectedAverageWait = new TimeSpan(0, 0, 270);

            InitializeSimulation();
        }

        /// <summary>
        /// Parameterized constructor.
        /// Allows for the setting of the number of patrons, open windows,
        /// hours open, and the expected average wait time.
        /// </summary>
        /// <param name="numPatrons">The number patrons.</param>
        /// <param name="numWindows">The number windows.</param>
        /// <param name="hoursOpen">The hours open.</param>
        /// <param name="expAvg">The expected average wait time.</param>
        public EventSimulation(int numPatrons, int numWindows, 
                                int hoursOpen, TimeSpan expAvg)
        {
            NumPatrons = Poisson(numPatrons);
            NumWindows = numWindows;
            HoursOpen = hoursOpen;
            expectedAverageWait = expAvg;

            InitializeSimulation();
        }
        #endregion

        #region Simulation Management
        /// <summary>
        /// Initializes the simulation by setting the appropriate number
        /// of queues to the window list, generating the arrival events,
        /// and setting the SimulationComplete flag to false;
        /// </summary>
        public void InitializeSimulation()
        {
            SimulationComplete = false;
            GenerateArrivals();
            for (int i = 0; i < NumWindows; i++)
                windowList.Add(new Queue<Registrant>());
        }

        /// <summary>
        /// Walks one step through the simulation.
        /// If the priority queue is empty, it simply sets the Simulation Complete
        /// flag to true. Otherwise, it handles the logic for dealing with new
        /// arrivals and departures, removing one item from the queue per step.
        /// </summary>
        public void StepSimulation()
        {
            if (!PQ.IsEmpty())
            {
                // Activate the next item on the PQ
                if (PQ.Peek().Type == EventType.ENTER)
                {
                    // New registrant has arrived, find the shortest line
                    // and place the registrant there.
                    Queue<Registrant> shortestLine = windowList[0];
                    for (int i = 1; i < windowList.Count; i++)
                        if (windowList[i].Count < shortestLine.Count)
                            shortestLine = windowList[i];
                    shortestLine.Enqueue(PQ.Peek().Patron);

                    // Update max line length, if applicable.
                    if (shortestLine.Count > MaxLineLength)
                        MaxLineLength = shortestLine.Count;

                    // If they entered an empty line, generate an exit event.
                    if (shortestLine.Count == 1)
                        GenerateDeparture(shortestLine.Peek(), PQ.Peek().Time);

                    // Dequeue the event.
                    PQ.Dequeue();
                }
                else if (PQ.Peek().Type == EventType.LEAVE)
                {
                    Queue<Registrant> shrinkingLine = null;

                    // Patron is leaving; find the line they are leaving from
                    // and adjust the line accordingly.
                    foreach (Queue<Registrant> q in windowList)
                    {
                        if (q.Count != 0 && q.Peek().ID == PQ.Peek().Patron.ID)
                        {
                            shrinkingLine = q;
                            break;
                        }
                    }

                    // pop the PQ and line queue and, if the line is not yet empty, generate
                    // an exit event for the next registrant
                    shrinkingLine.Dequeue();
                    if (shrinkingLine.Count != 0)
                        GenerateDeparture(shrinkingLine.Peek(), PQ.Peek().Time);
                    PQ.Dequeue();
                }

            }
            else // PQ is empty, set the complete flag.
                SimulationComplete = true;
        }

        /// <summary>
        /// Updates the tracking variables for shortest and longest waits,
        /// and increments the total wait time.
        /// </summary>
        /// <param name="waitTime">The wait time.</param>
        public void UpdateTimeTrackers(TimeSpan waitTime)
        {
            // Increment wait time and the number of handled departures.
            totalTime += waitTime;
            NumDepartures++;

            // Compare the wait time to the current shortest and longest,
            // and set the relevant property where applicable.
            if (waitTime < shortest)
                shortest = waitTime;
            if (waitTime > longest)
                longest = waitTime;
        }

        /// <summary>
        /// Generates a departure event for a patron.
        /// These should be created when a registrant begins their transaction
        /// at the registration window. A negative exponential curve is provided
        /// given the expected average wait time. It is assumed 90 seconds is the
        /// minimum transaction time.
        /// </summary>
        /// <param name="patron">The patron.</param>
        /// <param name="currentTime">The current time.</param>
        public void GenerateDeparture(Registrant patron, DateTime currentTime)
        {
            TimeSpan waitTime = new TimeSpan(0, 0, // The registration window time span.
                (int)(90 + NegativeExponential(expectedAverageWait.TotalSeconds - 90)));
            RegistrationEvent ev = new RegistrationEvent(EventType.LEAVE,
                currentTime + waitTime); // The departure event.

            // Link the patron to the departure event, enqueue the event,
            // and update the wait time tracking variables.
            ev.Patron = patron;
            ev.Patron.Departure = ev;
            PQ.Enqueue(ev);
            UpdateTimeTrackers(waitTime);
        }

        /// <summary>
        /// Generates the arrival events for the patrons.
        /// Creates events to span the entire time the booths are open on
        /// an even distribution and adds the events to the priority queue.
        /// </summary>
        public void GenerateArrivals()
        {
            TimeSpan start; // The time of arrival.
            const int SECONDS_PER_HOUR = 3600;

            for (int i = 0; i < NumPatrons; i++)
            {
                // Create the span of time starting from the registration opening time,
                // use that time to create a new arrival event, create a patron for the
                // event, and enqueue.
                start = new TimeSpan(0, 0, R.Next(HoursOpen * SECONDS_PER_HOUR));
                RegistrationEvent ev = new RegistrationEvent(EventType.ENTER, TimeWeOpen + start);
                ev.Patron = new Registrant(ev);
                PQ.Enqueue(ev);
            }
        }
        #endregion

        #region Random Number Distribution Methods
        /// <summary>
        /// Provides a negative exponential distribution random number.
        /// </summary>
        /// <param name="ExpectedValue">The expected average value.</param>
        /// <returns>A randomly generated double on a negative exponential curve.</returns>
        private double NegativeExponential(double ExpectedValue)
        {
            return -ExpectedValue * Math.Log(R.NextDouble(), Math.E);
        }

        /// <summary>
        /// Provides a randomly generated number based on the Poisson distribution
        /// centered on the given expected value.
        /// </summary>
        /// <param name="ExpectedValue">The expected value.</param>
        /// <returns>A randomly generated integer on the Poisson distribution.</returns>
        private int Poisson(double ExpectedValue)
        {
            double dLimit = -ExpectedValue;
            double dSum = Math.Log(R.NextDouble());

            int Count;
            for (Count = 0; dSum > dLimit; Count++)
                dSum += Math.Log(R.NextDouble());

            return Count;
        }
        #endregion

        #region ToString
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the simulation.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            String representation = String.Empty; // The string representation.

            // For each registration window, add a line with:
            //  - The window token
            //  - ID of the person currently registering.
            //  - A token (@@@@) for each person waiting in line.
            foreach (Queue<Registrant> q in windowList)
            {
                representation += "WINDOW] ";
                if (q.Count != 0)
                    representation += q.Peek();
                for (int i = 1; i < q.Count; i++)
                    representation += " @@@@";
                representation += "\n";
            }

            // Add a separator and display the simulation statistics.
            representation += "---------------------------\n";
            representation += $"Shortest wait: {shortest}\n";
            representation += $"Longest wait : {longest}\n";
            representation += $"Average wait : {new TimeSpan(0, 0, (int)totalTime.TotalSeconds / NumDepartures)}\n";
            representation += $"Longest Line : {MaxLineLength}\n";
            representation += $"Registrants  : {NumDepartures}";

            return representation;
        }
        #endregion
    }
}
