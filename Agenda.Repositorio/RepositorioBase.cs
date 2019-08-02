using Agenda.Model;
using Raven.Client;
using Raven.Client.Document;
using System.Collections.Generic;
using System.Linq;

namespace Agenda.Repositorio
{
    public class RepositorioBase<T> where T : ModelBase, new()
    {
        public DocumentStore store { get; set; }
        public RepositorioBase()
        {
            store = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "AgendaRaven"
            };

            store.Initialize();
        }

        public virtual string Cadastrar(T item)
        {
            item.Id = null;
            return Salvar(item);
        }

        public virtual string Atualizar(T item)
        {
            return Salvar(item);
        }

        public virtual string Salvar(T item)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                session.Store(item);
                session.SaveChanges();
            }

            return item.Id;
        }

        public virtual T Consulte(string idDoItem)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                return session.Load<T>(idDoItem);
            }
        }

        public List<T> ConsultePorTermo(string termo)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                return session
                    .Advanced
                    .DocumentQuery<T>()
                    .Where($"Nome:{termo}").Boost(1000)
                    .Where($"Nome:{termo}*").Boost(100)
                    .Where($"Nome:*{termo}*").Boost(10)
                    .ToList();
            }
        }

        public virtual List<T> Liste()
        {
            using (IDocumentSession session = store.OpenSession())
            {
                return session.Query<T>().ToList();
            }
        }

        //public virtual List<T> Liste(int pagina, int elemtosPorPagina, out RavenQueryStatistics estatisticas)
        //{
        //    var quantidadeAPular = (pagina - 1) * elemtosPorPagina;

        //    //using (IDocumentSession session = store.OpenSession())
        //    //{
        //    //    //return session
        //    //    //    .Query<T>()
        //    //    //    .Statistics(out estatisticas)
        //    //    //    .OrderBy(x => x.Descricao)
        //    //    //    .Skip(quantidadeAPular)
        //    //    //    .Take(elemtosPorPagina)
        //    //    //    .ToList();
        //    //    return session.ToString();
        //    //}
        //}

        public virtual void Remover(string idDoItemSalvo)
        {
            using (IDocumentSession session = store.OpenSession())
            {
                session.Delete(idDoItemSalvo);
                session.SaveChanges();
            }
        }
    }
}
  