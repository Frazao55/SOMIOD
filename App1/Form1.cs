using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace App1
{
    public partial class Form1 : Form
    {
        private const string SOMIOD_URL = "http://localhost:50202/api/somiod";
        private const string HTTP_SERVER_URL = "http://127.0.0.1:8080/";
        private const string MQTT_BROKER_URL = "127.0.0.1";
        private MqttClient mqttClient;
        private HttpListener httpListener;

        // Images
        readonly Image lowIntensityImage;
        readonly Image mediumIntensityImage;
        readonly Image highIntensityImage;
        readonly Image stoppedImage;

        // Fan initial status
        private string PowerState = "OFF";
        private string Intensity = "LOW";

        // Define the features of the fan and how we want to receive notifications (via HTTP or MQTT)
        private readonly Dictionary<string, string> features = new Dictionary<string, string>()
        {
            { "PowerState", "MQTT" },
            { "Intensity", "HTTP" }
        };

        public Form1()
        {
            InitializeComponent();

            // Start HTTP server
            StartHttpServer();

            // Connect to MQTT broker
            ConnectMqtt();

            // Load images
            lowIntensityImage = Properties.Resources.fan_low_intensity;
            mediumIntensityImage = Properties.Resources.fan_medium_intensity;
            highIntensityImage = Properties.Resources.fan_high_intensity;
            stoppedImage = Properties.Resources.fan_stopped;
        }

        async private void Form1_Load(object sender, EventArgs e)
        {
            // Register application, container and subscription
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    //Ponto 01
                    string xmlContent = "<resource res_type='application'><name>Ventilation</name></resource>";
                    StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await httpClient.PostAsync(SOMIOD_URL, stringContent);

                    if (!response.IsSuccessStatusCode)
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nAplicação já criada !", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //Ponto 02
                    xmlContent = "<resource res_type='container'><name>Fan</name></resource>";
                    stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                    response = await httpClient.PostAsync(SOMIOD_URL + "/Ventilation", stringContent);

                    if (!response.IsSuccessStatusCode)
                       MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nContainer já criado !", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Ponto 03
                    foreach (KeyValuePair<string, string> feature in features)
                    {
                        string endpoint = feature.Value == "MQTT" ? "mqtt://" + MQTT_BROKER_URL : HTTP_SERVER_URL;
                        xmlContent = $"<resource res_type='subscription'><name>sub{feature.Key}</name><feature>{feature.Key}</feature><event>1</event><endpoint>{endpoint}</endpoint></resource>";
                        stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

                        response = await httpClient.PostAsync(SOMIOD_URL + "/Ventilation/Fan", stringContent);
                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nSubscrição já criada !", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Console.WriteLine($"Subscription on {feature} topic was created successfully!");
                        }
                    }

                    // Ponto 04
                    SubscribeMqtt();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string xmlMessage = Encoding.UTF8.GetString(e.Message);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlMessage);

            string feature = doc.SelectSingleNode("/notification/resource/content/feature").InnerText;
            string value = doc.SelectSingleNode("notification/resource/content/value").InnerText;
            string @event = doc.SelectSingleNode("notification/event").InnerText;

            Console.WriteLine($"Received MQTT message on feature {feature} with value {value} on topic {e.Topic} triggered by event {(@event == "1" ? "creation" : "deletion")}");

            if (@event == "2") return; // Ignore deletion events

            UpdateStatus(feature, value);
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            // Validate request
            if (context.Request.Url.AbsolutePath.ToLower() != "/ventilation/fan")
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
                return;
            }

            // Get body content
            string bodyContent = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(bodyContent);

            string feature = doc.SelectSingleNode("/notification/resource/content/feature").InnerText;
            string value = doc.SelectSingleNode("notification/resource/content/value").InnerText;
            string @event = doc.SelectSingleNode("notification/event").InnerText;

            Console.WriteLine($"Received HTTP request from {context.Request.RemoteEndPoint.Address}:{context.Request.RemoteEndPoint.Port} on feature {feature} with value {value} triggered by event {(@event == "1" ? "creation" : "deletion")}");

            // Send response
            context.Response.StatusCode = 200;
            context.Response.Close();

            if (@event == "2") return; // Ignore deletion events

            // Update status
            UpdateStatus(feature, value);
        }

        private void UpdateStatus(string feature, string value)
        {
            Color color;
            switch (feature)
            {
                case "PowerState":
                    color = value == "ON" ? Color.Green : Color.DarkRed;
                    PowerState = value;
                    UpdateLabelsAndPictureBox(labelPowerState, value.ToUpper(), color);
                    break;
                case "Intensity":
                    if (value.ToUpper() == "LOW" || value.ToUpper() == "MEDIUM" || value.ToUpper() == "HIGH")
                    {
                        color = value == "LOW" ? Color.LightGreen :
                                value == "MEDIUM" ? Color.Orange :
                                value == "HIGH" ? Color.DarkRed :
                                labelIntensity.ForeColor;

                        Intensity = value;
                        UpdateLabelsAndPictureBox(labelIntensity, value.ToUpper(), color);
                    }
                    break;
                default:
                    break;
            }
        }

        private void UpdateLabelsAndPictureBox(Label label, string text, Color color)
        {
            if (label.InvokeRequired)
            {
                label.Invoke((MethodInvoker)(() => {
                    label.Text = text;
                    label.ForeColor = color;
                }));
            }
            else
            {
                label.Text = text;
                label.ForeColor = color;
            }

            Image image = null;
            if (PowerState == "ON")
            {
                switch (Intensity)
                {
                    case "LOW":
                        image = lowIntensityImage;
                        break;
                    case "MEDIUM":
                        image = mediumIntensityImage;
                        break;
                    case "HIGH":
                        image = highIntensityImage;
                        break;
                    default:
                        image = lowIntensityImage;
                        break;
                }
            }
            else
            {
                image = stoppedImage;
            }

            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke((MethodInvoker)(() =>
                {
                    pictureBox1.Image = image;
                    pictureBox1.Refresh();
                }));
            }
            else
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }

        // Ponto 04 - Subscrição MQTT
        private void SubscribeMqtt()
        {
            try
            {
                string[] topic = { "api/somiod/Ventilation/Fan" };
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };

                mqttClient.Subscribe(topic, qosLevels);
                Console.WriteLine($"MQTT subscription on api/somiod/ventilation/fan topic was created successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error subscribing topic MQTT - " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartHttpServer()
        {
            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(HTTP_SERVER_URL);

                httpListener.Start();
                Console.WriteLine("HTTP server started...");

                Task.Run(() => HandleHttpRequests());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting HTTP server - " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectMqtt()
        {
            try
            {
                mqttClient = new MqttClient(MQTT_BROKER_URL);
                mqttClient.Connect(Guid.NewGuid().ToString());

                if (!mqttClient.IsConnected)
                {
                    MessageBox.Show("Error connecting to message broker...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                mqttClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                Console.WriteLine("Connected to MQTT successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to MQTT - " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleHttpRequests()
        {
            try
            {
                while (httpListener.IsListening)
                {
                    HttpListenerContext context = httpListener.GetContext();
                    ProcessRequest(context);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling HTTP requests - " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopHttpServer()
        {
            try
            {
                if (httpListener != null && httpListener.IsListening)
                {
                    httpListener.Stop();
                    Console.WriteLine("HTTP server stopped...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error stopping HTTP server - " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisconnectMqtt()
        {
            try
            {
                if (mqttClient != null && mqttClient.IsConnected)
                {
                    mqttClient.Disconnect();
                    Console.WriteLine("Disconnected from MQTT...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error disconnecting from MQTT - " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // On Close
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisconnectMqtt();
            StopHttpServer();
        }
    }
}