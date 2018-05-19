using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace BarberSystem.Controle {
    class Log {

        public static StringBuilder log = new StringBuilder();
        public static void logException(Exception e){
            log.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            log.AppendLine();
            log.Append(e.Message);
            log.AppendLine();
            log.Append(e.StackTrace);
            log.AppendLine();
            if(e.InnerException != null){
                log.AppendLine("InnerException: " + e.InnerException.Message);
            }
            File.AppendAllText("LogError_BarberSystem.txt", log.ToString());
            log.Clear();
        }

        public static void logMessage(string message){
            log.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            log.Append(" " + message);
            log.AppendLine();
            File.AppendAllText("LogMessage_BarberSystem.txt", log.ToString());
            log.Clear();
        }
        
      


       

     /*
        // < constantes \>
        const string LOG_NAME = "BarberLog";
        const string SOURCE = "App";

        // < construtor \>
        public Log() {
            // < verifica se o log existe, se não cria \>
            if (EventLog.SourceExists(SOURCE)) {
                EventLog.CreateEventSource(SOURCE, LOG_NAME);
            }
        }

        public void WriteEntry(string entrada, EventLogEntryType tipoEntrada) {
            // < grava o texto na fonte de logs com o nome que definimos para a constante SOURCE \>
            EventLog.WriteEntry(SOURCE, entrada, tipoEntrada);
        }

        public void WriteEntry(string entrada) {

            WriteEntry(entrada, EventLogEntryType.Information);
        }

        public void WriteEntry(Exception e) {

            WriteEntry(e.ToString(), EventLogEntryType.Error);
        }*/
    }
}
