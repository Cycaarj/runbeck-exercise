// ############################################################################################
// # Author: Christopher Walmsley															  #
// # Date: 1/21/2020																		  #
// # File: Runbeck Code Exercise - DelimiterProcessor Class									  #
// ############################################################################################

using System;
using System.Collections.Generic;
using System.IO;
using Carbon.Csv;

namespace MainProgram
{
	class DelimiterProcesser
	{
		// Declare the private data members for the DelimiterProcessor class, including an external CSVReader.
		private readonly CsvReader reader;
		private readonly int numOfFields;
		private readonly char delimiter;
		private readonly string format;

		/// <summary>
		/// Constructor for the DelimiterProcessor class.
		/// </summary>
		/// <param name="path"> The file path to be read </param>
		/// <param name="dChar"> The delimiting character </param>
		/// <param name="type"> The file type (i.e. CSV or TSV) </param>
		/// <param name="fieldNum"> The number of expected fields </param>
		public DelimiterProcesser(StreamReader stream, char dChar, string type, int fieldNum)
		{
			reader = new CsvReader(stream, dChar);	
			delimiter = dChar;
			numOfFields = fieldNum;
			format = type;
		}

		/// <summary>
		/// The driving method for the DelimiterProcessor class. 
		/// </summary>
		/// <returns> An integer representing a status code </returns>
		public void Process_Input_File()
		{
			// Instantiate objects that will maintain the identified valid and invalid records. 
			List<string[]> validRecords = new List<string[]> { };
			List<string[]> invalidRecords = new List<string[]> { };
			string[] row;

			// Read the header row. If the header row does not contain the same number of columns as the expected number of fields, exit the method.
			try
			{
				row = reader.ReadRow();
			}
			catch (EndOfStreamException)
			{
				Console.WriteLine("\nERROR: Input file is empty. Exiting program.");
				return;
			}
			
			if (row.Length != numOfFields)
			{
				Console.WriteLine("\nERROR: The provided input file does not contain the expected formatting.");
				return;
			}
			// If the header is valid, begin processing the records by comparing the parsed CSV values with the expected number of fields. 
			else
			{
				while (!reader.IsEof)
				{
					// For every row, either add the record to the "valid" list if the number of elements match; otherwise, it is invalid. 
					row = reader.ReadRow();
					if (row.Length == numOfFields)
					{
						validRecords.Add(row);
					}
					else
					{
						invalidRecords.Add(row);
					}
				}
			}

			// Output file(s) will only be generated if one of the lists has at least 1 record. 
			if (validRecords.Count > 0 || invalidRecords.Count > 0)
			{
				Generate_Output_Files(validRecords, invalidRecords);
			}

			// Display a small informational piece to the user relating to the records identified and output files generated. 
			Console.WriteLine("\nFile processing was successful. Output file(s) were generated if there was at least one applicable (in)valid record.");
			Console.WriteLine("Number of valid records: " + validRecords.Count);
			Console.WriteLine("Number of invalid records: " + invalidRecords.Count);

		}

		/// <summary>
		/// Generate 1 or 2 output files based on the number of (in)valid records identified during processing. 
		/// </summary>
		/// <param name="validRecs"> The list containing valid records </param>
		/// <param name="invalidRecs"> The list containing invalid records </param>
		public void Generate_Output_Files(List<string[]> validRecs, List<string[]> invalidRecs)
		{
			// Create a directory in which to generate the output files. 
			// If the output files already exist (i.e. as a result of multiple runs), they will be overwritten. 
			string outputPath = @"C:\CW-RunbeckOutput";
			Directory.CreateDirectory(outputPath);
			File.Delete(outputPath + @"\ValidRecordFile." + format);
			File.Delete(outputPath + @"\InvalidRecordFile." + format);

			// Only create an output file if there is at least 1 record. 
			if (validRecs.Count > 0)
			{
				// Open the StreamWriter and append the valid list contents into the file following the same original format. 
				using (StreamWriter writer = File.AppendText(outputPath + @"\ValidRecordFile." + format))
				{
					foreach (string[] strArr in validRecs)
					{
						if (delimiter == ',')
						{
							writer.WriteLine(string.Join(",", strArr));
						}
						else
						{
							writer.WriteLine(string.Join("	", strArr));
						}
					}
					// Close the StreamWriter. 
					writer.Close();
				}
			}

			// Only create an output file if there is at least 1 record. 
			if (invalidRecs.Count > 0)
			{
				// Open the StreamWriter and append the invalid list contents into the file following the same original format. 
				using (StreamWriter writer = File.AppendText(outputPath + @"\InvalidRecordFile." + format))
				{
					foreach (string[] strArr in invalidRecs)
						if (delimiter == ',')
						{
							writer.WriteLine(string.Join(",", strArr));
						}
						else
						{
							writer.WriteLine(string.Join("	", strArr));
						}
					// Close the StreamWriter. 
					writer.Close();
				}
			}
		}
	}
}
