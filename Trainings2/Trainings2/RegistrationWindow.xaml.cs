using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        Trener trener = new Trener();        

        public RegistrationWindow()
        {
            InitializeComponent();
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
            trener.sex = 'м';
        }

        private void RadioButton_woman_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            trener.sex = 'ж';
        }
        
        public async Task InsertTrenerAsync()
        {
            trener.login = Login_reg.Text;
            trener.sname = Sname_reg.Text;
            trener.name = Name_reg.Text;
            trener.phone = Convert.ToInt32(Telephon_reg.Text);
            trener.age = Convert.ToInt32(Age_reg.Text);
            
            try
            {
                using (SqlConnection cn = Connections.Connection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addNewCoach", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter login = new SqlParameter
                    {
                        ParameterName = "@login",
                        Value = trener.login
                    };

                    SqlParameter name = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = trener.name
                    };

                    SqlParameter sname = new SqlParameter
                    {
                        ParameterName = "@sname",
                        Value = trener.sname
                    };

                    SqlParameter phone = new SqlParameter
                    {
                        ParameterName = "@phone",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = trener.phone
                    };

                    SqlParameter sex = new SqlParameter
                    {
                        ParameterName = "@sex",
                        Value = trener.sex
                    };

                    SqlParameter age = new SqlParameter
                    {
                        ParameterName = "@age",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = trener.age
                    };

                    cmd.Parameters.Add(login);
                    cmd.Parameters.Add(name);
                    cmd.Parameters.Add(sname);
                    cmd.Parameters.Add(phone);
                    cmd.Parameters.Add(sex);
                    cmd.Parameters.Add(age);

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
            InsertTrenerAsync().GetAwaiter();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            Close();
        }
    }
}
