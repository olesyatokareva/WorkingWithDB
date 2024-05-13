using System;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WorkingWithDB
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        string connectionString = "Server =localhost;Port=5432;Database=music; User id=postgres; Password=postgres";
        public Form1()
        {
            InitializeComponent();

            SqlConnectionReader();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //вывести таблицу пользователи
        private void SqlConnectionReader()
        {
            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT id_user, user_name FROM users";
            NpgsqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.HasRows) 
            {
                DataTable data = new DataTable();
                data.Load(dataReader);
                dataGridView1.DataSource =  data;
            
            }

            command.Dispose();
            sqlConnection.Close();

            //вывести таблицу песен с альбомом
            sqlConnection.Open();
            NpgsqlCommand command1 = new NpgsqlCommand();
            command1.Connection = sqlConnection;
            command1.CommandType = CommandType.Text;
            command1.CommandText = "SELECT ss.song_name, ss.genre_name, al.album_name FROM songs ss JOIN albums al ON ss.album_id = al.album_id ";
            NpgsqlDataReader dataReadersongs = command1.ExecuteReader();
            if (dataReadersongs.HasRows)
            {
                DataTable data = new DataTable();
                data.Load(dataReadersongs);
                dataGridView2.DataSource = data;

            }
            command1.Dispose();
            sqlConnection.Close();

            //вывести таблицу с плейлистами вместе с пользователями, которым они принадлежат
            sqlConnection.Open();
            NpgsqlCommand command2 = new NpgsqlCommand();
            command2.Connection = sqlConnection;
            command2.CommandType = CommandType.Text;
            command2.CommandText = "SELECT pl.playlist_id, pl.playlist_name, us.user_name FROM playlists pl JOIN users us ON pl.id_user = us.id_user";
            NpgsqlDataReader dataReaderpl = command2.ExecuteReader();
            if(dataReaderpl.HasRows)
            {
                DataTable data = new DataTable();
                data.Load(dataReaderpl);
                dataGridView3.DataSource = data;
            }
            command2.Dispose();
            sqlConnection.Close();

            //вывести таблицу исполнителей
            sqlConnection.Open();
            NpgsqlCommand command3 = new NpgsqlCommand();
            command3.Connection = sqlConnection;
            command3.CommandType = CommandType.Text;
            command3.CommandText = "SELECT * FROM artists";
            NpgsqlDataReader dataReaderart = command3.ExecuteReader();
            if (dataReaderart.HasRows)
            {
                DataTable data = new DataTable();
                data.Load(dataReaderart);
                dataGridView4.DataSource = data;
            }
            command3.Dispose();
            sqlConnection.Close();

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //Добавление нового пользователя
        private async void button1_Click(object sender, EventArgs e)
        {
            if(label7.Visible)
                label7.Visible = false;
            
            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("INSERT INTO users (id_user, user_name) VALUES ('{0}','{1}')", textBox1.Text, textBox2.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView1.DataSource = data;

                }
                comm.Dispose();
            }
               
            else
            {
                label7.Visible = true;
                label7.Text = "Необходимые поля не заполнены!";

            }

            
            sqlConnection.Close();
            SqlConnectionReader();
           

        }
        //Добавление новой песни
        private void button4_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox7.Text) &&
                !string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text) &&
                !string.IsNullOrEmpty(textBox12.Text) && !string.IsNullOrWhiteSpace(textBox12.Text) &&
                !string.IsNullOrEmpty(textBox13.Text) && !string.IsNullOrWhiteSpace(textBox13.Text) &&
                !string.IsNullOrEmpty(textBox14.Text) && !string.IsNullOrWhiteSpace(textBox14.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("CALL add_song('{0}','{1}','{2}','{3}','{4}')", textBox7.Text, textBox6.Text, textBox12.Text, textBox13.Text, textBox14.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView2.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label8.Visible = true;
                label8.Text = "Необходимые поля не заполнены!";

            }


            sqlConnection.Close();
            SqlConnectionReader();


        }
        //добавление нового плейлиста
        private void button5_Click(object sender, EventArgs e)
        {
            if (label14.Visible)
                label14.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox9.Text) && !string.IsNullOrWhiteSpace(textBox9.Text) &&
                !string.IsNullOrEmpty(textBox8.Text) && !string.IsNullOrWhiteSpace(textBox8.Text) &&
                !string.IsNullOrEmpty(textBox15.Text) && !string.IsNullOrWhiteSpace(textBox15.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("INSERT INTO playlists(playlist_id, playlist_name, id_user) VALUES('{0}','{1}','{2}')", textBox8.Text, textBox15.Text, textBox9.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView3.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label14.Visible = true;
                label14.Text = "Необходимые поля не заполнены!";

            }


            sqlConnection.Close();
            SqlConnectionReader();
        }
        //добавление нового исполнителя
        private void button6_Click(object sender, EventArgs e)
        {
            if (label18.Visible)
                label18.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox11.Text) && !string.IsNullOrWhiteSpace(textBox11.Text) &&
                !string.IsNullOrEmpty(textBox10.Text) && !string.IsNullOrWhiteSpace(textBox10.Text) &&
                !string.IsNullOrEmpty(textBox17.Text) && !string.IsNullOrWhiteSpace(textBox17.Text) &&
                !string.IsNullOrEmpty(textBox16.Text) && !string.IsNullOrWhiteSpace(textBox16.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("INSERT INTO artists(artist_id, artist_name, country, type_artist) VALUES('{0}','{1}','{2}','{3}')", textBox11.Text, textBox10.Text, textBox17.Text, textBox16.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView4.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label18.Visible = true;
                label18.Text = "Необходимые поля не заполнены!";

            }


            sqlConnection.Close();
            SqlConnectionReader();
        }
        //изменение имени пользователя
        private void button2_Click(object sender, EventArgs e)
        {
            if (label33.Visible)
                label33.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox4.Text) &&
                !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrWhiteSpace(textBox3.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("UPDATE users SET user_name = '{0}' WHERE id_user = '{1}'", textBox3.Text, textBox4.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView1.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label33.Visible = true;
                label33.Text = "Необходимые поля не заполнены!";

            }


            sqlConnection.Close();
            SqlConnectionReader();
        }
        //изменение названия плейлиста
        private void button7_Click(object sender, EventArgs e)
        {
            if (label34.Visible)
                label34.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox19.Text) && !string.IsNullOrWhiteSpace(textBox19.Text) &&
                !string.IsNullOrEmpty(textBox18.Text) && !string.IsNullOrWhiteSpace(textBox18.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("UPDATE playlists SET playlist_name = '{0}' WHERE playlist_id = '{1}'", textBox18.Text, textBox19.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView3.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label34.Visible = true;
                label34.Text = "Необходимые поля не заполнены!";

            }


            sqlConnection.Close();
            SqlConnectionReader();
        }
        //удаление песни из плейлиста
        private void button3_Click(object sender, EventArgs e)
        {
            if (label37.Visible)
                label37.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) &&
                !string.IsNullOrEmpty(textBox20.Text) && !string.IsNullOrWhiteSpace(textBox20.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("DELETE FROM playlistsongs WHERE (playlist_id = '{0}' AND song_id = '{1}')", textBox20.Text, textBox5.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView3.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label37.Visible = true;
                label37.Text = "Удаление невозможно. Заполните необходимые поля!";

            }


            sqlConnection.Close();
            SqlConnectionReader();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (label40.Visible)
                label40.Visible = false;

            NpgsqlConnection sqlConnection = new NpgsqlConnection(connectionString);
            sqlConnection.Open();

            if (!string.IsNullOrEmpty(textBox21.Text) && !string.IsNullOrWhiteSpace(textBox21.Text))
            {
                NpgsqlCommand comm = new NpgsqlCommand();
                comm.Connection = sqlConnection;
                comm.CommandType = CommandType.Text;
                comm.CommandText = String.Format("SELECT pl.playlist_id, pl.playlist_name, ss.song_id, ss.song_name FROM playlists pl JOIN playlistsongs ps ON pl.playlist_id = ps.playlist_id JOIN songs ss ON ps.song_id = ss.song_id where pl.playlist_id = '{0}'", textBox21.Text);
                NpgsqlDataReader dataReader = comm.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DataTable data = new DataTable();
                    data.Load(dataReader);
                    dataGridView5.DataSource = data;

                }
                comm.Dispose();
            }

            else
            {
                label40.Visible = true;
                label40.Text = "Введите необходимые данные!";

            }


            sqlConnection.Close();
        }
    }
}
