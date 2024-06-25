using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace WindowsFormSQLiteDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        private void SetConnection()
        {
            sql_con = new SQLiteConnection("Data Source=DemoT.db;Version=3;New=False;Compress=True;");
        }

        private void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        private void LoadData()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "select * from DemoT";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dataGridView1.DataSource = DT;
            sql_con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            string txtQuery = "insert into DemoT (Name) values ('" + tName.Text + "')";
            ExecuteQuery(txtQuery);
            LoadData();
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            string txtQuery = "update DemoT set Name ='"+ tName.Text+"' where Id='"+tId.Text+"'";
            ExecuteQuery(txtQuery);
            LoadData();
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            string txtQuery = "delete from DemoT where Id='" + tId.Text + "'";
            ExecuteQuery(txtQuery);
            LoadData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex>=0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                tId.Text = row.Cells[0].Value.ToString();
                tName.Text = row.Cells[1].Value.ToString();
                LoadData();
                
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex,e.RowIndex];
            }
        }
    }
}
