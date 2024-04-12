using VisualizzaLog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VisualizzaLog.Data;
using System.Text.RegularExpressions;
using NuGet.Packaging.Signing;
using System.Security.Cryptography;
using NuGet.Protocol;
using Newtonsoft.Json;
using VisualizzaLog.Data;

namespace VisualizzaLog.Services
{
    public class CheckViolations
    {

        public bool CheckRuleViolation(Connection connection, Rule rule)
        {
            switch (rule.Tipo)
            {
                case "IP sorgente":
                    return Regex.IsMatch(connection.SRCAddress, rule.Contenuto);

                case "IP destinazione":
                    return Regex.IsMatch(connection.DSTAddress, rule.Contenuto);

                case "Porta sorgente":
                    return Regex.IsMatch(connection.SRCPort, rule.Contenuto);

                case "Porta destinazione":
                    return Regex.IsMatch(connection.DSTPort, rule.Contenuto);

                default:
                    return false;
            }
        }

        public bool CheckArplogRuleViolation(Arplog arplog, Rule rule)
        {
            switch (rule.Tipo)
            {
                case "IP sorgente":
                    return Regex.IsMatch(arplog.Address, rule.Contenuto);

                case "Indirizzo Mac":
                    return Regex.IsMatch(arplog.MacAddress, rule.Contenuto);

                default:
                    return false;
            }
        }

    }
}