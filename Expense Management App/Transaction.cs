﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Expense_Management_App
{
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
        }

        SQLiteConnection connection = new SQLiteConnection(ConfigurationManager.ConnectionStrings["lite"].ToString());

        void getIncome()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    string select = "SELECT ID, Amount FROM Income WHERE ID NOT IN (SELECT[Income ID] FROM Transactions);";
                    SQLiteCommand command = new SQLiteCommand(select, connection);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    incomecmd.DisplayMember = "Amount";
                    incomecmd.ValueMember = "ID";
                    incomecmd.DataSource = dataTable;
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Connection to the database is closed");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Encountered this error " + error);
            }
        }

        void getBudget()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    string select = "SELECT ID, Name FROM Budget WHERE ID NOT IN (SELECT [Budget ID] FROM Transactions);";
                    SQLiteCommand command = new SQLiteCommand(select, connection);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    budgetcmd.DisplayMember = "Name";
                    budgetcmd.ValueMember = "ID";
                    budgetcmd.DataSource = dataTable;
                    connection.Close();
                }
                else
                {
                    MessageBox.Show("Connection to the database is closed");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Encountered this error " + error);
            }
        }

        private void Transaction_Load(object sender, EventArgs e)
        {
            getBudget();
            getIncome();
        }

        private void addExpensebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    if (sourcecmd.SelectedIndex != 0)
                    {
                        string insert = "Insert into Transactions Values(NULL, '0',@income, @status, '" + DateTime.Now.ToString("dd/MMMM/yyyy") + "')";
                        SQLiteCommand command = new SQLiteCommand(insert, connection);
                        command.Parameters.Add(new SQLiteParameter("@budget", budgetcmd.SelectedValue));
                        command.Parameters.Add(new SQLiteParameter("@income", incomecmd.SelectedValue));
                        command.Parameters.Add(new SQLiteParameter("@status", sourcecmd.SelectedItem));
                        var execute = command.ExecuteNonQuery();
                        if (execute > 0)
                        {
                            MessageBox.Show("New Transaction Created.");
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Failed to create a Transaction.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill in all fields");
                    }
                    connection.Close();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Encountered an error " + error.Message);
            }
        }

        internal string id;

        private void gunaButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    string insert = "Update Transactions set [Budget ID]=@budget, [Income ID]=@income, Status=@status where ID=@id";
                    if (id != null || id != "0")
                    {
                        SQLiteCommand command = new SQLiteCommand(insert, connection);
                        command.Parameters.Add(new SQLiteParameter("@budget", budgetcmd.SelectedValue));
                        command.Parameters.Add(new SQLiteParameter("@income", incomecmd.SelectedValue));
                        command.Parameters.Add(new SQLiteParameter("@status", sourcecmd.SelectedValue));
                        var execute = command.ExecuteNonQuery();
                        if (execute > 0)
                        {
                            MessageBox.Show("Transaction updated.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to update a Transaction.");
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Encountered an error " + error.Message);
            }
            Close();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}