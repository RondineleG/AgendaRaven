using Agenda.Model;
using Agenda.Repositorio;
using System.Windows;

namespace Agenda
    {
    public partial class WinContato : Window
        {
        public ModelContato contato { get; set; }

        RepositorioDeCliente repositorio;

        public WinContato()
            {
            InitializeComponent();
            contato = new ModelContato();
            this.DataContext = contato;
            repositorio = new RepositorioDeCliente();
            }

        public WinContato(ModelContato cliente)
            {
            InitializeComponent();
            this.DataContext = cliente;
            contato = cliente;
            repositorio = new RepositorioDeCliente();

            }

        private void Window_Loaded(object sender , RoutedEventArgs e)
            {
            repositorio.Liste();
            }
        
            private void BtnCancelar_Click(System.Object sender , RoutedEventArgs e)
            {
            Close();
            }

        private void BtnSalvar_Click(System.Object sender , RoutedEventArgs e)
            {
            contato = (ModelContato)this.DataContext;

            this.Close();
            }
        }
    }
