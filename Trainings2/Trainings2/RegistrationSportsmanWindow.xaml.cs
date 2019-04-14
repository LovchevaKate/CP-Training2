using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для RegistrationSportsmanWindow.xaml
    /// </summary>
    public partial class RegistrationSportsmanWindow : Window
    {
        Sportsman sportsman = new Sportsman();

        public RegistrationSportsmanWindow()
        {
            InitializeComponent();
            ReadAllCoachAsync().GetAwaiter();
        }
        
        public ObservableCollection<string> list = new ObservableCollection<string>();

        //для составления списка всех тренеров
        public async Task ReadAllCoachAsync()
        {
            string sqlExpression = "SELECT * FROM AllCoach";
            using (SqlConnection connection = Connections.Connection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        string id = reader.GetValue(0).ToString();
                        string name = reader.GetValue(2).ToString();
                        string sname = reader.GetValue(1).ToString();
                        string bigname = id + " " + name + " " + sname;
                        list.Add(bigname);
                    }
                }
                AllTreners.ItemsSource = list;
                reader.Close();
            }
        }

        private void String_reg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!(Regex.IsMatch(Name_reg.Text, @"^[а-яА-ЯёЁa-zA-Z]*$")) || !(Regex.IsMatch(Sname_reg.Text, @"^[а-яА-ЯёЁa-zA-Z]*$")))
                {
                    throw new Exception("Error!!! Неправильные символы");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Number_reg_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!(Regex.IsMatch(Age_reg.Text, @"^\d*$")))
                {
                    Age_reg.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }
                else if (!(Regex.IsMatch(Telephon_reg.Text, @"^\d*$")))
                {
                    Telephon_reg.Clear();
                    throw new Exception("Error!!! Неправильные символы");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RadioButton_man_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            sportsman.sex = 'м';
        }

        private void RadioButton_woman_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            sportsman.sex = 'ж';
        }

        public async Task InsertSportsmanAsync()
        {
            sportsman.login = Login_reg.Text;
            sportsman.sname = Sname_reg.Text;
            sportsman.name = Name_reg.Text;
            sportsman.phone = Convert.ToInt32(Telephon_reg.Text);
            sportsman.age = Convert.ToInt32(Age_reg.Text);
            var qwe = AllTreners.Text.Split(' ');
            int id = Convert.ToInt32(qwe[0]);
            sportsman.trenerid = id;

            try
            {
                using (SqlConnection cn = Connections.Connection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addNewSportsman", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter login = new SqlParameter
                    {
                        ParameterName = "@login",
                        Value = sportsman.login
                    };

                    SqlParameter name = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = sportsman.name
                    };

                    SqlParameter sname = new SqlParameter
                    {
                        ParameterName = "@sname",
                        Value = sportsman.sname
                    };

                    SqlParameter phone = new SqlParameter
                    {
                        ParameterName = "@phone",
                        Value = sportsman.phone
                    };

                    SqlParameter sex = new SqlParameter
                    {
                        ParameterName = "@sex",
                        Value = sportsman.sex
                    };

                    SqlParameter age = new SqlParameter
                    {
                        ParameterName = "@age",
                        Value = sportsman.age
                    };

                    SqlParameter idcoach = new SqlParameter
                    {
                        ParameterName = "@idcoach",
                        Value = sportsman.trenerid
                    };
                    
                    cmd.Parameters.Add(login);
                    cmd.Parameters.Add(name);
                    cmd.Parameters.Add(sname);
                    cmd.Parameters.Add(phone);
                    cmd.Parameters.Add(sex);
                    cmd.Parameters.Add(age);
                    cmd.Parameters.Add(idcoach);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_create_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Login_reg.Text) && !string.IsNullOrEmpty(AllTreners.Text) &&
                    !string.IsNullOrEmpty(Sname_reg.Text) && !string.IsNullOrEmpty(Name_reg.Text) &&
                    !string.IsNullOrEmpty(Telephon_reg.Text) && !string.IsNullOrEmpty(Age_reg.Text))
                {
                    InsertSportsmanAsync().GetAwaiter();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    throw new Exception("Поля не заполнены");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Close();
        }
    }
}
