using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Windows;

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

        private void MuestraPedidos()
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

        private void MuestraTodosPedidos()
        {
            string consulta = "SELECT CONCAT(cCliente, ' ', fechaPedido, ' ', formaPago) AS infoCompleta FROM Pedido P INNER JOIN Cliente C ON P.cCliente=C.Id";

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

        SqlConnection miConnexionSql;

        private void listaClientes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MuestraPedidos();
        }
    }
}