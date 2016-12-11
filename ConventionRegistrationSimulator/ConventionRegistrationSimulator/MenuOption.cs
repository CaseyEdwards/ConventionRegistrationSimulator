//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project    :		Project 4 - Convention Registration Simulator
//	File Name  :		MenuOptions.cs
//	Description:		Enumerated type representing main menu options.
//	Course     :		CSCI 2210-201 - Data Structures
//	Author     :		Casey Edwards, zcee10@etsu.edu
//	Created    :		Friday, November 4th, 2016
//	Copyright  :		Casey Edwards 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace ConventionRegistrationSimulator
{
    /// <summary>
    /// Enumerated type representing the main menu options.
    /// Each option is assigned an integer value for converting from user input to enum.
    /// </summary>
    enum MenuOption
    {
        RUN_SIMULATION = 1,  // Run the simulation.
        SET_NUM_PATRONS = 2, // Set expected number of patrons.
        SET_HOURS_OPEN = 3,  // Set the number of hours open.
        SET_NUM_WINDOWS = 4, // Set the number of registration windows.
        SET_AVG_WAIT = 5,    // Set the average wait time.
        EXIT = 6             // Exit the program.
    }
}
