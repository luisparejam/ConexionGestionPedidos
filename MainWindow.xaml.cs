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

        SqlConnection miConnexionSql;
    }
}