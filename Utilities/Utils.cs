using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Bodycam.Utilities
{
    public static class BodycamIDManager
    {
        private static readonly string idDirectory = @"C:\Bodycam\id";
        private static readonly string idFilePath = Path.Combine(idDirectory, "Bodycam_id.bcid");

        public static string GetOrCreateBodycamID()
        {
            // Ensure the id directory exists
            if (!Directory.Exists(idDirectory))
            {
                Directory.CreateDirectory(idDirectory);
            }

            // Check if ID file exists
            if (File.Exists(idFilePath))
            {
                return File.ReadAllText(idFilePath);
            }

            // Generate a new id
            string newId = GenerateUniqueID();

            // Save the new ID to the file
            File.WriteAllText(idFilePath, newId);

            return newId;
        }

        private static string GenerateUniqueID()
        {
            // Get a system-specific identifier (e.g., hardware GUID)
            string systemIdentifier = GetSystemIdentifier();

            // Combine it with a random string to make the ID unique
            string randomComponent = GenerateRandomString(8);

            string uniqueID = $"Axon4-{randomComponent}";

            return uniqueID;
        }

        private static string GetSystemIdentifier()
        {
            // Use the machine GUID from the registrey (this is generally unique per machine)
            string machineGuid = string.Empty;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography"))
            {
                if (key != null)
                {
                    object guidValue = key.GetValue("MachineGuid");
                    if (guidValue != null)
                    {
                        machineGuid = guidValue.ToString();
                    }
                }
            }

            return machineGuid;
        }

        private static string GenerateRandomString(int length)
        {
            const string vaildChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder result = new StringBuilder(length);
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                foreach (byte b in randomBytes)
                {
                    result.Append(vaildChars[b % vaildChars.Length]);
                }
            }
            return result.ToString();
        }
    }
}