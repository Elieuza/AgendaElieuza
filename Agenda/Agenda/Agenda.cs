using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Agenda
{

    public partial class frmAgenda : Form
    {
        public frmAgenda()
        {
            InitializeComponent();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            SqlConnection objCon = Conexao.Conectar();
            try
            {
                SqlCommand SqlComd = new SqlCommand(@"INSERT INTO Contatos(Nome,Email) " +
                    "VALUES(@nome,@email)",objCon);
                SqlComd.Parameters.AddWithValue("@nome", txtNome.Text);
                SqlComd.Parameters.AddWithValue("@email", txtEmail.Text);
                SqlComd.ExecuteNonQuery();
                MessageBox.Show("Informações gravadas com sucesso!");
                Conexao.fecharConexao();
                Exibir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void limparControles()
        {
            txtNome.Text = "";
            txtEmail.Text = "";
        }

        private void frmAgenda_Load(object sender, EventArgs e)
        {
            Exibir();
        }
        private void Exibir()
        {
            SqlConnection objCon = Conexao.Conectar();
            SqlCommand cmd = new SqlCommand("Select * from contatos ", objCon);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            dgvDados.DataSource = dataSet;
            dgvDados.DataMember = dataSet.Tables[0].TableName;

            Conexao.fecharConexao();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparControles();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            DialogResult resposta = MessageBox.Show("Deseja realmente sair?",
                "Agenda de Contatos", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resposta == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            String resposta = Convert.ToString(this.dgvDados.CurrentRow.Cells["id"].Value);
            if (resposta != string.Empty)
            {
                SqlConnection objCon = Conexao.Conectar();
                SqlCommand Comd = new SqlCommand("delete from contatos where id='" + resposta + "';");
                Comd.ExecuteNonQuery();
                MessageBox.Show("Registro apagado com sucesso!");
                Conexao.fecharConexao();
                Exibir();
            }

        }

        private void dgvDados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.txtNome.Text = Convert.ToString(this.dgvDados.CurrentRow.Cells["nome"].Value);
            this.txtEmail.Text = Convert.ToString(this.dgvDados.CurrentRow.Cells["email"].Value);
            

        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            string campoID = Convert.ToString(this.dgvDados.CurrentRow.Cells[id]);
            if (campoID != string.Empty)
            {
                SqlConnection objCon = Conexao.Conectar();
                SqlCommand Comd = new SqlCommand("update contatos set nome='"+txtNome.Text+"',email='"+txtEmail.Text+"' where id=" +campoID+";",objCon);

                Comd.ExecuteNonQuery();
                MessageBox.Show("Registro atualizado com sucesso!");
                Exibir();
            }
        }
    }
    class Conexao
    {
        private static SqlConnection sqlCon = null;
        private static SqlCommand SqlComd;
        private SqlDataAdapter dr;
        private static String conectionString = " Data Source=.\\SQLEXPRESS;" +
                                                 "Initial Catalog = Agenda;" +
                                                 "User ID = sa;" +
                                                 "Password = 3121";

        public static SqlConnection Conectar()
        {
            sqlCon = new SqlConnection(conectionString);
            try
            {
                sqlCon.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return sqlCon;
        }
        public static void fecharConexao()
        {
            if (sqlCon != null)
            {
                sqlCon.Close();
            }
        }

    }
}
