using Raven.Client;
using Agenda.Model;
using Agenda.Repositorio;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Agenda
    {
    
    public partial class WinPrincipal : Window
    {
        const int QUANTIDADE_POR_PAGINA = 10;

        public string IdDoClienteSalvo { get; set; }
        public RepositorioDeCliente Repositorio { get; set; }
        public int PaginaAtual { get; set; } = 1;
        public int QuantidadeTotalDePaginas { get; set; }

        public WinPrincipal()
        {
            InitializeComponent();
            DataContext = new WindowViewModel(this);
            Repositorio = new RepositorioDeCliente();
            CarregueElementosDoBancoDeDados();
        }

        private void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            var contato = ChameOEditorDeCliente(new ModelContato());
            Repositorio.Cadastrar(contato);
            CarregueElementosDoBancoDeDados();
        }

        private void CarregueElementosDoBancoDeDados()
        {
            RavenQueryStatistics estatisticas;
            lstClientes.DataContext = Repositorio.Liste(PaginaAtual, QUANTIDADE_POR_PAGINA, out estatisticas);
            QuantidadeTotalDePaginas = (int)Math.Ceiling((decimal)estatisticas.TotalResults / (decimal)QUANTIDADE_POR_PAGINA);
            txtPaginaAtual.Text = $"Página {PaginaAtual} de {QuantidadeTotalDePaginas}; Total de elmentos: {estatisticas.TotalResults}; Duração da consulta: {estatisticas.DurationMilliseconds} ms";
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (lstClientes.SelectedItem == null)
            {
                MessageBox.Show("Selecione um item na lista!");
                return;
            }

            var contato = (ModelContato)lstClientes.SelectedItem;

            contato = Repositorio.Consulte(contato.Id);

            ChameOEditorDeCliente(contato);

            Repositorio.Atualizar(contato);
            CarregueElementosDoBancoDeDados();
        }

        private ModelContato ChameOEditorDeCliente(ModelContato contato)
        {
            var winContato = new WinContato(contato);
            winContato.ShowDialog();
            return winContato.contato;
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (lstClientes.SelectedItem == null)
            {
                MessageBox.Show("Selecione um item na lista!");
                return;
            }

            var cliente = ((ModelContato)(lstClientes.SelectedItem));
            Repositorio.Remover(cliente.Id);
            CarregueElementosDoBancoDeDados();
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            CarregueElementosDoBancoDeDados();
        }

        private void txtConsulta_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConsulta.Text))
            {
                CarregueElementosDoBancoDeDados();
                return;
            }

            lstClientes.DataContext = Repositorio.ConsultePorTermo(txtConsulta.Text);
        }

        private void txtIdade_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtIdade.Text))
            {
                CarregueElementosDoBancoDeDados();
                return;
            }

             
            else
            {
                lstClientes.DataContext = new List<ModelContato>();
            }
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (PaginaAtual > 1)
            {
                PaginaAtual--;
            }

            CarregueElementosDoBancoDeDados();
        }

        private void btnProximo_Click(object sender, RoutedEventArgs e)
        {
            if (PaginaAtual < QuantidadeTotalDePaginas)
            {
                PaginaAtual++;
            }

            CarregueElementosDoBancoDeDados();
        }
    }
}
