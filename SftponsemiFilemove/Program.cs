using System;
using System.IO;
using Renci.SshNet;

namespace SFTPconnect
{
	internal static class Program
	{
		static void Main()
		{
			string localDirectory = @"C:\xml\de_xml\"; // Source directory where files are located
			string backupDirectory = @"C:\xml\backup_relocate\"; // relocate directory files
			string sftpDirectory = "/files/FSC/de_xml/"; // Remote SFTP directory path

			try
			{
				// Initialize SFTP client
				using (var client = new SftpClient("112.199.64.167", "onsemi_system", "ONSemi1*"))
				{
					client.Connect();

					string[] files = Directory.GetFiles(localDirectory);

					foreach (var filePath in files)
					{
						string fileName = Path.GetFileName(filePath);
						using (var fileStream = new FileStream(filePath, FileMode.Open))
						{
							client.UploadFile(fileStream, sftpDirectory + fileName);
						}

						string destinationFile = Path.Combine(backupDirectory, fileName);
						File.Move(filePath, destinationFile);
					}

					client.Disconnect();
				}

				Console.WriteLine("Files uploaded and moved to backup directory successfully!");
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}
			Environment.Exit(0);
		}
	}
}
