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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppEvaluation
{
    public partial class AppEvaluation : Form
    {
        private static string URL = "http://localhost:50202/api/somiod";

        private string URL_APP = "";
        private string URL_CONTAINER = "";
        private string data_cache= "";

        private string[] logs;
        private string[] edit_cache = new string[4];
        private bool flag_edit_sub = false;
        private bool flag_delete = false;

        public AppEvaluation()
        {
            InitializeComponent();
        }

        async private void AppEvaluation_Load(object sender, EventArgs e)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);
                request.Headers.Add("somiod-discover", "application");
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro ao ir buscar as aplicações");
                }

                string content = await response.Content.ReadAsStringAsync();
                List<string> applicationNames = JsonConvert.DeserializeObject<List<string>>(content);
                listBoxApplications.Items.AddRange(applicationNames.ToArray());
            }

        }

        /* 
         * When an application is selected, the containers of that application are shown in the listBoxContainers and
         * the information for that application is shown in the textBoxInformation
         */
        async private void listBoxApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex != -1)
            {
                string selectedItem = listBox.SelectedItem.ToString();

                using (HttpClient httpClient = new HttpClient())
                {
                    URL_APP = URL + "/" + selectedItem;
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL_APP);
                    request.Headers.Add("somiod-discover", "container");
                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro ao ir buscar os Containers");
                    }

                    string content = await response.Content.ReadAsStringAsync();
                    List<string> valores = JsonConvert.DeserializeObject<List<string>>(content);
                    listBoxContainers.Items.Clear();
                    listBoxContainers.Items.AddRange(valores.ToArray());
                    listBoxSubscriptions.Items.Clear();
                    listBoxData.Items.Clear();
                }

                //For Information textBox
                textBoxInformation.Text = "";
                logs = new string[4];
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL_APP);
                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro no logs para a app");
                        return;
                    }

                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(content);
                    string linha = "Application: " + jsonObject["Id"].ToString() + "     " + jsonObject["Name"].ToString() + "     " + jsonObject["CreationDT"].ToString() + Environment.NewLine + Environment.NewLine;
                    logs[0] = linha;
                    textBoxInformation.Text += logs[0];
                }
            }
        }

        /* 
         * When a container is selected, the subscriptions and data of that container are shown in the listBoxSubscriptions and listBoxData, respectively.
         * And the information for that container is shown in the textBoxInformation
         */
        async private void listBoxContainers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex != -1)
            {
                string selectedItem = listBox.SelectedItem.ToString();

                using (HttpClient httpClient = new HttpClient())
                {
                    URL_CONTAINER = URL_APP + "/" + selectedItem;
                    HttpRequestMessage request01 = new HttpRequestMessage(HttpMethod.Get, URL_CONTAINER);
                    request01.Headers.Add("somiod-discover", "subscription");
                    HttpResponseMessage response01 = await httpClient.SendAsync(request01);

                    if (!response01.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response01.StatusCode}\nErro ao ir buscar as Subscriptions");
                    }

                    HttpRequestMessage request02 = new HttpRequestMessage(HttpMethod.Get, URL_CONTAINER);
                    request02.Headers.Add("somiod-discover", "data");
                    HttpResponseMessage response02 = await httpClient.SendAsync(request02);

                    if (!response02.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response02.StatusCode}\nErro ao ir buscar os Data");
                    }

                    string content = await response01.Content.ReadAsStringAsync();
                    List<string> valores01 = JsonConvert.DeserializeObject<List<string>>(content);
                    content = await response02.Content.ReadAsStringAsync();
                    List<string> valores02 = JsonConvert.DeserializeObject<List<string>>(content);

                    listBoxSubscriptions.Items.Clear();
                    listBoxData.Items.Clear();
                    listBoxSubscriptions.Items.AddRange(valores01.ToArray());
                    listBoxData.Items.AddRange(valores02.ToArray());
                }

                //For Information textBox
                textBoxInformation.Text = "";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL_CONTAINER);
                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro no logs para o container");
                    }

                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(content);
                    string linha = "\tContainer: " + jsonObject["Id"].ToString() + "     " + jsonObject["Name"].ToString() + "     " + jsonObject["Parent"].ToString() + "     " + jsonObject["CreationDT"].ToString() + Environment.NewLine + Environment.NewLine;
                    logs[1] = linha;
                    logs[2] = logs[3]= "";
                    textBoxInformation.Text = logs[0]+logs[1];
                }
            }
        }


        /* 
         * When a subscription is selected, the information for that subscription is shown in the textBoxInformation
         */
        async private void listBoxSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex != -1)
            {
                //For Information textBox
                textBoxInformation.Text = "";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL_CONTAINER + "/sub/" + listBoxSubscriptions.SelectedItem.ToString());
                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro no logs para a subscription");
                    }

                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(content);
                    string feature = jsonObject["Feature"].ToString() == "" ? "null" : jsonObject["Feature"].ToString();
                    string linha = "\t\tSubscription: " + jsonObject["Id"].ToString() + "     " + jsonObject["Name"].ToString() + "     " + jsonObject["Parent"].ToString() + "     " + feature + "     " + jsonObject["Event"].ToString() + "     " + jsonObject["Endpoint"].ToString() + "     " + jsonObject["CreationDT"].ToString() + Environment.NewLine + Environment.NewLine;
                    logs[2] = linha;
                    string existe = logs[3] == "" ? "": logs[3];
                    textBoxInformation.Text = logs[0] + logs[1]+ logs[2]+ logs[3];
                }
            }
        }

        /* 
         * When a date is selected, the information for that date is shown in the textBoxInformation
         */
        async private void listBoxData_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            if (listBox.SelectedIndex != -1)
            {
                //For Information textBox
                textBoxInformation.Text = "";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL_CONTAINER + "/data/" + listBoxData.SelectedItem.ToString());
                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro no logs para a data");
                    }

                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(content);
                    string linha = "\t\tData: " + jsonObject["Id"].ToString() + "     " + jsonObject["Name"].ToString() + "     " + jsonObject["Parent"].ToString() + "     " + jsonObject["Content"].ToString() + "     " + jsonObject["CreationDT"].ToString() + Environment.NewLine + Environment.NewLine;
                    logs[3] = linha;
                    string existe = logs[2] == "" ? "" : logs[2];
                    textBoxInformation.Text = logs[0] + logs[1] + logs[2] + logs[3];
                }
            }
        }

        // Deleting an application
        async private void btnRemove01_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Tens a certeza?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                if (listBoxApplications.SelectedIndex != -1)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, URL_APP);
                        HttpResponseMessage response = await httpClient.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro porque existem dependencias");
                        }
                        else
                        {
                            MessageBox.Show("Aplicação removida com sucesso");
                            listBoxApplications.Items.RemoveAt(listBoxApplications.SelectedIndex);
                            listBoxContainers.Items.Clear();
                            listBoxSubscriptions.Items.Clear();
                            listBoxData.Items.Clear();
                            logs[0] = "";
                            refreshLogs();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Selecione uma aplicação");
                }
            }
        }

        // Editing an application
        async private void btnEdit01_Click(object sender, EventArgs e)
        {
            if (listBoxApplications.SelectedIndex != -1)
            {
                ExibirMessageBoxInserirDados("Novo nome da aplicação:", "Alterar");
                if (data_cache != "")
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string xmlContent = "<resource res_type='application'><name>"+data_cache+"</name></resource>";
                        StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                        HttpResponseMessage response = await httpClient.PutAsync(URL_APP, stringContent);

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Aplicação renomeada com sucesso");
                            listBoxApplications.Items[listBoxApplications.SelectedIndex] = data_cache;
                            listBoxContainers.Items.Clear();
                            listBoxSubscriptions.Items.Clear();
                            listBoxData.Items.Clear();
                        }
                    }
                }

                data_cache = "";
            }
            else
            {
                MessageBox.Show("Selecione uma aplicação");
            }
        }

        // Add an application
        async private void btnAdd01_Click(object sender, EventArgs e)
        {
            ExibirMessageBoxInserirDados("Nome da aplicação:", "Adicionar");
            if (data_cache != "")
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string xmlContent = "<resource res_type='application'><name>" + data_cache + "</name></resource>";
                    StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                    HttpResponseMessage response = await httpClient.PostAsync(URL, stringContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                    }
                    else
                    {
                        MessageBox.Show("Aplicação adicionada com sucesso");
                        listBoxApplications.Items.Add(data_cache);
                        listBoxContainers.Items.Clear();
                        listBoxSubscriptions.Items.Clear();
                        listBoxData.Items.Clear();
                    }
                }
            }
            data_cache = "";
        }

        //container-related btns
        private void btnAdd03_Click(object sender, EventArgs e)
        {
            //add container
            if (listBoxApplications.SelectedIndex != -1)
            {
                ExibirMessageBoxInserirDados("Nome do container:", "Adicionar");
                if (data_cache != "")
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string xmlContent = "<resource res_type='container'><name>" + data_cache + "</name></resource>";
                        StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                        HttpResponseMessage response = httpClient.PostAsync(URL_APP, stringContent).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Container adicionado com sucesso");
                            listBoxContainers.Items.Add(data_cache);
                        }
                    }
                }

                data_cache = "";
            }
            else
            {
                MessageBox.Show("Selecione uma aplicação");
            }
        }

        private void btnEdit03_Click(object sender, EventArgs e)
        {
            //edit container
            if (listBoxContainers.SelectedIndex != -1)
            {
                ExibirMessageBoxInserirDados("Novo nome do container:", "Alterar");
                if (data_cache != "")
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string xmlContent = "<resource res_type='container'><name>" + data_cache + "</name></resource>";
                        StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                        HttpResponseMessage response = httpClient.PutAsync(URL_APP + "/" + listBoxContainers.SelectedItem.ToString(), stringContent).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Container renomeado com sucesso");
                            listBoxContainers.Items[listBoxContainers.SelectedIndex] = data_cache;
                        }
                    }
                }

                data_cache = "";
            }
            else
            {
                MessageBox.Show("Selecione um container");
            }
        }

        private void btnRemove02_Click(object sender, EventArgs e)
        {
            //remove container
            if (listBoxContainers.SelectedIndex != -1)
            {
                DialogResult resultado = MessageBox.Show("Tens a certeza?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    if (listBoxContainers.SelectedIndex != -1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, URL_APP + "/" + listBoxContainers.SelectedItem.ToString());
                            HttpResponseMessage response = httpClient.SendAsync(request).Result;

                            if (!response.IsSuccessStatusCode)
                            {
                                MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}\nErro porque existem dependencias");
                            }
                            else
                            {
                                MessageBox.Show("Container removido com sucesso");
                                listBoxContainers.Items.RemoveAt(listBoxContainers.SelectedIndex);
                                listBoxSubscriptions.Items.Clear();
                                listBoxData.Items.Clear();
                                logs[1] = "";
                                refreshLogs();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selecione um container");
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um container");
            }

        }

        //subscription-related btns
        private void btnAdd02_Click(object sender, EventArgs e)
        {
            //add subscription
            if (listBoxContainers.SelectedIndex != -1)
            {
                ExibirMesssageBonxInserirSubscription("Nome da subscription:", "Adicionar");
                if (edit_cache[0] != "" && edit_cache[1] != "" && edit_cache[2] != "" && edit_cache[3] != "" && flag_edit_sub == true)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string xmlContent = "<resource res_type='subscription'><name>" + edit_cache[0] + "</name><feature>" + edit_cache[1] + "</feature><event>" + edit_cache[2] + "</event><endpoint>" + edit_cache[3] + "</endpoint></resource>";
                        StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                        HttpResponseMessage response = httpClient.PostAsync(URL_CONTAINER, stringContent).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Subscription adicionada com sucesso");
                            listBoxSubscriptions.Items.Add(edit_cache[0]);
                        }
                    }
                }
                else if(flag_edit_sub == true)
                {
                    MessageBox.Show("Preencha todos os campos");
                }

                edit_cache = new string[4];
            }
            else
            {
                MessageBox.Show("Selecione um container");
            }
            
        }

        private void btnDelete03_Click(object sender, EventArgs e)
        {
            if (listBoxSubscriptions.SelectedIndex != -1)
            {
                DialogResult resultado = MessageBox.Show("Tens a certeza?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, URL_CONTAINER + "/sub/" + listBoxSubscriptions.SelectedItem.ToString());
                        HttpResponseMessage response = httpClient.SendAsync(request).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Subscription removida com sucesso");
                            listBoxSubscriptions.Items.RemoveAt(listBoxSubscriptions.SelectedIndex);
                            logs[2] = "";
                            refreshLogs();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione uma subscription");
            }
        }


        //date-related btns
        async private void btnAdd04_Click(object sender, EventArgs e)
        {
            //add data
            if (listBoxContainers.SelectedIndex != -1)
            {
                MostrarMessageBoxNewData();
                if (flag_delete == true)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string xmlContent;
                        if (edit_cache[0] == "")
                        {
                            xmlContent = $"<resource res_type='data'><content>{edit_cache[1]}</content></resource>";
                        }
                        else
                        {
                            xmlContent = $"<resource res_type='data'><name>{edit_cache[0]}</name><content>{edit_cache[1]}</content></resource>";
                        }

                        StringContent stringContent = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                        HttpResponseMessage response = await httpClient.PostAsync(URL_CONTAINER, stringContent);

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Data adicionado com sucesso");
                            listBoxData.Items.Add(data_cache);
                        }
                    }
                    edit_cache = new string[4];
                }
            }
            else
            {
                MessageBox.Show("Selecione um container");
            }
        }

        async private void btnDelete04_Click(object sender, EventArgs e)
        {
            if (listBoxData.SelectedIndex != -1)
            {
                DialogResult resultado = MessageBox.Show("Tens a certeza?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        HttpResponseMessage response = await httpClient.DeleteAsync(URL_CONTAINER + "/data/" + listBoxData.SelectedItem.ToString());

                        if (!response.IsSuccessStatusCode)
                        {
                            MessageBox.Show($"Erro na solicitação. Código de status: {response.StatusCode}");
                        }
                        else
                        {
                            MessageBox.Show("Data removida com sucesso");
                            listBoxData.Items.RemoveAt(listBoxData.SelectedIndex);
                            logs[3] = "";
                            refreshLogs();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione uma data");
            }
        }

        // refresh logs
        private void refreshLogs()
        {
            textBoxInformation.Text = "";
            textBoxInformation.Text = logs[0] + logs[1] + logs[2] + logs[3];
        }

        private void MostrarMessageBoxNewData()
        {
            // Form to add a new Data resource where the user can insert the name and the content of the data
            Form form = new Form
            {
                Text = "Inserir Dados",
                Size = new System.Drawing.Size(290, 220),
                StartPosition = FormStartPosition.CenterScreen
            };

            form.FormBorderStyle = FormBorderStyle.FixedSingle;

            Label label = new Label
            {
                Text = "Nome (Opcional)",
                Location = new System.Drawing.Point(10, 20),
                AutoSize = true
            };

            TextBox textBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 50),
                Width = 250
            };

            Label label1 = new Label
            {
                Text = "Content",
                Location = new System.Drawing.Point(10, 90),
                AutoSize = true
            };

            TextBox textBox1 = new TextBox
            {
                Location = new System.Drawing.Point(10, 120),
                Width = 250
            };

            Button okButton = new Button
            {
                Text = "Adicionar",
                Location = new System.Drawing.Point(20, 150)
            };

            Button cancelButton = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(170, 150)
            };

            okButton.Click += (sender, e) =>
            {
                string name = textBox.Text;
                string content = textBox1.Text;
                edit_cache[0] = name;
                edit_cache[1] = content;
                flag_delete = true;
                form.Close();
            };

            cancelButton.Click += (sender, e) =>
            {
                form.Close();
                flag_delete = false;
            };

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);

            form.ShowDialog();
        }

        // MsgBox to insert data
        private void ExibirMessageBoxInserirDados(string label_name, string confirm_name)
        {

            Form form = new Form
            {
                Text = "Inserir Dados",
                Size = new System.Drawing.Size(290, 170),
                StartPosition = FormStartPosition.CenterScreen
            };

            form.FormBorderStyle = FormBorderStyle.FixedSingle;

            Label label = new Label
            {
                Text = label_name,
                Location = new System.Drawing.Point(10, 20),
                AutoSize = true
            };

            TextBox textBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 50),
                Width = 250
            };

            Button okButton = new Button
            {
                Text = confirm_name,
                Location = new System.Drawing.Point(20, 90)
            };

            Button cancelButton = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(170, 90)
            };

            okButton.Click += (sender, e) =>
            {
                string dadosInseridos = textBox.Text;
                data_cache = dadosInseridos;
                flag_delete = true;
                form.Close();
            };

            cancelButton.Click += (sender, e) =>
            {
                flag_delete = false;
                form.Close();
            };

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);

            form.ShowDialog();
        }

        private void ExibirMesssageBonxInserirSubscription(string label_name, string confirm_name)
        {

            Form form = new Form
            {
                Text = "Inserir Dados",
                Size = new System.Drawing.Size(290, 360),
                StartPosition = FormStartPosition.CenterScreen
            };

            form.FormBorderStyle = FormBorderStyle.FixedSingle;

            Label label = new Label
            {
                Text = "Nome",
                Location = new System.Drawing.Point(10, 20),
                AutoSize = true
            };

            TextBox textBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 50),
                Width = 250
            };

            Label label1 = new Label
            {
                Text = "Feature",
                Location = new System.Drawing.Point(10, 90),
                AutoSize = true
            };

            TextBox textBox1 = new TextBox
            {
                Location = new System.Drawing.Point(10, 120),
                Width = 250
            };

            Label label2 = new Label
            {
                Text = "Event",
                Location = new System.Drawing.Point(10, 150),
                AutoSize = true
            };
            NumericUpDown numericUpDown = new NumericUpDown
            {
                Location = new System.Drawing.Point(10, 180),
                Width = 250,
                Minimum = 0,
                Maximum = 2,
                DecimalPlaces = 0
            };
            Label label3 = new Label
            {
                Text = "Endpoint",
                Location = new System.Drawing.Point(10, 210),
                AutoSize = true
            };

            TextBox textBox3 = new TextBox
            {
                Location = new System.Drawing.Point(10, 240),
                Width = 250
            };



            Button okButton = new Button
            {
                Text = confirm_name,
                Location = new System.Drawing.Point(20, 270)
            };

            Button cancelButton = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(170, 270)
            };

            okButton.Click += (sender, e) =>
            {
                flag_edit_sub = true;
                edit_cache[0] = textBox.Text;
                edit_cache[1] = textBox1.Text;
                edit_cache[2] = numericUpDown.Value.ToString();
                edit_cache[3] = textBox3.Text;

                form.Close();
            };

            cancelButton.Click += (sender, e) =>
            {
                flag_edit_sub = false;
                edit_cache[0] = edit_cache[1] = edit_cache[2] = edit_cache[3] = "";
                form.Close();
            };

            form.Controls.Add(label);
            form.Controls.Add(textBox);
            form.Controls.Add(label1);
            form.Controls.Add(textBox1);
            form.Controls.Add(label2);
            form.Controls.Add(numericUpDown);
            form.Controls.Add(label3);
            form.Controls.Add(textBox3);

            form.Controls.Add(okButton);
            form.Controls.Add(cancelButton);

            form.ShowDialog();
        }
    }
}
