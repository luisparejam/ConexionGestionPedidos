using System.Configuration;
using System.Data.SqlClient;
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


        }

        private void MuestraClientes()
        {
            string consulta = "SELECT * FROM Cliente";

        }

        SqlConnection miConnexionSql;
    }
}