using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface ICaixasLogic
    {
        Task<List<CaixasEntity>> ListarAsync();
        Task<CaixasEntity?> ObterAsync(int id);
        Task<CaixasEntity?> ObterAbertoPorLojaAsync(int idLoja);

        Task<ResponseMessage> AbrirAsync(CaixasEntity entity);
        Task<ResponseMessage> FecharAsync(int id, byte[] rv, int idFuncionarioFechamento, decimal valorFechamento, DateTime? dataFechamento = null);
        Task<ResponseMessage> AtualizarObsAsync(int id, byte[] rv, string? obs);

        Task<ResponseMessage> ExcluirAsync(int id);
    }
}
