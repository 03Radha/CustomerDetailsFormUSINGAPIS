using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CustomerDetailsFormUSINGAPIS
{
    public partial class Form1 : Form
    {
        HttpClient client = new HttpClient();
        private Form dataGridViewForm;
        private List<CustomerDetails> customers;
        private bool isEditMode = false;
       // private bool isDialogOpen = false;
        private bool isUpdate = false;
        TextBox searchTextBox = new TextBox();
        private bool isDelete = false;
        private bool isview = false;
        private Panel popupPanel;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            AddTextBoxFocusEvents(this);
            dataGridView1.CellClick += dataGridView1_CellClick;
            searchTextBox.TextChanged += new EventHandler(searchTextBox_TextChanged);
            // Initialize HttpClient and set the base address
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44344/api/Customers/GetCustomerdetails")
            };

            // Load customers asynchronously
            LoadCustomersAsync().ConfigureAwait(false);
          //  ToggleTextBoxesEnabled(false);

        }

        private async Task LoadCustomersAsync()
        {
            try
            {
                // Send a GET request to the API
                HttpResponseMessage response = await client.GetAsync("GetCustomerDetails");

                // Ensure a successful response
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseData = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response to a list of customer details
                List<CustomerDetails> customers = JsonConvert.DeserializeObject<List<CustomerDetails>>(responseData);

                // Bind the list to the DataGridView
                dataGridView1.DataSource = customers;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred: {e.Message}");
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            if (customers != null)
            {
                string searchQuery = searchTextBox.Text.ToLower();
                var filteredCustomers = customers.Where(c => c.Name.ToLower().Contains(searchQuery)).ToList();
                dataGridView1.DataSource = filteredCustomers;
            }
        }

        public void ClearFileds()
        {
            textid.Text = "";
            textnm.Text = "";
            textalisenm.Text = "";
            textctmtype.Text = "";
            textalert.Text = "";
            textemail.Text = "";
            textweb.Text = "";
            textBanknm.Text = "";
            textfax.Text = "";
            textcustcode.Text = "";
            textcredtday.Text = "";
            textcrdr.Text = "";
            textcrdlimit.Text = "";
            textAdd.Text = "";
            textareanm.Text = "";
            textcity.Text = "";
            textpstcode.Text = "";
            textstnm.Text = "";
            textscode.Text = "";
            textphnone.Text = "";
            textphtwo.Text = "";
            textmob.Text = "";
            textpancrd.Text = "";
            textadhrcrd.Text = "";
            textgst.Text = "";
            textlincesno.Text = "";
            textotherone.Text = "";
            textothertwo.Text = "";
            textdealer.Text = "";


        }

        private void textnm_TextChanged(object sender, EventArgs e)
        {

            //TextBox textBox = sender as TextBox;
            //if (textBox != null)
            //{
            //    textBox.BackColor = Color.Black; // Change to your desired color
            //    textBox.ForeColor = Color.White;
            //}

        }

        private void textnm_Enter(object sender, EventArgs e)
        {
            //textnm.Enabled = false; // Disable the TextBox

            // AddTextBoxFocusEvents(e);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (this.ActiveControl is TextBox || this.ActiveControl is ComboBox || this.ActiveControl is DateTimePicker)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void AddTextBoxFocusEvents(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    c.Enter += TextBox_Enter;
                    c.Leave += TextBox_Leave;
                }

                // Recursively add events to child controls
                if (c.Controls.Count > 0)
                {
                    AddTextBoxFocusEvents(c);
                }
            }
           // ToggleTextBoxesEnabled(true);
        }

        // Change the background color when the TextBox gains focus
        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = Color.Black;
                textBox.ForeColor = Color.White;
            }
        }

        // Revert the background color when the TextBox loses focus
        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.Black;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OpenTextBox();
            btnsave.Visible = true;
            btncancle.Visible = true;
            groupBox1.Visible = true;
            btnNew.Visible = false;
            btnModfy.Visible = false;
            btnExite.Visible = false;
            btnDel.Visible = false;
            btnView.Visible = false;
            button1.Visible = false;
            // textnm.Enabled = true;
            // ToggleTextBoxesEnabled(true);
            ShowSaveCancelButtons();

        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(textnm.Text))
            {
                MessageBox.Show("Name is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textcustcode.Text))
            {
                MessageBox.Show("Customer code is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textalisenm.Text))
            {
                MessageBox.Show("Alias name is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textalert.Text))
            {
                MessageBox.Show("Alert is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textctmtype.Text))
            {
                MessageBox.Show("Customer type is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textemail.Text))
            {
                MessageBox.Show("Email is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textweb.Text))
            {
                MessageBox.Show("Web is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textBanknm.Text))
            {
                MessageBox.Show("Bank name is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textfax.Text))
            {
                MessageBox.Show("Fax is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textAdd.Text))
            {
                MessageBox.Show("Address is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textareanm.Text))
            {
                MessageBox.Show("Area name is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textcity.Text))
            {
                MessageBox.Show("City is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textpstcode.Text))
            {
                MessageBox.Show("Postal code is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textstnm.Text))
            {
                MessageBox.Show("State name is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textphnone.Text))
            {
                MessageBox.Show("Phone 1 is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textphtwo.Text))
            {
                MessageBox.Show("Phone 2 is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textmob.Text))
            {
                MessageBox.Show("Mobile is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textpancrd.Text))
            {
                MessageBox.Show("PAN card is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textadhrcrd.Text))
            {
                MessageBox.Show("Aadhar card is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textgst.Text))
            {
                MessageBox.Show("GST is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textlincesno.Text))
            {
                MessageBox.Show("License number is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textotherone.Text))
            {
                MessageBox.Show("Other 1 is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textothertwo.Text))
            {
                MessageBox.Show("Other 2 is empty");
                return false;
            }
            if (string.IsNullOrEmpty(textdealer.Text))
            {
                MessageBox.Show("Dealer is empty");
                return false;
            }

            return true;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            //validatetextboxes();
            //try
            //{
            //int creditDays;
            //decimal crDrBalance, creditLimit;

            //if (!int.TryParse(textcredtday.Text, out creditDays))
            //{
            //    MessageBox.Show("Please enter a valid number for Credit Days.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (!decimal.TryParse(textcrdr.Text, out crDrBalance))
            //{
            //    MessageBox.Show("Please enter a valid number for CrDr Balance.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (!decimal.TryParse(textcrdlimit.Text, out creditLimit))
            //{
            //    MessageBox.Show("Please enter a valid number for Credit Limit.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            ////}
            //byte[] imageBytes = null;
            //if (pictureBox1.Image != null)
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
            //        imageBytes = ms.ToArray();
            //    }
            //}

            if (ValidateInputs())
            {
                
                var cust = new CustomerDetails
                {
                    //Id = int.Parse(textid.Text),
                    Name = textnm.Text,
                    AliasName = textalisenm.Text,
                    CustomerType = textctmtype.Text,
                    MobileAlert = textalert.Text,
                    Email = textemail.Text,
                    Website = textweb.Text,
                    BankName = textBanknm.Text,
                    Fax = textfax.Text,
                    CustCode = textcustcode.Text,
                    CreditDays = int.Parse(textcredtday.Text),
                    CrDrBalance = decimal.Parse(textcrdr.Text),
                    CreditLimit = decimal.Parse(textcrdlimit.Text),
                    ModifyOn = DateTime.Now,
                    Address = textAdd.Text,
                    AreaName = textareanm.Text,
                    PlaceCity = textcity.Text,
                    PostalCode = textpstcode.Text,
                    StateName = textstnm.Text,
                    StateCode = textscode.Text,
                    OffPhone1 = textphnone.Text,
                    OffPhone2 = textphtwo.Text,
                    Mobile = textmob.Text,
                    PanCardNo = textpancrd.Text,
                    AadharCardNo = textadhrcrd.Text,
                    GST = textgst.Text,
                    FoodLicenceNo = textlincesno.Text,
                    Other1 = textotherone.Text,
                    Other2 = textothertwo.Text,
                    TypeOfDealer = textdealer.Text,
                };

                MessageBox.Show("Data inserted successfully.");
                ClearFileds();
                CreateCustomer(cust);
                groupBox1.Visible = true;
                // ToggleTextBoxesEnabled(false);
                btnsave.Visible = false;
                btncancle.Visible = false;
                btnNew.Visible = true;
                btnModfy.Visible = true;
                btnDel.Visible = true;
                btnExite.Visible = true;
                btnView.Visible = true;
                button1.Visible = true;
            }
        }
        //catch (Exception ex)
        //{
        //    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}



        private void CreateCustomer(CustomerDetails customer)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            var response = client.PostAsync("CreateCustomer", content).Result;
            response.EnsureSuccessStatusCode();
        }

        private void btncancle_Click(object sender, EventArgs e)
        {
            CloseTextBox();
            // isEditMode = false;
            //button1.Visible = true;// new button
            btnView.Visible = true;
            btnNew.Visible = true;
            btnedite.Visible = true;
            buttondel.Visible = true;
            btncancle.Visible = false;
            btnModfy.Visible = true;
            buttondel.Visible = false;
            btnDel.Visible = true;
            btnedite.Visible = false;
            btnExite.Visible = true;
            btnsave.Visible = false;
            button1.Visible = true;

            //btnClear.Visible = true;

            groupBox1.Visible = true;
            // groupBox2.Visible = false;
            // btnupload.Visible = true;
            ClearFileds();
        }
        private void UpdateCustomer(CustomerDetails customer, int Id)
        {
            var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
            try
            {
                var response = client.PutAsync($"UpdateCustomer/{Id}", content).Result;
                response.EnsureSuccessStatusCode();
            }
            //catch (HttpRequestException ex)
            //{
            //    // Log the detailed error message
            //    //MessageBox.Show($"Request error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            catch (Exception ex)
            {
                // Log any other exceptions
                //MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModfy_Click(object sender, EventArgs e)
        {
            OpenTextBox();
            ShowDataGridViewInDialog();
            isEditMode = true;
            btnsave.Visible = false;
            btncancle.Visible = true;
            groupBox1.Visible = true;
            btnNew.Visible = false;
            btnModfy.Visible = false;
            btnExite.Visible = false;
            btnDel.Visible = false;
            btnView.Visible = false;
            btnedite.Visible = true;
            button1.Visible = false;
            ShowEditCancelButtons();

        }
        private void DeleteCustomer(int id)
        {
            var response = client.DeleteAsync($"DeleteCustomer/{id}").Result;
            response.EnsureSuccessStatusCode();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            ShowDataGridViewInDialog();
            isEditMode = true;
            button1.Visible = false;
            btnView.Visible = false;
            btnedite.Visible = false;
            buttondel.Visible = true;
            isUpdate = false;
            isDelete = true;
            btncancle.Visible = true;
            btnModfy.Visible = false;
            btnDel.Visible = false;
            btnExite.Visible = false;
            // button1.Visible = true;
            btnNew.Visible = false;
            //  b.Visible = false;
            ShowDeleteCancelButton();
        }

        private void btnedite_Click(object sender, EventArgs e)
        {
            //if (pictureBox1.Image != null)
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
            //       // imageBytes = ms.ToArray();
            //    }
            //}
            if (ValidateInputs())
            {

                var custo = new CustomerDetails
                {
                    Id = int.Parse(textid.Text),
                    Name = textnm.Text,
                    AliasName = textalisenm.Text,
                    CustomerType = textctmtype.Text,
                    MobileAlert = textalert.Text,
                    Email = textemail.Text,
                    Website = textweb.Text,
                    BankName = textBanknm.Text,
                    Fax = textfax.Text,
                    CustCode = textcustcode.Text,
                    CreditDays = int.Parse(textcredtday.Text),
                    CrDrBalance = decimal.Parse(textcrdr.Text),
                    CreditLimit = decimal.Parse(textcrdlimit.Text),
                    ModifyOn = DateTime.Now,
                    Address = textAdd.Text,
                    AreaName = textareanm.Text,
                    PlaceCity = textcity.Text,
                    PostalCode = textpstcode.Text,
                    StateName = textstnm.Text,
                    StateCode = textscode.Text,
                    OffPhone1 = textphnone.Text,
                    OffPhone2 = textphtwo.Text,
                    Mobile = textmob.Text,
                    PanCardNo = textpancrd.Text,
                    AadharCardNo = textadhrcrd.Text,
                    GST = textgst.Text,
                    FoodLicenceNo = textlincesno.Text,
                    Other1 = textotherone.Text,
                    Other2 = textothertwo.Text,
                    TypeOfDealer = textdealer.Text,
                };

                UpdateCustomer(custo, custo.Id);
                MessageBox.Show("Data updated successfully");
                ClearFileds();

                btnsave.Visible = false;
                btncancle.Visible = false;
                btnNew.Visible = true;
                btnModfy.Visible = true;
                btnDel.Visible = true;
                btnExite.Visible = true;
                btnView.Visible = true;
                button1.Visible = true;
                btnedite.Visible = false;

            }
        }
        public void SearchCustomer(string Name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    // If the search TextBox is empty, reload all customer data
                    var response = client.GetAsync("").Result;
                    response.EnsureSuccessStatusCode();
                    string jsonString = response.Content.ReadAsStringAsync().Result;
                    customers = JsonConvert.DeserializeObject<List<CustomerDetails>>(jsonString);
                    dataGridView1.DataSource = customers;
                }
                else
                {
                    // Fetch and filter data based on the search text
                    var response = client.GetStringAsync($"SearchByName?Name={Name}").Result;
                    var filteredCustomers = JsonConvert.DeserializeObject<List<CustomerDetails>>(response);
                    dataGridView1.DataSource = filteredCustomers;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Please insert a Correct Name");
            }
        }
        private async void ShowDataGridViewInDialog()
        {
            try
            {
                var response = await client.GetAsync("");
                response.EnsureSuccessStatusCode();

                string jsonString = await response.Content.ReadAsStringAsync();
                customers = JsonConvert.DeserializeObject<List<CustomerDetails>>(jsonString);

                dataGridView1.DataSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //dataGridViewForm = new Form();
            //dataGridViewForm.Text = "Customer Data";
            //dataGridViewForm.Size = new Size(800, 600);

            //dataGridView1.Visible = true;
            //dataGridView1.Dock = DockStyle.Fill;
            //dataGridViewForm.Controls.Add(dataGridView1);

            dataGridViewForm = new Form();
           // dataGridViewForm = new DataGridTableStyle.none;
            //dataGridViewForm.Text = "Customer Data";
            dataGridViewForm.Size = new Size(700, 600);
            dataGridViewForm.WindowState=FormWindowState.Normal;
            dataGridViewForm.ControlBox = false;
            dataGridView1.Size = new Size(50,50);

             Label searchlabel = new Label();
            searchlabel.Text = "Search Name here";
            searchlabel.Location = new Point(10, 10);
            searchlabel.Width = 200;

            // Create a TextBox
            //TextBox searchTextBox = new TextBox();
            searchTextBox.Location = new Point(10, 35);
            searchTextBox.Width = 200;
            //searchTextBox.TextChanged += (sender, e) => SearchCustomer(searchTextBox.Text);



            // Configure and add the DataGridView
            dataGridView1.Visible = true;
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Height = 500;
            dataGridView1.Location = new Point(10, 40);

            // Add controls to the form
            dataGridViewForm.Controls.Add(searchTextBox);
            dataGridViewForm.Controls.Add(searchlabel);
            dataGridViewForm.Controls.Add(dataGridView1);

            dataGridViewForm.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textid.Text = row.Cells["Id"].Value?.ToString() ?? string.Empty;
                textnm.Text = row.Cells["Name"].Value?.ToString() ?? string.Empty;
                textalisenm.Text = row.Cells["AliasName"].Value?.ToString() ?? string.Empty;
                textctmtype.Text = row.Cells["CustomerType"].Value?.ToString() ?? string.Empty;
                textalert.Text = row.Cells["MobileAlert"].Value?.ToString() ?? string.Empty;
                textemail.Text = row.Cells["Email"].Value?.ToString() ?? string.Empty;
                textweb.Text = row.Cells["Website"].Value?.ToString() ?? string.Empty;
                textBanknm.Text = row.Cells["BankName"].Value?.ToString() ?? string.Empty;
                textfax.Text = row.Cells["Fax"].Value?.ToString() ?? string.Empty;
                textcustcode.Text = row.Cells["CustCode"].Value?.ToString() ?? string.Empty;
                textcredtday.Text = row.Cells["CreditDays"].Value?.ToString() ?? string.Empty;
                textcrdr.Text = row.Cells["CrDrBalance"].Value?.ToString() ?? string.Empty;
                textcrdlimit.Text = row.Cells["CreditLimit"].Value?.ToString() ?? string.Empty;
                textAdd.Text = row.Cells["Address"].Value?.ToString() ?? string.Empty;
                textareanm.Text = row.Cells["AreaName"].Value?.ToString() ?? string.Empty;
                textcity.Text = row.Cells["PlaceCity"].Value?.ToString() ?? string.Empty;
                textpstcode.Text = row.Cells["PostalCode"].Value?.ToString() ?? string.Empty;
                textstnm.Text = row.Cells["StateName"].Value?.ToString() ?? string.Empty;
                textscode.Text = row.Cells["StateCode"].Value?.ToString() ?? string.Empty;
                textphnone.Text = row.Cells["OffPhone1"].Value?.ToString() ?? string.Empty;
                textphtwo.Text = row.Cells["OffPhone2"].Value?.ToString() ?? string.Empty;
                textmob.Text = row.Cells["Mobile"].Value?.ToString() ?? string.Empty;
                textpancrd.Text = row.Cells["PanCardNo"].Value?.ToString() ?? string.Empty;
                textadhrcrd.Text = row.Cells["AadharCardNo"].Value?.ToString() ?? string.Empty;
                textgst.Text = row.Cells["GST"].Value?.ToString() ?? string.Empty;
                textlincesno.Text = row.Cells["FoodLicenceNo"].Value?.ToString() ?? string.Empty;
                textotherone.Text = row.Cells["Other1"].Value?.ToString() ?? string.Empty;
                textothertwo.Text = row.Cells["Other2"].Value?.ToString() ?? string.Empty;
                textdealer.Text = row.Cells["TypeOfDealer"].Value?.ToString() ?? string.Empty;


                //if (row.Cells["Picture1"].Value != null && row.Cells["Picture1"].Value is byte[] imageBytes)
                //{
                //    using (MemoryStream ms = new MemoryStream(imageBytes))
                //    {
                //        pictureBox1.Image = Image.FromStream(ms);
                //    }
                //   // currentImage = imageBytes; // Store the existing image bytes
                //}
                //else
                //{
                //    pictureBox1.Image = null;
                //    //currentImage = null; // No image to store
                //}
                //isEditMode = true;

                if (dataGridViewForm != null && isEditMode)
                {
                    dataGridViewForm.Close();
                }


                //buttonEdit.Visible = true;
                //buttonCancel.Visible = true;
                //button1.Visible = false;
                //btnview.Visible = false;
                ////btnModify.Visible = false; // Initially hidden
                ////btnCancel.Visible = false; // Initially hidden
                //btnupd.Visible = false;
                //btndel.Visible = false;
                //btnexit.Visible = false;
                //btnClear.Visible = false;

                else if (isUpdate)
                {
                    ToggleButtonsForEdit();
                }
                else if (isDelete)
                {
                    ToggleButtonsForDelete();
                }
                else if (isview)
                {
                    ToggleButtonsForView();
                }

            }
        }
        private void ToggleButtonsForEdit()
        {
            // groupBox2.Visible = true;
            btnedite.Visible = true;
            btncancle.Visible = true;
            btnDel.Visible = false;
            btnView.Visible = false;
            btnModfy.Visible = false;
            btnDel.Visible = false;
            btnExite.Visible = false;
            button1.Visible = false;
            //button1.Visible = false;
            btnsave.Visible = false;
            groupBox1.Visible = false;
            // button1.Visible = false;

        }

        private void ToggleButtonsForDelete()
        {
            //groupBox2.Visible = true;
            groupBox1.Visible = false;
            btnDel.Visible = true;
            btncancle.Visible = true;
            btnDel.Visible = false;
            btnView.Visible = false;
            btnModfy.Visible = false;
            btnDel.Visible = false;
            btnExite.Visible = false;
            button1.Visible = false;
            //button1.Visible = false;
            btnsave.Visible = false;
            groupBox1.Visible = false;
        }

        private void ToggleButtonsForView()
        {
            // groupBox2.Visible = false;
            groupBox1.Visible = true;
            btnDel.Visible = false;
            btncancle.Visible = false;
            btnedite.Visible = false;
            btnView.Visible = true;
            btnModfy.Visible = true;
            btnDel.Visible = true;
            btnExite.Visible = true;
            button1.Visible = true;
            btnsave.Visible = false;
        }

        private void btnExite_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    ClearFileds();
        //}

        private void btnView_Click(object sender, EventArgs e)
        {
            ShowDataGridViewInDialog();
            isview = true;
            isUpdate = false;
            isDelete = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ClearFileds();
        }

        private void buttondel_Click(object sender, EventArgs e)
        {
            var id = int.Parse(textid.Text);
            DeleteCustomer(id);
            MessageBox.Show("Data Deleted Successfully");
            ClearFileds();


            btnsave.Visible = false;
            buttondel.Visible = false;
            btncancle.Visible = false;
            groupBox1.Visible = true;
            btnNew.Visible = true;
            btnModfy.Visible = true;
            btnExite.Visible = true;
            //button1.Visible = true;
            btnView.Visible = true;
            btnDel.Visible = true;
            button1.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void CloseTextBox()
        {
            textid.ReadOnly = true;
            textnm.ReadOnly = true;
            textalisenm.ReadOnly = true;
            textctmtype.ReadOnly = true;
            textalert.ReadOnly = true;
            textemail.ReadOnly = true;
            textweb.ReadOnly = true;
            textBanknm.ReadOnly = true;
            textfax.ReadOnly = true;
            textcustcode.ReadOnly = true;
            textcredtday.ReadOnly = true;
            textcrdr.ReadOnly = true;
            textcrdlimit.ReadOnly = true;
            textAdd.ReadOnly = true;
            textareanm.ReadOnly = true;
            textcity.ReadOnly = true;
            textpstcode.ReadOnly = true;
            textstnm.ReadOnly = true;
            textscode.ReadOnly = true;
            textphnone.ReadOnly = true;
            textphtwo.ReadOnly = true;
            textmob.ReadOnly = true;
            textpancrd.ReadOnly = true;
            textadhrcrd.ReadOnly = true;
            textgst.ReadOnly = true;
            textlincesno.ReadOnly = true;
            textotherone.ReadOnly = true;
            textothertwo.ReadOnly = true;
            textdealer.ReadOnly = true;


        }
        private void OpenTextBox()
        {
            textid.ReadOnly = false;
            textnm.ReadOnly = false;
            textalisenm.ReadOnly = false;
            textctmtype.ReadOnly = false;
            textalert.ReadOnly = false;
            textemail.ReadOnly = false;
            textweb.ReadOnly = false;
            textBanknm.ReadOnly = false;
            textfax.ReadOnly = false;
            textcustcode.ReadOnly = false;
            textcredtday.ReadOnly = false;
            textcrdr.ReadOnly = false;
            textcrdlimit.ReadOnly = false;
            textAdd.ReadOnly = false;
            textareanm.ReadOnly = false;
            textcity.ReadOnly = false;
            textpstcode.ReadOnly = false;
            textstnm.ReadOnly = false;
            textscode.ReadOnly = false;
            textphnone.ReadOnly = false;
            textphtwo.ReadOnly = false;
            textmob.ReadOnly = false;
            textpancrd.ReadOnly = false;
            textadhrcrd.ReadOnly = false;
            textgst.ReadOnly = false;
            textlincesno.ReadOnly = false;
            textotherone.ReadOnly = false;
            textothertwo.ReadOnly = false;
            textdealer.ReadOnly = false;


        }
        //public void validatetextboxes()
        //{
        //    if (string.IsNullOrEmpty(textnm.Text) == true || string.IsNullOrEmpty(textcustcode.Text) == true || 
        //        string.IsNullOrEmpty(textalisenm.Text) == true || string.IsNullOrEmpty(textctmtype.Text) == true 
        //        || string.IsNullOrEmpty(textmob.Text) == true || string.IsNullOrEmpty(textweb.Text) == true ||
        //        string.IsNullOrEmpty(textemail.Text) == true || string.IsNullOrEmpty(textBanknm.Text) == true || 
        //        string.IsNullOrEmpty(textfax.Text) == true || 
        //        string.IsNullOrEmpty(textAdd.Text) == true || string.IsNullOrEmpty(textareanm.Text) == true || 
        //        string.IsNullOrEmpty(textcity.Text) == true || string.IsNullOrEmpty(textpstcode.Text) == true || 
        //        string.IsNullOrEmpty(textstnm.Text) == true || string.IsNullOrEmpty(textscode.Text) == true || 
        //        string.IsNullOrEmpty(textphnone.Text) == true || string.IsNullOrEmpty(textphtwo.Text) == true || 
        //        string.IsNullOrEmpty(textalert.Text) == true || string.IsNullOrEmpty(textpancrd.Text) == true || 
        //        string.IsNullOrEmpty(textadhrcrd.Text) == true || string.IsNullOrEmpty(textgst.Text) == true || 
        //        string.IsNullOrEmpty(textlincesno.Text) == true || string.IsNullOrEmpty(textotherone.Text) == true ||
        //        string.IsNullOrEmpty(textothertwo.Text) == true || string.IsNullOrEmpty(textdealer.Text) == true  )
        //    {
        //        MessageBox.Show("text empty");
                
        //    }
           
        //}

        private void btnUpld_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog
            //{
            //    Filter = "Image Files|.jpg;.jpeg;.png;.bmp"
            //};

            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    pictureBox1.Image = Image.FromFile(openFileDialog.FileName);

            //    byte[] imageBytes;
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
            //        imageBytes = ms.ToArray();
            //    }
            //}
        }

        private void textnm_TextChanged_1(object sender, EventArgs e)
        {
           
               
            
        }
        private void ShowSaveCancelButtons()
        {
            //Hide other buttons
            button1.Visible = false;
            btnedite.Visible = false;
            btnDel.Visible = false;
            btnExite.Visible = false;
           // btnClear.Visible = false;
            btnView.Visible = false;
            // Show Save and Cancel buttons and center them
            btnsave.Visible = true;
            btncancle.Visible = true;

            // Center Save and Cancel buttons
            int centerX = (this.ClientSize.Width - btnsave.Width) / 3;
            btnsave.Location = new Point(centerX, btnsave.Location.Y);

            centerX = (this.ClientSize.Width - btncancle.Width) / 2;
            btncancle.Location = new Point(centerX, btncancle.Location.Y);
        }

        private void HideSaveCancelButtons()
        {
            //Show Others Buttons
            button1.Visible = true;
            btnedite.Visible = true;
            btnDel.Visible = true;
           // btnClear.Visible = true;
            btnExite.Visible = true;
            btnView.Visible = true;

            // Hide Save and Cancel buttons
            btnsave.Visible = false;
            btncancle.Visible = false;
            btnedite.Visible = false;
            buttondel.Visible = false;
        }

        private void ShowEditCancelButtons()
        {
            // Hide other buttons
            button1.Visible = false;
            btnModfy.Visible = false;
            btnDel.Visible = false;
            btnExite.Visible = false;
           // btnClear.Visible = false;
            btnView.Visible = false;
            // Show Save and Cancel buttons and center them
          btnedite.Visible = true;
            btncancle.Visible = true;

            // Center Save and Cancel buttons
            int centerX = (this.ClientSize.Width - btnedite.Width) / 3;
            btnedite.Location = new Point(centerX, btnedite.Location.Y);

            centerX = (this.ClientSize.Width - btncancle.Width) / 2;
            btncancle.Location = new Point(centerX, btncancle.Location.Y);
        }

        private void ShowDeleteCancelButton()
        {
            // Hide other buttons
            button1.Visible = false;
            btnModfy.Visible = false;
            btnDel.Visible = false;
            btnExite.Visible = false;
            // btnClear.Visible = false;
            btnView.Visible = false;
            // Show Save and Cancel buttons and center them
            buttondel.Visible = true;
            btncancle.Visible = true;

            // Center Save and Cancel buttons
            int centerX = (this.ClientSize.Width - buttondel.Width) / 3;
            buttondel.Location = new Point(centerX, buttondel.Location.Y);

            centerX = (this.ClientSize.Width - btncancle.Width) / 2;
            btncancle.Location = new Point(centerX, btncancle.Location.Y);
        }

    }

}


