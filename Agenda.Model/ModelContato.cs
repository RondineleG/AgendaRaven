namespace Agenda.Model
    {
    public class ModelContato : ModelBase

        {
        public ModelContato()
            {
            Endereco = new ModelEndereco();

            }
        public int Codigo { get; set; }
        public ModelEndereco Endereco { get; set; }
        public string Nome { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneFixo { get; set; }
        public string Email { get; set; }

        }
    }

