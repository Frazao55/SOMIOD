using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App2
{
    public partial class Form1 : Form
    {
        const string URL = "http://localhost:50202/api/somiod/";
        public Form1()
        {
            InitializeComponent();
        }

       async private void Form1_Load(object sender, EventArgs e)
       {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string xmlContent = "<resource res_type='application'><name>FanControl</name></resource>";
                    StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await httpClient.PostAsync(URL, stringContent);

                    if (!response.IsSuccessStatusCode)
                        MessageBox.Show($"Erro ao criar recurso - {response.StatusCode}!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
       }

        async private void changeFeatureState(string feature, string value)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string xmlContent = "<resource res_type='data'>" +
                                            //"<name></name>" +
                                            $"<content>" +
                                                $"<feature>{feature}</feature>" +
                                                $"<value>{value}</value>" +
                                            $"</content>" +
                                        "</resource>";

                    StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await httpClient.PostAsync(URL + "/Ventilation/Fan", stringContent);

                    if (!response.IsSuccessStatusCode)
                        MessageBox.Show($"Request error - {response.StatusCode}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show($"{feature} changed to {value}!", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        async private void btnFanOn_Click(object sender, EventArgs e)
        {
            changeFeatureState("PowerState", "ON");
        }

        async private void btnFanOff_Click(object sender, EventArgs e)
        {
            changeFeatureState("PowerState", "OFF");
        }

        private void buttonLowIntensity_Click(object sender, EventArgs e)
        {
            changeFeatureState("Intensity", "LOW");
        }

        private void buttonMediumIntensity_Click(object sender, EventArgs e)
        {
            changeFeatureState("Intensity", "MEDIUM");
        }

        private void buttonHighIntensity_Click(object sender, EventArgs e)
        {
            changeFeatureState("Intensity", "HIGH");
        }
    } 
}