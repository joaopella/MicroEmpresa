using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface ICaixasRepository
    {
        Task<List<CaixasEntity>> ListarAsync();
        Task<CaixasEntity?> ObterAsync(int id);
        Task<CaixasEntity?> ObterAbertoPorLojaAsync(int idLoja);

        Task<int> AbrirAsync(CaixasEntity entity);                  // retorna Id criado
        Task<bool> FecharAsync(int id, byte[] rv, int idFunc, decimal valorFechamento, DateTime dataFechamento);
        Task<bool> AtualizarObsAsync(int id, byte[] rv, string? obs);

        Task<bool> ExcluirAsync(int id);
    }
}
