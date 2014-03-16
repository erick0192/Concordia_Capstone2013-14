using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using MinimalisticTelnet;

namespace Communication
{
    public class RouterCnx
    {
        private string IPAdress;
        private string Login;
        private string Password;
        private TelnetConnection TelnetCnx;
                
        /// <summary> 
        /// Establish a Telnet connection with the router 
        /// </summary>
        public RouterCnx(string aIPAddress, string aLogin, string aPassword)
        {
            IPAdress = aIPAddress;
            Login = aLogin;
            Password = aPassword;
            
            TelnetCnx = new TelnetConnection(IPAdress, 23);

            string Response = TelnetCnx.Login(Login, Password, 300);
            string prompt = Response.TrimEnd();

            prompt = Response.Substring(prompt.Length - 1, 1);

            if (prompt != "$" && prompt != ">" && prompt != "#")
            {
                throw new Exception("Unable to connect to the requested IP");
            }
            
        }
         
        /// <summary>
        /// Connect to the Router
        /// </summary>
        /// <returns>the status of the connection</returns>
        public bool Connect()
        {
            return TelnetCnx.IsConnected;
        }

        public int GetRSSIValue(string MACAddress)
        {
            string ReturnString;
            string pat = @"[+-]\d+";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            TelnetCnx.WriteLine("wl scan");
            Thread.Sleep(300);
            ReturnString = TelnetCnx.Read();

            TelnetCnx.WriteLine("wl rssi " + MACAddress);
            Thread.Sleep(300);

            ReturnString = TelnetCnx.Read();
            Match m = r.Match(ReturnString);

            if (m.Success == false)
            {
                throw new Exception("Unable to extract RSSI Value");
            }

            return int.Parse(m.ToString()) ;
        }

        
    }
}
