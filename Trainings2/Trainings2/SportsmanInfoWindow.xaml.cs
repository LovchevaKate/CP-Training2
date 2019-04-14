using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для SportsmanInfoWindow.xaml
    /// </summary>
    public partial class SportsmanInfoWindow : Window
    {
        public SportsmanInfoWindow()
        {
            InitializeComponent();
            TypeDay.ItemsSource = list;
        }

        public List<string> list = new List<string>() { "утро", "день", "вечер"};
        
        //добавление питания
        public async Task AddEatingAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.SportsmanConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addDiet", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter idsportsman = new SqlParameter
                    {
                        ParameterName = "@idsportsman",
                        Value = WorkClass.sportsman.sportsmanid
                    };

                    SqlParameter date = new SqlParameter
                    {
                        ParameterName = "@date",
                        Value = WorkClass.diet.date
                    };

                    SqlParameter period = new SqlParameter
                    {
                        ParameterName = "@period",
                        Value = WorkClass.diet.dietperiod
                    };

                    SqlParameter idmeal = new SqlParameter
                    {
                        ParameterName = "@idmeal",
                        Value = WorkClass.meal.mealid
                    };

                    cmd.Parameters.Add(idsportsman);
                    cmd.Parameters.Add(date);
                    cmd.Parameters.Add(period);
                    cmd.Parameters.Add(idmeal);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteMealAsync()
        {
            try
            {
                using (SqlConnection cn = Connections.SportsmanConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("deleteMeal", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter idmeal = new SqlParameter
                    {
                        ParameterName = "@idmeal",
                        Value = WorkClass.meal.mealid
                    };
                    
                    cmd.Parameters.Add(idmeal);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //заполнение таблицы питания
        public async Task ReadDietAsync()
        {
            string sqlExpression = "SELECT * FROM readDiet2(@idsportsman, @dietdate)";
            using (SqlConnection connection = Connections.SportsmanConnection.GetConnection())
            {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);

                    SqlParameter idsportsman = new SqlParameter
                {
                    ParameterName = "@idsportsman",
                    Value = WorkClass.sportsman.sportsmanid
                };

                SqlParameter dietdate = new SqlParameter
                {
                    ParameterName = "@dietdate",
                    Value = WorkClass.diet.date
                };

                command.Parameters.Add(idsportsman);
                command.Parameters.Add(dietdate);


                SqlDataReader reader = command.ExecuteReader();
               
                    while (reader.Read())
                    {
                        Diet diet = new Diet
                        {
                            dietid = Convert.ToInt32(reader.GetValue(0)),
                            date = Convert.ToDateTime(reader.GetValue(1)),
                            dietperiod = reader.GetValue(2).ToString()
                        };
                        if (diet.dietperiod == "утро")
                            diet.morning = reader.GetValue(3).ToString();
                        else if (diet.dietperiod == "день")
                            diet.lunch = reader.GetValue(3).ToString();
                        else if (diet.dietperiod == "вечер")
                            diet.diner = reader.GetValue(3).ToString();

                        EatingDataGrid.Items.Add(diet);
                    }
                    connection.Close();
              
            }
        }

        //заполнение таблицы блюд
        public async Task ReadAllMealAsync()
        {
            string sqlExpression = "SELECT * FROM MealCalories2()";
            using (SqlConnection connection = Connections.SportsmanConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Meal meal = new Meal
                        {
                            mealid = Convert.ToInt32(reader.GetValue(0)),
                            mealname = reader.GetValue(1).ToString(),
                            calories = Convert.ToInt32(reader.GetValue(2))
                        };

                        MealDataGrid.Items.Add(meal);
                    }
                    connection.Close();
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await ReadAllMealAsync();
                WorkClass.diet.date = DateTime.Today;
                await ReadDietAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //выход
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            SportsmanWindow sportsmanWindow = new SportsmanWindow();
            sportsmanWindow.Show();
            Close();
        }

        //для перехода к созданию блюда
        private void Button_Click_CreateMeal(object sender, RoutedEventArgs e)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            Close();
        }

        //выбор даты
        private async void Calendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                WorkClass.diet.date = Calendar.SelectedDate.Value;
                EatingDataGrid.Items.Clear();
                await ReadDietAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //кнопка заполнения питания
        private async void Button_Click_CreateEating(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Calendar.Text) && !string.IsNullOrEmpty(TypeDay.Text) &&
                    WorkClass.meal.mealid != 0)
                {
                    WorkClass.diet.dietperiod = TypeDay.Text;
                    await AddEatingAsync();
                    EatingDataGrid.Items.Clear();
                    await ReadDietAsync();
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

        //выбор блюда
        private void MealDataGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WorkClass.meal = MealDataGrid.SelectedItem as Meal;
                MealName.Text = WorkClass.meal.mealname;
                if (WorkClass.meal == null)
                {
                    throw new Exception("Блюдо не выбрано.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Button_Click_DeleteMeal(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.meal.mealid != 0)
                {
                    await DeleteMealAsync();
                    MealDataGrid.Items.Clear();
                    await ReadAllMealAsync();
                }
                else
                {
                    throw new Exception("Блюдо не выбрано");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
