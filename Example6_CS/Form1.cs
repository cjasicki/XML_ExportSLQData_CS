using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Example6_CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //*****
        //** Global variable
        //*****
        DataSet dsData = null;

        //*****
        //** Form
        //*****
        //*******************************************************
        //** Procedure: Form1_Load()
        //**   Initializes datagridview
        //*******************************************************
        private void Form1_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            LoadPayrollInfo();
        }

        //****
        //** Button
        //** Procedure:btnExport_load()
        //** Exports dataset to XML file

        private void BtnExport_Click_1(object sender, EventArgs e)
        {
            DialogResult dlgAnswer;
            DataSet dsXML;

            sfdXML.DefaultExt = "xml";
            sfdXML.Filter = "XML files(*.xml)|*.xml|All files(*.*|*.*";
            sfdXML.InitialDirectory = "C:\\Users\\cjasi\\Documents\\";
            sfdXML.OverwritePrompt = true;
            sfdXML.Title = "Save XML File";

            dlgAnswer = sfdXML.ShowDialog();
            if (dlgAnswer == DialogResult.OK)
            {
                dsData.WriteXml(sfdXML.FileName);

                // Now read back into another dataset
                dsXML = new DataSet();
                dsXML.ReadXml(sfdXML.FileName);
                dgvDisplay.DataSource = dsXML.Tables[0];
            }
        }

        //*****
        //** Procedure
        //*****
        //*******************************************************
        //** Procedure: LoadPayrollInfo()
        //**   Displays contents of Payroll table
        //*******************************************************
        private void LoadPayrollInfo()
        {
            SqlConnection cnSQL;
            SqlDataAdapter daSQL;

            lblError.Text = "";
            btnExport.Enabled = false;

            if (ConfigurationManager.ConnectionStrings["Payroll"] == null)
            {
                lblError.Text = "No Connection to Database";
            }
            else
            {
                cnSQL = clsDatabase.AcquireConnection("Payroll");
                if(cnSQL == null)
                {
                    lblError.Text = "Error Connection to Database";
                }
                else
                {
                    // Remove any existion dataset
                    if(dsData != null)
                    {
                        dsData.Dispose();
                    }
                    dsData = new DataSet();
                    try
                    {
                        daSQL = new SqlDataAdapter("SELECT * FROM dbo.Payroll;", cnSQL);
                        daSQL.Fill(dsData);
                        daSQL.Dispose();
                        dgvInfo.DataSource = dsData.Tables[0];
                        btnExport.Enabled = true;
                    }
                    catch
                    {
                        lblError.Text = "Error retrieving Payroll data";
                        dsData.Dispose();
                        dsData = null;
                    }
                    finally
                    {
                        cnSQL.Close();
                        cnSQL.Dispose();
                    }
                }
            }
        }
    }
}
