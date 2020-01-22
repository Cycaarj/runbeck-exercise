// ############################################################################################
// # Author: Christopher Walmsley															  #
// # Date: 1/21/2020																		  #
// # File: Runbeck Code Exercise - DelimiterProcessor Class									  #
// ############################################################################################

using System;
using System.IO;

namespace MainProgram
{
	class Program
	{
		static void Main(string[] args)
		{
		// ############################################################################################
		// # INIT																					  #
		// ############################################################################################
			// Declare the variables that will be used to track the question responses and process the input. 
			string filePath, fileType;
			int fieldCount = -1;
			StreamReader stream = null;
			DelimiterProcessor processor;

			// Display the welcome message and questions. 
			// Store the responses given to the questions into the aforementioned string variables.
			Console.WriteLine("Welcome to the Delimited Text File Processing Application");
			Console.WriteLine("==========================================================\n");
			Console.WriteLine("Please provide answers to each of the following questions.\n");

		// ############################################################################################
		// # QUESTION ONE																			  #
		// ############################################################################################
			// Open the file for reading. Ensure that the file path is correct. 
			Console.WriteLine("1. What is the file path of the file that needs to be processed? If applicable, please include the file extension.");
			do
			{
				filePath = Console.ReadLine();
				try
				{
					stream = new StreamReader(filePath);
				}
				catch (FileNotFoundException)
				{
					Console.WriteLine("\nThe file was not found. Please provide a valid file path.");
				}
				catch (ArgumentException)
				{
					Console.WriteLine("\nThe file was not found. Please provide a valid file path.");
				}
				catch (PathTooLongException)
				{
					Console.WriteLine("\nPlease provide a valid file path that is fewer than 260 characters.");
				}
			} while (stream == null);			
			
		// ############################################################################################
		// # QUESTION TWO																			  #
		// ############################################################################################
			Console.WriteLine("\n2. Is the file format CSV (comma-separated values) or TSV (tab-separated values)?");
			fileType = Console.ReadLine().ToLower();

			// The only acceptable answers to this question are "CSV" and "TSV." 
			// The program will loop until a user provides one of these answers.
			while (!fileType.Equals("csv") && !fileType.Equals("tsv"))
			{
				Console.WriteLine("\nThe only acceptable file types are CSV or TSV. Please provide a valid format name.");
				fileType = Console.ReadLine().ToLower();
			}

		// ############################################################################################
		// # QUESTION THREE																			  #
		// ############################################################################################
			// The system will only allow a user to input an integer value. 
			Console.WriteLine("\n3. How many fields should each record contain (as an integer value)?");
			while (fieldCount == -1)
			{
				try
				{
					fieldCount = int.Parse(Console.ReadLine());
				}
				catch (FormatException)
				{
					Console.WriteLine("\nPlease try again. Your response should only contain an integer value.");
				}
			}
			
		// ############################################################################################
		// # FILE PROCESSING																		  #
		// ############################################################################################
			// Display the user's inputs.
			Console.WriteLine("\nThese are the answers you provided for each question respectively:\n" 
									+ filePath + ", " + fileType + ", " + fieldCount);

			// Begin processing on the input file. 
			if (fileType.Equals("csv"))
			{
				processor = new DelimiterProcessor(stream, ',', "csv", fieldCount);
				processor.Process_Input_File();
			}
			else
			{
				processor = new DelimiterProcessor(stream, '\t', "tsv", fieldCount);
				processor.Process_Input_File();
			}

			// Close the input stream. 
			stream.Close();
		}
	}
}
