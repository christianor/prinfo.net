using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace com.monitoring.prinfo
{
    /// <summary>
    /// Hilfsklasse für den Umgang mit Segmentierten Ip`s,
    /// wird für das Iterieren durch IPv4 Adressen benötigt
    /// <example>
    /// // Beispiel einer Ip in Array-Schreibweise
    /// int[] i = new int[4];
    /// i[0] = 192;
    /// i[1] = 168;
    /// i[2] = 178;
    /// i[3] = 1;
    /// </example>
    /// </summary>
    public static class IpIterator
    {

        readonly static int ipSegSize = 4;

        /// <summary>
        /// Wandelt einen String in eine segmentierte IP um
        /// </summary>
        /// <param name="ip">Eine gültige IPv4</param>
        /// <returns>Segmentierte IP int[4]</returns>
        public static int[] StringToIpSeg(string ip)
        {
            // check ip
            System.Net.IPAddress.Parse(ip);
            return StringArrayToIpSegs(ip.Split('.'));
        }

        /// <summary>
        /// Wandelt eine segmentierte Ip in einen String um
        /// </summary>
        /// <param name="ipSegs">Segmentierte IPv4</param>
        /// <returns>IPv4 als String</returns>
        public static string IpSegsToString(int[] ipSegs)
        {
            string returnString = String.Empty;

            for (int i = 0; i < ipSegSize; i++)
            {
                returnString += ipSegs[i].ToString();
                if (i < 3)
                    returnString += '.';
            }

            return returnString;
        }

        /// <summary>
        /// Hilfsmethode die die String Ip-Segmente in Integer Ip-Segmente konvertiert
        /// </summary>
        /// <param name="arr">Array String Segmente</param>
        /// <returns></returns>
        public static int[] StringArrayToIpSegs(string[] arr)
        {
            int[] intArr = new int[arr.Length];

            for (int i = 0; i < ipSegSize; i++)
            {
                intArr[i] = int.Parse(arr[i]);
            }

            return intArr;
        }

        /// <summary>
        /// Prüft ob die segmentierte Start-IP größer ist als die segmentierte End-IP
        /// </summary>
        /// <param name="startIp">Segmentierte IP als Ausgang</param>
        /// <param name="endIp">Segmentierte IP die das Ende der Iteration darstellen soll</param>
        /// <returns>Gibt "false" zurück falls "startIp" und "endIp" nicht zusammen passen</returns>
        public static bool ValidateStartAndEnd(int[] startIp, int[] endIp)
        {

            if (endIp[0] > startIp[0]) return true;

            if (endIp[0] == startIp[0] &&
                endIp[1] > startIp[1]) return true;

            if (endIp[0] == startIp[0] &&
                endIp[1] == startIp[1] &&
                endIp[2] > startIp[2]) return true;

            if (endIp[0] == startIp[0] &&
                endIp[1] == startIp[1] &&
                endIp[2] == startIp[2] &&
                endIp[3] > startIp[3]) return true;

            return false;

        }

        /// <summary>
        /// Vergleicht ob 2 segmentierte Ips gleich sind
        /// </summary>
        /// <param name="ipSegs1">Segmentierte Ip</param>
        /// <param name="ipSegs2">Die zu vergleichende segmentierte Ip</param>
        /// <returns>"true" falls die segmentierten IPv4`s gleich sind</returns>
        public static bool IpSegsEqual(int[] ipSegs1, int[] ipSegs2)
        {
            for (int i = 0; i < ipSegSize; i++)
            {
                if (ipSegs1[i] != ipSegs2[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Erhöt eine segmentierte IP um den Wert 1
        /// <remarks>
        /// 10.1.1.1 + 1 = 10.1.1.2
        /// oder
        /// 10.1.1.255 + 1 = 10.1.2.1
        /// </remarks>
        /// </summary>
        /// <param name="ipSegs">Segmentierte IPv4</param>
        public static void RaiseIpSegs(int[] ipSegs)
        {
            for (int i = ipSegSize - 1; i > -1; i--)
            {
                if (ipSegs[i] < 255)
                {
                    ipSegs[i]++;
                    break;
                }
                else if (i != 0)
                {
                    ipSegs[i] = 1;
                }
                else break;
            }
        }

        /// <summary>
        /// Gibt segmentierte IPv4 aus
        /// </summary>
        /// <param name="ipSegs"></param>
        public static void WriteIpSegsToStd(int[] ipSegs)
        {
            int i = 0;
            foreach (int j in ipSegs)
            {
                Console.Write(ipSegs[i]);

                if (i < 3)
                    Console.Write('.');

                i++;
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Berechnet die Schrittdistanz zwischen zwei segmentierten IPv4`s
        /// </summary>
        /// <param name="ipStart">Ausgang</param>
        /// <param name="ipEnd">Ende</param>
        /// <returns>Schrittdistanz zwischen den IPs</returns>
        public static int CalcIpSegsDistance(int[] ipStart, int[] ipEnd)
        {

            if (!ValidateStartAndEnd(ipStart, ipEnd))
                return 0;
            else
            {
                int value = 0;


                if(ipStart[0] < ipEnd[0])
                {
                    int segmentBreite = ipEnd[0] - ipStart[0];

                }

                return value;
            }
            

        }
    }
}
