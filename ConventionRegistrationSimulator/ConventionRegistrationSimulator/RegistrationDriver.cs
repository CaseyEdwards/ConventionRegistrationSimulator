//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		RegistrationDriver.cs
//	Description:		Handles user input/output for the Convention Registration Simulation.
//                          Allows the user to set parameters and watch as simulations unfold.
//	Course     :		CSCI 2210-201 - Data Structures
//	Author     :		Casey Edwards, zcee10@etsu.edu
//	Created    :		Thursday, November 3rd, 2016
//	Copyright  :		Casey Edwards 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using UtilityNamespace;
using System.Threading;

namespace ConventionRegistrationSimulator
{
    /// <summary>
    /// Driver for the simulation program.
    /// Handles all user I/O and handles the running and displaying of the simulation.
    /// Allows the user to set parameters to change the function of the simulation.
    /// </summary>
    class RegistrationDriver
    {
        #region Properties
        private static EventSimulation sim;  // The simulation object.
        private static TimeSpan AvgWaitTime  // The average transaction time.
            = new TimeSpan(0, 0, 270);
        private static int NumPatrons = 1000,// Number of expected patrons.
                           NumWindows = 5,   // Number of registration windows opened.
                           HoursOpen = 10;   // Number of hours open.
        #endregion

        #region Main
        /// <summary>
        /// Defines the entry point of the application.
        /// Uses a menu to allow the user to change parameters such as number of
        /// expected guests, average wait time, number of hours the event is
        /// accepting new arrivals, and the number of open windows. Runs and displays
        /// a simulation based on these parameters.
        /// </summary>
        static void Main()
        {
            Menu menu = BuildMenu();     // The main menu.
            MenuOption UserMenuChoice;  // The user's menu choice.

            Console.BackgroundColor = ConsoleColor.White;
            
            // Until the user wishes to exit, display the menu and get
            // their choice, then take the appropriate action.
            do
            {
                menu.Display();
                UserMenuChoice = (MenuOption)menu.GetChoice();
                HandleMenuSelection(UserMenuChoice);
            } while (UserMenuChoice != MenuOption.EXIT);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Performs the action specified by the user's menu selection.
        /// </summary>
        /// <param name="choice">The menu choice.</param>
        private static void HandleMenuSelection(MenuOption choice)
        {
            // Perform the operation dictated by the user.
            switch (choice)
            {
                #region Run Simulation
                case MenuOption.RUN_SIMULATION:
                    // Run the simulation and prompt for keypress when finished.
                    RunSimulation();
                    Console.WriteLine("\n\tPress any key to return to the main menu.");
                    Console.ReadKey();
                    break;
                #endregion
                #region Set Number of Patrons
                case MenuOption.SET_NUM_PATRONS:
                    // Prompt user for the number of patrons.
                    Console.Clear();
                    Console.Write("\n\n\tEnter the number of expected patrons -> ");
                    try
                    {
                        NumPatrons = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"\nError: {ex.Message}.\nPress any key to return to main menu.");
                        Console.ReadKey();
                    }
                    break;
                #endregion
                #region Set Hours Open
                case MenuOption.SET_HOURS_OPEN:
                    // Prompt user for the number of hours open
                    Console.Clear();
                    Console.Write("\n\n\tEnter the number of hours registration is open -> ");
                    try
                    {
                        HoursOpen = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"\nError: {ex.Message}.\nPress any key to return to main menu.");
                        Console.ReadKey();
                    }
                    break;
                #endregion
                #region Set Number of Windows
                case MenuOption.SET_NUM_WINDOWS:
                    // Prompt user for the number of available registration windows.
                    Console.Clear();
                    Console.Write("\n\n\tEnter the number of registration windows -> ");
                    try
                    {
                        NumWindows = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"\nError: {ex.Message}.\nPress any key to return to main menu.");
                        Console.ReadKey();
                    }
                    break;
                #endregion
                #region Set Average Wait Time
                case MenuOption.SET_AVG_WAIT:
                    // Prompt user for the average transaction time in seconds.
                    Console.Clear();
                    Console.Write("\n\n\tEnter the expected average transaction time, in *seconds* -> ");
                    try
                    {
                        AvgWaitTime = new TimeSpan(0, 0, Convert.ToInt32(Console.ReadLine()));
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"\nError: {ex.Message}.\nPress any key to return to main menu.");
                        Console.ReadKey();
                    }
                    break;
                #endregion
                #region Exit Program
                case MenuOption.EXIT:
                    // Thank the user for using the program and wait for a keypress to exit.
                    Console.Clear();
                    Console.WriteLine("\n\n\n\t\tThank you for using the");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\t\tConvention Registration Simulator");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\n\n\t\tPress any key to exit.");
                    Console.ReadKey();
                    break;
                #endregion
            }
        }

        /// <summary>
        /// Runs the simulation.
        /// Steps through the simulation, printing the status to the
        /// console. Sleeps the thread on each pass for better readability.
        /// </summary>
        private static void RunSimulation()
        {
            // Create a new simulation object with the chosen parameters.
            sim = new EventSimulation(NumPatrons, NumWindows, HoursOpen, AvgWaitTime);

            while (!sim.SimulationComplete)
            {
                // Until the simulation is complete, step through the simulation,
                // printing the simulation representation after each step.
                // Sleep time of 100ms is suggested.
                Console.Clear();
                sim.StepSimulation();
                Console.WriteLine(sim);
                //Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Builds the main menu.
        /// </summary>
        /// <returns>The completed menu.</returns>
        private static Menu BuildMenu()
        {
            // Create the menu object and set the title.
            Menu m = new Menu("Convention Registration Simulation");

            // Add each option on a new line.
            m += "Run a Simulation";
            m += "Set Number of Expected Patrons";
            m += "Set Number of Hours Open";
            m += "Set Number of Registration Windows";
            m += "Set Length of Average Transaction";
            m += "Exit Program";

            return m;
        }
        #endregion
    }
}
