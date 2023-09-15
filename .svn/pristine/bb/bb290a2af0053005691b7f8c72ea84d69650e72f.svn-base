using Business;
using HelpersLibrary;
using ReseauPsy.Models;
using ReseauPsy.Models.Therapist;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerRunner
{
    class Program
    {
        #region Catch Application Stop

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            // Exiting system due to external CTRL-C, or process kill, or shutdown
            EmailHelper.Send("SERVERSYNCRUNNER STOPPED", "SYNC ReseauPsy - ServerSyncRunner Stopped on : " + DateTime.Now, ConfigurationManager.AppSettings["EmailTo"], "");

            Console.WriteLine("System Exiting...");

            // If you have some cleanup please make it here. Yeah.. cleanup is always boring to do..
            // Thread.Sleep(5000); //simulate some cleanup delay
            // Console.WriteLine("Cleanup complete");

            // Force a garbage collection to occur for this demo.
            GC.Collect();

            // allow main to run off

            // shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }
        #endregion

        static int currentSeconds;

        static void Main(string[] args)
        {

            currentSeconds = 0;

            // To catching up the ShutDown
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("SERVERSYNCRUNNER RESEAU PSY START, Never close that application");

            EmailHelper.Send("SERVERSYNC RUNNER RESEAU PSY STARTED", string.Format("ServerSyncRunner Started on : {0}", DateTime.Now), ConfigurationManager.AppSettings["EmailTo"], string.Empty);


            while (true)
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    Console.WriteLine(string.Format("{0} - Job started\n", DateTime.Now));

                    SendAppointmentEmailReminder();
                    DeleteTherapistClientRequestExpired();

                    stopwatch.Stop();
                    Console.WriteLine($"Job Completed in {Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} secondes");
                }
                catch (Exception ex)
                {
                    EmailHelper.Send("SERVERSYNCRUNNER GOT ERROR", "SYNC RESEAU PSY - ServerSyncRunner Stopped on : " + DateTime.Now + ex.Message, ConfigurationManager.AppSettings["EmailTo"], "");
                }

                int sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings["SleepTime"]);
                Thread.Sleep(sleepTime); // 3 600 000 => 1 heure -- 300 000 => 5 min ---- CODE exécuté à toutes les seconde pour vérifier si on doit lancer quelque chose
            }
        }


        private static void SendAppointmentEmailReminder()
        {
            Console.WriteLine("Début de l'envoie des courriels");

            var _context = new ReseauPsyEntities();

            var date24hours = DateTime.Now.AddHours(24);

            var tomorrowAppointments = _context.ClientAppointments
                .Where(x => x.StartDateTime <= date24hours && !x.IsRemeinderEmailSent)
                .ToList();

            Console.WriteLine($"{tomorrowAppointments.Count} email(s) à envoyer");

            var resourceManager = new ResourceManager(typeof(Resources.ResourceServerRunner));

            foreach (var appointment in tomorrowAppointments)
            {

                var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

                string emailTitle = string.Format(resourceManager.GetString("AppointmentReminder_Subject", culture),
                    appointment.StartDateTime.ToString("dd MMMM"),
                    appointment.StartDateTime.ToString("HH:mm"));

                string emailContent = "";


                var paymentUrl = $"{ConfigurationManager.AppSettings["websiteUrl"]}{resourceManager.GetString("AppointmentReminder_PaymentLink", culture)}{appointment.PaymentUrlCode}";
                var paymenyLink = $"<a href='{paymentUrl}'>{paymentUrl}</a>";

                if (appointment.ConsultationTypeId == 1) //In person
                {
                    var therapistInfo = new GetTherapistInfo(_context, appointment.TherapistId);
                    therapistInfo.GetMostRecentTherapistInfo();

                    emailContent = string.Format(resourceManager.GetString("AppointmentReminder_Content_InPerson", culture),
                        appointment.StartDateTime.ToString("dd MMMM"),
                        appointment.StartDateTime.ToString("HH:mm"),
                        $"{therapistInfo.Adress}, {therapistInfo.City}",
                        paymenyLink);

                }
                else if (appointment.ConsultationTypeId == 2) //Online
                {

                    emailContent = string.Format(resourceManager.GetString("AppointmentReminder_Content_Online", culture),
                       appointment.StartDateTime.ToString("dd MMMM"),
                       appointment.StartDateTime.ToString("HH:mm"),
                       paymenyLink);
                }

                var clientInfos = new GetClientInfos(_context, appointment.ClientId);
                clientInfos.GetMostRecentClientInfo();

                EmailHelper.Send(emailTitle, emailContent, clientInfos.Email, "");

                appointment.IsRemeinderEmailSent = true;

                if (appointment.ClientBillGeneratedDate == null)
                    appointment.ClientBillGeneratedDate = DateTime.Now;

                if (appointment.TotalDollarAmount == null)
                {
                    GetClaimableAmountForAppointment getClaimableAmountForAppointment = new GetClaimableAmountForAppointment(_context);
                    getClaimableAmountForAppointment.CalculateAppointmentPayment(appointment.Id);

                    appointment.TotalDollarAmount = getClaimableAmountForAppointment.ClaimableAmount;
                }
            }

            _context.SaveChanges();

            Console.WriteLine("Les courriels ont été envoyé avec succès");
        }

        private static void DeleteTherapistClientRequestExpired()
        {
            Console.WriteLine();
            Console.WriteLine("Début des renvoie des rendez-vous à l'admin");

            var _context = new ReseauPsyEntities();

            var maxSentDate = DateTime.Now.AddDays(-7);


            var therapistClientRequestExpired = _context.TherapistClientRequest
                .Where(x => x.RequestSentDate <= maxSentDate && x.IsAccepted == null)
                .ToList();

            Console.WriteLine($"{therapistClientRequestExpired.Count} rendez-vous à renvoyer à l'admin");

            foreach (var request in therapistClientRequestExpired)
            {
                request.IsAccepted = false;
            }

            _context.SaveChanges();

            Console.WriteLine("Les rendez-vous ont été renvoyé à l'administration");
            Console.WriteLine();
        }

    }
}
