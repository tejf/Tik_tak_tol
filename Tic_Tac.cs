using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Projekt1
{
    public partial class Tic_Tac : Form
    {
        DataBase db = new DataBase();
        /// <summary>
        /// Konstruktor inicjalizujący działanie aplikacji
        /// </summary>
        public Tic_Tac()
        {
            InitializeComponent();
        }

        private void hasloLogowanieGracz2TextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void hasloLogowanieTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void emailLogowanieTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void emailLogowanieGracz2TextBox_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void hasloRejestracjaGracz2TextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void hasloRejestracjaSQLTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void EmailRejestracjaSQLTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        private void emailRejestracjaGracz2SqlTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Metoda sprawdzająca czy wartości z textBoxow istnieja w bazie danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logowanieButton_Click(object sender, EventArgs e)
        {
            String email1 = emailLogowanieTextBox.Text;
            String haslo1 = hasloLogowanieTextBox.Text;
            String email2 = emailLogowanieGracz2TextBox.Text;
            String haslo2 = hasloLogowanieGracz2TextBox.Text;
            

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM tik_tak WHERE email1 = @use1 and haslo1 = @pass1 and email2 = @use2 and haslo2 = @pass2", db.getConnection());

            command.Parameters.Add("@use1", MySqlDbType.VarChar).Value = email1;
            command.Parameters.Add("@pass1", MySqlDbType.VarChar).Value = haslo1;
            command.Parameters.Add("@pass2", MySqlDbType.VarChar).Value = haslo2;
            command.Parameters.Add("@use2", MySqlDbType.VarChar).Value = email2;

            adapter.SelectCommand = command;

            adapter.Fill(table);

            // check if the user exists or not
            if (table.Rows.Count > 0)
            {
                this.Hide();
                Form1 ob = new Form1(email1, email2);//wywoluje form1 i przesylam email1 i email2 poprzez parametr
                ob.Show();
            }
            else
            {
                // check if the username or the haslo don't exist
                MessageBox.Show("NIE PODANO POPRAWNIE PRZYNAJMNIEJ JEDNEJ WARTOSCI PRZY LOGOWANIU", "Złe dane", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            db.closeConnection();
        }
        /// <summary>
        /// Metoda zapisująca w bazie danych wartości z textBoxów
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rejestracjaSqlButton_Click(object sender, EventArgs e)
        {
            //string dodaj;
            string email1, haslo1, email2, haslo2;
            //dodaj = zapisDoSqlTextBox.Text;
            email1 = EmailRejestracjaSQLTextBox.Text;
            haslo1 = hasloRejestracjaSQLTextBox.Text;
            email2 = emailRejestracjaGracz2SqlTextBox.Text;
            haslo2 = hasloRejestracjaGracz2TextBox.Text;
            //MessageBox.Show(email1 + haslo1 + email2 + haslo2);
            MySqlCommand command = new MySqlCommand("INSERT INTO tik_tak(`email1`, `email2`, `haslo1`, `haslo2`, `punkt1`, `punkt2`)" +
                " VALUES (@email1, @email2, @haslo1, @haslo2, @punkt1, @punkt2)", db.getConnection());
            command.Parameters.Add("@email1", MySqlDbType.Text).Value = email1;
            command.Parameters.Add("@email2", MySqlDbType.Text).Value = email2;
            command.Parameters.Add("@haslo1", MySqlDbType.Text).Value = haslo1;
            command.Parameters.Add("@haslo2", MySqlDbType.Text).Value = haslo2;
            command.Parameters.Add("@punkt1", MySqlDbType.Int64).Value = 0;
            command.Parameters.Add("@punkt2", MySqlDbType.Int64).Value = 0;

            db.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Konto zostało założone", "Konto założone", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("ERROR");
            }

            // close the connection
            db.closeConnection();
            //oddawida start - po kliknieciu zarejestruj, ma zresetowac wpisany tekst
            EmailRejestracjaSQLTextBox.ResetText();
            hasloRejestracjaSQLTextBox.ResetText();
            emailRejestracjaGracz2SqlTextBox.ResetText();
            hasloRejestracjaGracz2TextBox.ResetText();
        }  
        private void Tic_Tac_Load(object sender, EventArgs e)
        {

        }
    }
}
