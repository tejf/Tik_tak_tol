using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Projekt1
{
    /// <summary>
    /// Glowna klasa, w ktorej zawarte sa wszystkie najwazniejsze metody projektu.
    /// Na poczatku tworzone sa 2 wazne zmienne - "turn", dzieki ktorej wiemy kogo jest tura,
    /// oraz "turn_count" czyli licznik tur, ktory jest inkrementowany po kazdym kliknieciu przycisku.
    /// </summary>
    public partial class Form1 : Form
    {
        bool turn = true;  // true = tura X,   false = tura Y
        int turn_count = 0;// licznik tur wykorzystywany do sprawdzenia czy gra skonczyla sie remisem.

        public string Email1 { get; }
        public string Email2 { get; }

        public Form1(string em1, string em2)
        {
            Email1 = em1;//odczyt email1
            Email2 = em2;//odczyt email2
            InitializeComponent();
        }
        /// <summary>
        /// 3 metody uruchamiane po kliknieciu jednej z opcji w pasku zadan - "About", "Exit", "Instructions".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Bartosz Rajzer, Maciej Pelc, Dawid Sopel", "About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1. Click a button to put your mark on it. \n2. First to put 3 in a row wins.", "Instructions");
        }
        /// <summary>
        /// Najwazniejsza metoda, ktora definiuje co sie wydarzy po kliknieciu przycisku.
        /// Metoda sprawdza dzieki zmiennej "turn" kogo jest aktualnie tura i dzieki temu wie,
        /// czy zmienic tekst na przycisku na "X" czy "O", po czym dezaktywuje klikniety przycisk.
        /// Nastepnie nastepuje zmiana tury i zinkrementowanie zmiennej "turn_count".
        ///
        /// Nastepny "if" sprawdza kto jest zwyciezca gry i wyswietla adekwatny komentarz.
        /// Jesli zmienna "turn_count" jest rowna 9, gra konczy sie remisem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (turn)
                b.Text = "X";
            else
                b.Text = "O";

            turn = !turn;
            b.Enabled = false;

            turn_count++;

            if (Equations.chechForWinner(new Equations.button(A1.Text, A1.Enabled),
                new Equations.button(A2.Text, A2.Enabled),
                new Equations.button(A3.Text, A3.Enabled),
                new Equations.button(B1.Text, B1.Enabled),
                new Equations.button(B2.Text, B2.Enabled),
                new Equations.button(B3.Text, B3.Enabled),
                new Equations.button(C1.Text, C1.Enabled),
                new Equations.button(C2.Text, C2.Enabled),
                new Equations.button(C3.Text, C3.Enabled)))
            {
                disableButtons();
                String winner = "";
                if (turn)
                {

                    DataBase db = new DataBase();
                    db.openConnection();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand dodajpunkta = new MySqlCommand("UPDATE tik_tak SET punkt2 = punkt2 + 1 WHERE email2 = @imejl", db.getConnection());//dodawanie punkta dla drugiego maila
                    dodajpunkta.Parameters.Add("@imejl", MySqlDbType.Text).Value = Email2;
                    adapter.SelectCommand = dodajpunkta;
                    adapter.Fill(table);
                    MySqlCommand zapytanie = new MySqlCommand("select punkt2 from tik_tak where email2 = @email", db.getConnection());//pytanie o punkty dla drugiego maila
                    zapytanie.Parameters.Add("@email", MySqlDbType.Text).Value = Email2;

                    MySqlDataReader read = zapytanie.ExecuteReader();
                    read.Read();//czytam
                    int punkty = read.GetInt32(0);//wstawiam wynik z czytania do zmiennej

                    db.closeConnection();
                    winner = "O(gracz 2), his email - " + Email2.ToString() + " points total: " + punkty;
                }
                else
                {

                    DataBase db = new DataBase();
                    db.openConnection();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand dodajpunkta = new MySqlCommand("UPDATE tik_tak SET punkt1 = punkt1 + 1 WHERE email1 = @imejl", db.getConnection());//dodawanie punkta dla pierwszego maila
                    dodajpunkta.Parameters.Add("@imejl", MySqlDbType.Text).Value = Email1;
                    adapter.SelectCommand = dodajpunkta;
                    adapter.Fill(table);
                    MySqlCommand zapytanie = new MySqlCommand("select punkt1 from tik_tak where email1 = @email", db.getConnection());//pytanie o punkty dla pierwszego maila
                    zapytanie.Parameters.Add("@email", MySqlDbType.Text).Value = Email1;

                    MySqlDataReader read = zapytanie.ExecuteReader();
                    read.Read();//czytam
                    int punkty = read.GetInt32(0);//wstawiam wynik z czytania do zmiennej
                    db.closeConnection();

                    winner = "X(gracz 1), his email - " + Email1.ToString() + " points total: " + punkty;
                }

                MessageBox.Show("The winner is " + winner, "Congratulations!");
            }
            else
            {
                if (turn_count == 9)
                    MessageBox.Show("Draw!\nTry again! ;)", "Unlucky!");
            }
        }


        /// <summary>
        /// Prosta metoda dezaktywujaca wszystkie przyciski, wykorzystywana po zakonczeniu rozgrywki.
        /// </summary>
        private void disableButtons()
        {
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }
            }
            catch { }
        }
        /// <summary>
        /// Metoda aktywujaca sie po kliknieciu opcji "New Game".
        /// Metoda ta wraca wszystkie ustawienia do tych sprzed rozpoczecia rozgrywki.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = true;
                    b.Text = "";
                }
            }
            catch { }

        }



     
    }

    
}
