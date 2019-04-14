using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using Trainings2.Models;

namespace Trainings2
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        int count = 0;
        int countcalories = 0;

        public MenuWindow()
        {
            InitializeComponent();
        }

        public async Task ReadFoodAsync()
        {
            string sqlExpression = "SELECT * FROM Food";
            using (SqlConnection connection = Connections.SportsmanConnection.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);

                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Food food = new Food()
                        {
                            idfood = Convert.ToInt32(reader.GetValue(0)),
                            foodname = reader.GetValue(1).ToString(),
                            protein= Convert.ToInt32(reader.GetValue(2)),
                            fat = Convert.ToInt32(reader.GetValue(3)),
                            carbohydrates = Convert.ToInt32(reader.GetValue(4)),
                            calories = Convert.ToInt32(reader.GetValue(5))
                        };

                        FoodDataGrid.Items.Add(food);
                    }
                    connection.Close();
                }
            }
        }

        public async Task AddMealAsync(string fn)
        {
            try
            {
                WorkClass.meal.mealname = MealName.Text;
                using (SqlConnection cn = Connections.SportsmanConnection.GetConnection())
                {
                    await cn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("addNewMeal", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter mealname = new SqlParameter
                    {
                        ParameterName = "@mealname",
                        Value = WorkClass.meal.mealname
                    };

                    SqlParameter foodname = new SqlParameter
                    {
                        ParameterName = "@foodname",
                        Value = fn
                    };                    

                    cmd.Parameters.Add(mealname);
                    cmd.Parameters.Add(foodname);

                    await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_AddFood(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WorkClass.food.idfood != 0)
                {
                    MealFood.Items.Insert(count, WorkClass.food.foodname);
                    countcalories = countcalories + WorkClass.food.calories;
                    CountCalories.Text = countcalories.ToString();
                    count++;
                }
                else
                {
                    throw new Exception("Не выбран продукт");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadFoodAsync().GetAwaiter();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FoodDataGrid_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                WorkClass.food = FoodDataGrid.SelectedItem as Food;
                if (WorkClass.food == null)
                {
                    throw new Exception("Продукт не выбран.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_create_meal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(MealName.Text) && MealFood.Items != null)
                {
                    for (int i = 0; i < MealFood.Items.Count; i++)
                    {
                        Food f = new Food();
                        f.foodname = (string)MealFood.Items[i];
                        AddMealAsync(f.foodname).GetAwaiter();
                    }
                }
                else
                {
                    throw new Exception("Ошибка создания");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            SportsmanInfoWindow sportsmanInfoWindow = new SportsmanInfoWindow();
            sportsmanInfoWindow.Show();
            Close();
        }
    }
}
