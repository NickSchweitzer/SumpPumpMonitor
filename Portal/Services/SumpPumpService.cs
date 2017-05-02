using System;
using Newtonsoft.Json;

using CodingMonkeyNet.SumpPumpMonitor.IoT.Messages;
using CodingMonkeyNet.SumpPumpMonitor.IoT;
using System.Text;
using Amqp;
using Amqp.Framing;
using System.Security.Cryptography;
using System.Net;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Services
{
    public class SumpPumpService : IIoTHubSender<SumpPumpSettings>
    {
        private const int Port = 5671;
        private static readonly long UtcReference = (new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks;
        private readonly IoTHubConfiguration Configuration;

        public SumpPumpService(IoTHubConfiguration config)
        {
            Configuration = config;
        }

        public void SendMessage(string deviceId, SumpPumpSettings message)
        {
            Address address = new Address(Configuration.HostName, Port, null, null);
            Connection connection = new Connection(address);
            Session session = new Session(connection);

            string uri = Fx.Format("{0}/messages/devicebound", Configuration.HostName);
            string token = GetSharedAccessSignature(Configuration.SharedAccessKeyName, Configuration.SharedAccessKey, uri, new TimeSpan(1, 0, 0));
            bool cbs = PutToken(connection, Configuration.HostName, token, uri);

            if (cbs)
            {
                string toAddress = Fx.Format("/devices/{0}/messages/devicebound", deviceId);
                SenderLink senderLink = new SenderLink(session, "sender-link", "/messages/devicebound");

                var testMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                Message amqpMessage = new Message()
                {
                    BodySection = new Amqp.Framing.Data() { Binary = testMessage }
                };
                amqpMessage.Properties = new Properties()
                {
                    To = toAddress,
                    MessageId = Guid.NewGuid().ToString()
                };
                amqpMessage.ApplicationProperties = new ApplicationProperties();
                amqpMessage.ApplicationProperties["iothub-ack"] = "full";

                senderLink.Send(amqpMessage);
                senderLink.Close();
            }
        }

        private static bool PutToken(Connection conn, string host, string accessSignature, string audience)
        {
            bool result = true;
            Session session = new Session(conn);

            string cbsReplyToAddress = "cbs-reply-to";
            var cbsSender = new SenderLink(session, "cbs-sender", "$cbs");
            var cbsReceiver = new ReceiverLink(session, cbsReplyToAddress, "$cbs");

            // construct the put-token message
            var request = new Message(accessSignature);
            request.Properties = new Properties();
            request.Properties.MessageId = Guid.NewGuid().ToString();
            request.Properties.ReplyTo = cbsReplyToAddress;
            request.ApplicationProperties = new ApplicationProperties();
            request.ApplicationProperties["operation"] = "put-token";
            request.ApplicationProperties["type"] = "azure-devices.net:sastoken";
            request.ApplicationProperties["name"] = audience;
            cbsSender.Send(request);

            // receive the response
            var response = cbsReceiver.Receive();
            if (response == null || response.Properties == null || response.ApplicationProperties == null)
                result = false;
            else
            {
                int statusCode = (int)response.ApplicationProperties["status-code"];
                string statusCodeDescription = (string)response.ApplicationProperties["status-description"];
                if (statusCode != (int)202 && statusCode != (int)200) // !Accepted && !OK
                    result = false;
            }

            // the sender/receiver may be kept open for refreshing tokens
            cbsSender.Close();
            cbsReceiver.Close();
            session.Close();

            return result;
        }

        private static string GetSharedAccessSignature(string keyName, string sharedAccessKey, string resource, TimeSpan tokenTimeToLive)
        {
            string expiry = ((long)(DateTime.UtcNow - new DateTime(UtcReference, DateTimeKind.Utc) + tokenTimeToLive).TotalSeconds()).ToString();
            string encodedUri = WebUtility.UrlEncode(resource);

            HMACSHA256 sha = new HMACSHA256(Convert.FromBase64String(sharedAccessKey));
            byte[] hmac = sha.ComputeHash(Encoding.UTF8.GetBytes(encodedUri + "\n" + expiry));

            string sig = Convert.ToBase64String(hmac);

            if (keyName != null)
            {
                return Fx.Format("SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                encodedUri,
                WebUtility.UrlEncode(sig),
                WebUtility.UrlEncode(expiry),
                WebUtility.UrlEncode(keyName));
            }
            else
            {
                return Fx.Format("SharedAccessSignature sr={0}&sig={1}&se={2}",
                    encodedUri,
                    WebUtility.UrlEncode(sig),
                    WebUtility.UrlEncode(expiry));
            }
        }
    }
}