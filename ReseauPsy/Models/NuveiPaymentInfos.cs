using Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ReseauPsy.Models
{
    public enum PaymentType
    {
        PAYMENT,
        PREAUTH,
        PREAUTHCOMPLETION,
        REFUND
    }
    public enum Currencies
    {
        CAD
    }

    public class Nuvei
    {
        protected string TerminalCurrency { get; set; }
        protected string AmountStr { get; set; }
        protected int OrderId { get; set; }
        protected string Date { get; set; }
        protected string CardNumber { get; set; }
        protected string CardType { get; set; }
        protected string CardExpiration { get; set; }
        protected string CardHolderName { get; set; }
        protected string CardCvv { get; set; }
        protected string TerminalId { get; set; }
        protected string NuveiSecret { get; set; }
        protected string UniqueRef { get; set; }
        protected string XmlStr { get; set; }
        protected ReseauPsyEntities _context { get; set; }
        
        protected Nuvei(
            decimal transactionAmount,
            DateTime transactionDateTime,
            ReseauPsyEntities _context)
        {
            this.AmountStr = transactionAmount.ToString().Replace(",", ".");
            this.Date = transactionDateTime.ToString("dd-MM-yyyy:HH:mm:ss:fff");
            this.TerminalId = ConfigurationManager.AppSettings["terminalId"];
            this.NuveiSecret = ConfigurationManager.AppSettings["sharedSecret"];
            this._context = _context;
            this.OrderId = GetLastOrderId() + 1;
        }



        /// <summary>
        /// Get the last orderId in the database
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        private int GetLastOrderId()
        {
            int? lastOrderId = _context.ClientPaymentInfos
                .OrderByDescending(x => x.OrderId)
                .Select(x => x.OrderId)
                .FirstOrDefault();

            return lastOrderId == null ? 0 : lastOrderId.Value;
        }
        

        public bool UrlPayment(ClientAppointments clientAppointment)
        {

            #region Send the payment

            // Envoie la requète a "nuvei"
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://testpayments.nuvei.com/merchant/xmlpayment"); // Créer la requête avec l'URL utiliser
            byte[] bytes = Encoding.UTF8.GetBytes(this.XmlStr);
            webRequest.ContentType = "text/xml; encoding=\"utf-8\"";
            webRequest.ContentLength = bytes.Length;
            webRequest.Method = "POST";
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();



            #endregion

            #region Read response

            // Lie la reponse
            XElement responseXmlElement = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseXmlElement = XDocument.Parse(new StreamReader(responseStream).ReadToEnd()).Root;
            }


            #endregion

            #region Save response in database

            //Save in the database
            var clientPaymentInfos = new ClientPaymentInfo();
            var xmlNoseList = responseXmlElement.Elements().ToList();
            clientPaymentInfos.ClientAppointmentId = clientAppointment.Id;
            clientPaymentInfos.OrderId = this.OrderId;
            clientPaymentInfos.TransactionDateTime = DateTime.Now;
            clientPaymentInfos.ResponseBody = responseXmlElement.Name.ToString();

            //True by default, change if error later
            clientPaymentInfos.DidSucceed = true;
            bool didPaymentPassed = true;

            foreach (var item in xmlNoseList)
            {
                if (item.Name == "UNIQUEREF")
                    clientPaymentInfos.UniqueRef = item.Value;

                else if (item.Name == "RESPONSECODE")
                    clientPaymentInfos.ResponseCode = item.Value;

                else if (item.Name == "RESPONSETEXT")
                    clientPaymentInfos.ResponseText = item.Value;

                else if (item.Name == "APPROVALCODE")
                    clientPaymentInfos.ApprovalCode = item.Value;

                else if (item.Name == "AVSRESPONSE")
                    clientPaymentInfos.AvsResponse = item.Value;

                else if (item.Name == "CVVRESPONSE")
                    clientPaymentInfos.CvvResponse = item.Value;

                else if (item.Name == "ERROR" || item.Name == "ERRORSTRING")
                {
                    didPaymentPassed = false;
                    clientPaymentInfos.DidSucceed = false;
                    clientPaymentInfos.ResponseText = item.Value;
                }
            }

            if (didPaymentPassed)
            { 
                clientAppointment.ClientPaymentDate = DateTime.Now;

                GetNextClientPaymentReceiptNumber getNextClientPaymentReceipt = new GetNextClientPaymentReceiptNumber(_context);
                clientAppointment.ClientPaymentReceiptNumber = getNextClientPaymentReceipt.Number;
            }

            _context.ClientPaymentInfos.Add(clientPaymentInfos);
            _context.SaveChanges();

            #endregion

            return didPaymentPassed;
        }
    }



    public class NuveiPayment : Nuvei
    {
        /// <summary>
        /// Set all the informations for the Nuvei payment or preauth
        /// </summary>
        /// <param name="terminalCurrency">Currency for the payment, use Currencies enum</param>
        /// <param name="cardNumber">The card number with or without spaces. Accept Visa and mastercard as for 2021-11-09</param>
        /// <param name="cardExpiration">Card expiration MMYY or MM/YY</param>
        /// <param name="transactionDateTime">When the payment need to be proccess</param>
        /// <param name="transactionAmount">The amount for the payment</param>
        /// <param name="orderId">Unique order id for the transaction</param>
        /// <param name="cardHolderName">The name on the card</param>
        /// <param name="cardCvv">Cvv behind the card 3 or 4 digit</param>
        /// <param name="paymentType">Use PaymentType enum, payment or preauth</param>
        public NuveiPayment(
            Currencies terminalCurrency,
            string cardNumber,
            string cardExpiration,
            DateTime transactionDateTime,
            decimal transactionAmount,
            string cardHolderName,
            string cardCvv,
            ReseauPsyEntities _context)
            : base(transactionAmount, transactionDateTime, _context)
        {
            var cardHelper = new CardNumberTypeRegex();

            this.TerminalCurrency = terminalCurrency.ToString();
            this.CardNumber = cardNumber.Trim();
            this.CardType = cardHelper.GetCardTypeFromCardNumber(this.CardNumber);
            this.CardExpiration = cardExpiration.Replace("/", "");
            this.CardHolderName = cardHolderName;
            this.CardCvv = cardCvv;

            GenerateXml();
        }

        private string GenerateTextHash()
        {
            string textForHash = string.Format("{0}:{1}:{2}:{3}:{4}",
                this.TerminalId,
                this.OrderId,
                this.AmountStr,
                this.Date,
                this.NuveiSecret);

            string textHash;

            using (SHA512 hash = SHA512Managed.Create())
            {
                textHash = String.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(textForHash))
                    .Select(item => item.ToString("x2"))).ToLower();
            }

            return textHash;
        }
        private void GenerateXml()
        {
            // Créer la liste des nodes du xml avec les donnée inclus
            List<RequestNodeChild> lstRequestNodeChild = new List<RequestNodeChild>
            {
                new RequestNodeChild { nodeChildName = "ORDERID", nodeChildData = this.OrderId.ToString() },
                new RequestNodeChild { nodeChildName = "TERMINALID", nodeChildData = this.TerminalId },
                new RequestNodeChild { nodeChildName = "AMOUNT", nodeChildData = this.AmountStr },
                new RequestNodeChild { nodeChildName = "DATETIME", nodeChildData = this.Date },
                new RequestNodeChild { nodeChildName = "CARDNUMBER", nodeChildData = this.CardNumber },
                new RequestNodeChild { nodeChildName = "CARDTYPE", nodeChildData = this.CardType },
                new RequestNodeChild { nodeChildName = "CARDEXPIRY", nodeChildData = this.CardExpiration },
                new RequestNodeChild { nodeChildName = "CARDHOLDERNAME", nodeChildData = this.CardHolderName },
                new RequestNodeChild { nodeChildName = "HASH", nodeChildData = GenerateTextHash() },
                new RequestNodeChild { nodeChildName = "CURRENCY", nodeChildData = this.TerminalCurrency },
                new RequestNodeChild { nodeChildName = "TERMINALTYPE", nodeChildData = "2" },
                new RequestNodeChild { nodeChildName = "TRANSACTIONTYPE", nodeChildData = "7" },
                new RequestNodeChild { nodeChildName = "CVV", nodeChildData = CardCvv }
            };


            // Crée le xml
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode paymentNode = doc.CreateElement("PAYMENT");
            doc.AppendChild(paymentNode);

            XmlNode paymentChildNode;
            lstRequestNodeChild.ForEach(node =>
            {
                paymentChildNode = doc.CreateElement(node.nodeChildName);
                paymentChildNode.AppendChild(doc.CreateTextNode(node.nodeChildData));
                paymentNode.AppendChild(paymentChildNode);
            });

            this.XmlStr = doc.OuterXml;
        }
    }

    public class NuveiPreAuth : Nuvei
    {

        /// <summary>
        /// Set all the informations for the Nuvei payment or preauth
        /// </summary>
        /// <param name="terminalCurrency">Currency for the payment, use Currencies enum</param>
        /// <param name="cardNumber">The card number with or without spaces. Accept Visa and mastercard as for 2021-11-09</param>
        /// <param name="cardExpiration">Card expiration MMYY or MM/YY</param>
        /// <param name="transactionDateTime">When the payment need to be proccess</param>
        /// <param name="transactionAmount">The amount for the payment</param>
        /// <param name="orderId">Unique order id for the transaction</param>
        /// <param name="cardHolderName">The name on the card</param>
        /// <param name="cardCvv">Cvv behind the card 3 or 4 digit</param>
        /// <param name="paymentType">Use PaymentType enum, payment or preauth</param>
        public NuveiPreAuth(
            Currencies terminalCurrency,
            string cardNumber,
            string cardExpiration,
            DateTime transactionDateTime,
            decimal transactionAmount,
            string cardHolderName,
            string cardCvv,
            ReseauPsyEntities _context)
            : base(transactionAmount, transactionDateTime, _context)
        {
            var cardHelper = new CardNumberTypeRegex();

            this.TerminalCurrency = terminalCurrency.ToString();
            this.CardNumber = cardNumber.Trim();
            this.CardType = cardHelper.GetCardTypeFromCardNumber(this.CardNumber);
            this.CardExpiration = cardExpiration.Replace("/", "");
            this.CardHolderName = cardHolderName;
            this.CardCvv = cardCvv;

            GenerateXml();
        }
        private string GenerateTextHash()
        {
            string textForHash = string.Format("{0}:{1}:{2}:{3}:{4}",
                    this.TerminalId,
                    this.OrderId,
                    this.AmountStr,
                    this.Date,
                    this.NuveiSecret);

            string textHash;

            using (SHA512 hash = SHA512Managed.Create())
            {
                textHash = String.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(textForHash))
                    .Select(item => item.ToString("x2"))).ToLower();
            }

            return textHash;
        }
        private void GenerateXml()
        {
            // Créer la liste des nodes du xml avec les donnée inclus
            List<RequestNodeChild> lstRequestNodeChild = new List<RequestNodeChild>
            {
                new RequestNodeChild { nodeChildName = "ORDERID", nodeChildData = this.OrderId.ToString() },
                new RequestNodeChild { nodeChildName = "TERMINALID", nodeChildData = this.TerminalId },
                new RequestNodeChild { nodeChildName = "AMOUNT", nodeChildData = this.AmountStr },
                new RequestNodeChild { nodeChildName = "DATETIME", nodeChildData = this.Date },
                new RequestNodeChild { nodeChildName = "CARDNUMBER", nodeChildData = this.CardNumber },
                new RequestNodeChild { nodeChildName = "CARDTYPE", nodeChildData = this.CardType },
                new RequestNodeChild { nodeChildName = "CARDEXPIRY", nodeChildData = this.CardExpiration },
                new RequestNodeChild { nodeChildName = "CARDHOLDERNAME", nodeChildData = this.CardHolderName },
                new RequestNodeChild { nodeChildName = "HASH", nodeChildData = GenerateTextHash() },
                new RequestNodeChild { nodeChildName = "CURRENCY", nodeChildData = this.TerminalCurrency },
                new RequestNodeChild { nodeChildName = "TERMINALTYPE", nodeChildData = "2" },
                new RequestNodeChild { nodeChildName = "TRANSACTIONTYPE", nodeChildData = "7" },
                new RequestNodeChild { nodeChildName = "CVV", nodeChildData = CardCvv }
            };


            // Crée le xml
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode paymentNode = doc.CreateElement("PREAUTH");
            doc.AppendChild(paymentNode);

            XmlNode paymentChildNode;
            lstRequestNodeChild.ForEach(node =>
            {
                paymentChildNode = doc.CreateElement(node.nodeChildName);
                paymentChildNode.AppendChild(doc.CreateTextNode(node.nodeChildData));
                paymentNode.AppendChild(paymentChildNode);
            });

            this.XmlStr = doc.OuterXml;
        }

    }
    public class NuveiPreAuthCompletion : Nuvei
    {

        /// <summary>
        /// Set all the information for Nuvei PreAuthCompletion
        /// </summary>
        /// <param name="transactionAmount">The amount of the transaction</param>
        /// <param name="transactionDateTime">The transaction time mostly datetime now</param>
        /// <param name="uniqueRef">The unique ref of the prepayment. Located in the database</param>
        /// <param name="paymentType">Keep it at preauthcompletion</param>
        public NuveiPreAuthCompletion(
            decimal transactionAmount,
            string uniqueRef,
            DateTime transactionDateTime,
            ReseauPsyEntities _context)
            : base(transactionAmount, transactionDateTime, _context)
        {
            this.UniqueRef = uniqueRef;
            GenerateXml();
        }
        private string GenerateTextHash()
        {
            string textForHash = string.Format("{0}:{1}:{2}:{3}:{4}",
                    this.TerminalId,
                    this.UniqueRef,
                    this.AmountStr,
                    this.Date,
                    this.NuveiSecret);

            string textHash;

            using (SHA512 hash = SHA512Managed.Create())
            {
                textHash = String.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(textForHash))
                    .Select(item => item.ToString("x2"))).ToLower();
            }

            return textHash;
        }
        private void GenerateXml()
        {
            // Créer la liste des nodes du xml avec les donnée inclus
            List<RequestNodeChild> lstRequestNodeChild = new List<RequestNodeChild>
            {
                new RequestNodeChild { nodeChildName = "UNIQUEREF", nodeChildData = this.UniqueRef },
                new RequestNodeChild { nodeChildName = "TERMINALID", nodeChildData = this.TerminalId },
                new RequestNodeChild { nodeChildName = "AMOUNT", nodeChildData = this.AmountStr },
                new RequestNodeChild { nodeChildName = "DATETIME", nodeChildData = this.Date },
                new RequestNodeChild { nodeChildName = "HASH", nodeChildData = GenerateTextHash() },
            };


            // Crée le xml
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode paymentNode = doc.CreateElement("PREAUTHCOMPLETION");
            doc.AppendChild(paymentNode);

            XmlNode paymentChildNode;
            lstRequestNodeChild.ForEach(node =>
            {
                paymentChildNode = doc.CreateElement(node.nodeChildName);
                paymentChildNode.AppendChild(doc.CreateTextNode(node.nodeChildData));
                paymentNode.AppendChild(paymentChildNode);
            });

            this.XmlStr = doc.OuterXml;
        }
    }
    public class NuveiRefund : Nuvei
    {

        /// <summary>
        /// Set the information for the Refund operation
        /// </summary>
        /// <param name="transactionAmount"></param>
        /// <param name="uniqueRef"></param>
        /// <param name="transactionDateTime"></param>
        /// <param name="_context"></param>
        /// <param name="operatorName"></param>
        /// <param name="reason"></param>
        /// <param name="paymentType"></param>
        public NuveiRefund(
            string uniqueRef,
            DateTime transactionDateTime,
            ReseauPsyEntities _context,
            string operatorName,
            string reason,
            decimal transactionAmount)
            : base(transactionAmount, transactionDateTime, _context)
        {
            this.UniqueRef = uniqueRef;
            GenerateXml(operatorName, reason);
        }

        private string GenerateTextHash()
        {
            string textForHash = string.Format("{0}:{1}:{2}:{3}:{4}",
                this.TerminalId,
                this.UniqueRef,
                this.AmountStr,
                this.Date,
                this.NuveiSecret);

            string textHash;

            using (SHA512 hash = SHA512Managed.Create())
            {
                textHash = String.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(textForHash))
                    .Select(item => item.ToString("x2"))).ToLower();
            }

            return textHash;
        }
        private void GenerateXml(string therapistName, string refundReason)
        {

            // Créer la liste des nodes du xml avec les donnée inclus
            List<RequestNodeChild> lstRequestNodeChild = new List<RequestNodeChild>
            {
                new RequestNodeChild { nodeChildName = "UNIQUEREF", nodeChildData = this.UniqueRef },
                new RequestNodeChild { nodeChildName = "TERMINALID", nodeChildData = this.TerminalId },
                new RequestNodeChild { nodeChildName = "AMOUNT", nodeChildData = this.AmountStr },
                new RequestNodeChild { nodeChildName = "DATETIME", nodeChildData = this.Date },
                new RequestNodeChild { nodeChildName = "HASH", nodeChildData = GenerateTextHash() },
                new RequestNodeChild { nodeChildName = "OPERATOR", nodeChildData = therapistName },
                new RequestNodeChild { nodeChildName = "REASON", nodeChildData = refundReason },
            };


            // Crée le xml
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode paymentNode = doc.CreateElement("REFUND");
            doc.AppendChild(paymentNode);

            XmlNode paymentChildNode;
            lstRequestNodeChild.ForEach(node =>
            {
                paymentChildNode = doc.CreateElement(node.nodeChildName);
                paymentChildNode.AppendChild(doc.CreateTextNode(node.nodeChildData));
                paymentNode.AppendChild(paymentChildNode);
            });

            this.XmlStr = doc.OuterXml;

        }
    }
}