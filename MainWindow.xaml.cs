using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Input;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;

            miConnexionSql = new SqlConnection(miConexion);

            MuestraClientes();

            MuestraTodosPedidos();
        }

        private void MuestraClientes()
        {
            try
            {
                string consulta = "SELECT * FROM Cliente";
                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConnexionSql);
                using (miAdaptadorSql)
                {
                    DataTable clientesTabla = new DataTable();
                    miAdaptadorSql.Fill(clientesTabla);
                    listaClientes.DisplayMemberPath = "nombre";
                    listaClientes.SelectedValuePath = "Id";
                    listaClientes.ItemsSource = clientesTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MuestraPedidos()
        {
            try
            {
                string consulta = "SELECT * FROM Pedido P INNER JOIN Cliente C ON P.cCliente=C.Id " +
                                  "WHERE C.Id=@ClienteId";

                SqlCommand sqlComando = new SqlCommand(consulta, miConnexionSql);

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(sqlComando);
                using (miAdaptadorSql)
                {
                    sqlComando.Parameters.AddWithValue("@ClienteId", listaClientes.SelectedValue);
                    DataTable pedidosTabla = new DataTable();
                    miAdaptadorSql.Fill(pedidosTabla);
                    pedidosCliente.DisplayMemberPath = "fechaPedido";
                    pedidosCliente.SelectedValuePath = "Id";
                    pedidosCliente.ItemsSource = pedidosTabla.DefaultView;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MuestraTodosPedidos()
        {
            try
            {
                string consulta = "SELECT *, CONCAT(cCliente, ' ', fechaPedido, ' ', formaPago) AS infoCompleta FROM Pedido P INNER JOIN Cliente C ON P.cCliente=C.Id";

                SqlDataAdapter miAdaptadorSql = new SqlDataAdapter(consulta, miConnexionSql);
                using (miAdaptadorSql)
                {
                    DataTable pedidosTabla = new DataTable();
                    miAdaptadorSql.Fill(pedidosTabla);
                    todosPedidos.DisplayMemberPath = "infoCompleta";
                    todosPedidos.SelectedValuePath = "Id";
                    todosPedidos.ItemsSource = pedidosTabla.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        SqlConnection miConnexionSql;

        private void listaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(todosPedidos.SelectedValue.ToString());

            try
            {
                string consulta = "DELETE FROM Pedido WHERE Id=@PedidoId";
                SqlCommand miSqlCommand = new SqlCommand(consulta, miConnexionSql);
                miConnexionSql.Open();
                miSqlCommand.Parameters.AddWithValue("PedidoId", todosPedidos.SelectedValue.ToString());
                miSqlCommand.ExecuteNonQuery();
                miConnexionSql.Close();
                MuestraTodosPedidos();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "INSERT INTO Cliente (nombre) VALUES(@nombre)";
                SqlCommand miSqlCommand = new SqlCommand(consulta, miConnexionSql);
                miConnexionSql.Open();
                miSqlCommand.Parameters.AddWithValue("nombre", insertaCliente.Text);
                miSqlCommand.ExecuteNonQuery();
                miConnexionSql.Close();
                MuestraClientes();
                insertaCliente.Text = "";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "DELETE FROM Cliente WHERE Id=@ClienteId";
                SqlCommand miSqlCommand = new SqlCommand(consulta, miConnexionSql);
                miConnexionSql.Open();
                miSqlCommand.Parameters.AddWithValue("ClienteId", listaClientes.SelectedValue.ToString());
                miSqlCommand.ExecuteNonQuery();
                miConnexionSql.Close();

                MuestraClientes();
                MuestraTodosPedidos();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}