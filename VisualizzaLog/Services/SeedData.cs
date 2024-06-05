using VisualizzaLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VisualizzaLog.Data;
using System.Text.RegularExpressions;
using NuGet.Packaging.Signing;
using System.Security.Cryptography;
using NuGet.Protocol;
using Newtonsoft.Json;

namespace VisualizzaLog.Services
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<VisualizzaLogContext>();

            var connectionsFilePath = "Services/Files/connections.txt";
            var arplogFilePath = "Services/Files/arplog.txt";

            if (!File.Exists(connectionsFilePath) || !File.Exists(arplogFilePath))
            {
                throw new FileNotFoundException("Uno o entrambi i file di log non sono presenti.");
            }

            var lastConnectionHash = dbContext.FileHashes.FirstOrDefault(fh => fh.FileName == "connections.txt");
            var lastArplogHash = dbContext.FileHashes.FirstOrDefault(fh => fh.FileName == "arplog.txt");

            var currentConnectionHash = GetFileHash(connectionsFilePath);
            var currentArplogHash = GetFileHash(arplogFilePath);

            bool isFirstLoad = lastConnectionHash == null && lastArplogHash == null;

            if (!isFirstLoad && lastConnectionHash != null && lastConnectionHash.Hash == currentConnectionHash &&
                lastArplogHash != null && lastArplogHash.Hash == currentArplogHash)
            {
                // I file non sono stati sostituiti, quindi non è necessario eseguire l'inizializzazione
                return;
            }


            string[] connectionLines = File.ReadAllLines(connectionsFilePath);
            string[] arplogLines = File.ReadAllLines(arplogFilePath);

            var dateRegex = new Regex(@"^#\s*(\w{3}/\d{1,2}/\d{4})");

            var connectionDateMatch = dateRegex.Match(connectionLines[0]);
            var arplogDateMatch = dateRegex.Match(arplogLines[0]);

            if (!connectionDateMatch.Success || !arplogDateMatch.Success)
            {
                // Formato della data non valido, gestisci l'errore di conseguenza
                throw new FormatException("Formato della data non valido nei file di log.");
            }

            var connectionTimestamp = DateTime.Parse(connectionDateMatch.Groups[1].Value);
            var arplogTimestamp = DateTime.Parse(arplogDateMatch.Groups[1].Value);

            string[] connLines = connectionLines.Skip(6).ToArray();

            foreach (var connLine in connLines)
            {
                var fields = connLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (!(fields[2].Contains('s') || fields[2].Contains('d') && !fields[2].Contains("udp")))
                {
                    Array.Resize(ref fields, fields.Length + 1);
                    fields[fields.Length - 1] = fields[fields.Length - 2].Trim();
                    fields[fields.Length - 2] = fields[fields.Length - 3].Trim();
                    fields[fields.Length - 3] = fields[fields.Length - 4].Trim();
                    fields[fields.Length - 4] = fields[2].Trim();
                    fields[2] = " ";
                }

                var connection = new Connection
                {
                    ConnectionTimestamp = connectionTimestamp,
                    Flag = fields[1].Trim(),
                    NatFlag = fields[2].Trim(),
                    Protocol = fields[3].Trim(),
                    SRCAddress = fields[4].Split(':')[0].Trim(),
                    SRCPort = (fields[4].Split(':').Length > 1 ? fields[4].Split(':')[1].Trim() : ""),
                    DSTAddress = fields[5].Split(':')[0].Trim(),
                    DSTPort = (fields[5].Split(':').Length > 1 ? fields[5].Split(':')[1].Trim() : ""),
                    TCPState = fields.Length >= 7 ? fields[6] : null
                };

                dbContext.Connections.Add(connection);
            }

            string[] arpLines = arplogLines.Skip(6).ToArray();

            foreach (var arpLine in arpLines)
            {
                var fields = arpLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (!fields[3].Contains(':'))
                {
                    Array.Resize(ref fields, fields.Length + 1);
                    fields[fields.Length - 1] = fields[fields.Length - 2].Trim();
                    fields[fields.Length - 2] = " ";
                }

                if (fields.Length < 4) continue;

                var arplog = new Arplog
                {
                    ArplogTimestamp = arplogTimestamp,
                    Flag = fields[1],
                    Address = fields[2],
                    MacAddress = fields[3],
                    Interface = string.Join(" ", fields.Skip(4))
                };

                dbContext.Arplogs.Add(arplog);
            }

            if (isFirstLoad)
            {
                
                dbContext.FileHashes.Add(new FileHash { FileName = "connections.txt", Hash = currentConnectionHash });
                dbContext.FileHashes.Add(new FileHash { FileName = "arplog.txt", Hash = currentArplogHash });
            }
            else
            {
                
                UpdateFileHash(dbContext, "connections.txt", currentConnectionHash);
                UpdateFileHash(dbContext, "arplog.txt", currentArplogHash);
            }


            dbContext.SaveChanges();
        }

        public static void UpdateFileHash(VisualizzaLogContext dbContext, string fileName, string hash)
        {
            var fileHash = dbContext.FileHashes.FirstOrDefault(fh => fh.FileName == fileName);
            if (fileHash != null)
            {
                fileHash.Hash = hash;
            }
            else
            {
                dbContext.FileHashes.Add(new FileHash { FileName = fileName, Hash = hash });
            }
        }

        public static string GetFileHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }
        }
    }
}
