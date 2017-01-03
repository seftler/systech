using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;



namespace Systems_Technologies
{
    public partial class MainPage : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-IM4NPBC\\SQLEXPRESS;Initial Catalog=energy;Integrated Security=True"); //строка подключения

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDateStart.Text = Calendar.SelectedDate.ToShortDateString(); // отображение выбранной даты календаря
            TextBoxDateEnd.Text = Calendar.SelectedDate.AddDays(1).ToShortDateString();
        }

        protected void SelectButton_Click(object sender, EventArgs e)
        {
            String select_string = "select CONVERT( VARCHAR( 5 ), value_dt, 108) as Временная_отметка, sum(case parameter_id when '1' then ROUND(value, 3) else 0 end) as Активная_прием, sum(case parameter_id when '2' then ROUND(value, 3) else 0 end) as Активная_отдача, sum(case parameter_id when '3' then ROUND(value, 3) else 0 end) as Реактивная_прием, sum(case parameter_id when '4' then ROUND(value, 3) else 0 end) as Реактивная_отдача from measuring_values where (value_dt > @search_start) and (value_dt <= @search_end) and (meter_id = @id_station) group by value_dt"; // строка select - запроса
            SqlCommand sql_command = new SqlCommand(select_string, connection);

            int id_station=0;
            if (DropDownListStation.Text == "ПС Восточная, Фидер 1") // выбор прибора измерений
            {
                id_station = 1;
            }
            if (DropDownListStation.Text == "ПС Восточная, Фидер 2")
            {
                id_station = 2;
            }

            sql_command.Parameters.Add("@id_station", SqlDbType.Int).Value = id_station.ToString(); // определение параметров для подстановки в select - запрос
            sql_command.Parameters.Add("@search_start", SqlDbType.DateTime2).Value = TextBoxDateStart.Text;
            sql_command.Parameters.Add("@search_end", SqlDbType.DateTime2).Value = TextBoxDateEnd.Text;

            
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            SqlDataAdapter data_adapter = new SqlDataAdapter(); //связь между DataSet и SQL Server
            data_adapter.SelectCommand = sql_command;
            DataSet data_set = new DataSet(); //извлечение и сохранение данных
            data_adapter.Fill(data_set, "value_dt");
            GridView.DataSource = data_set; //отображение данных
            GridView.DataBind();
            connection.Close();

        }
    }
}