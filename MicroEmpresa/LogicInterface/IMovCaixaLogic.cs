using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IMovCaixaLogic
    {
        Task<List<MovCaixaEntity>> ListarPorCaixaAsync(int idCaixa);
        Task<MovCaixaEntity?> ObterAsync(int id);

        Task<ResponseMessage> CriarAsync(MovCaixaEntity entity);
        Task<ResponseMessage> AtualizarDescricaoAsync(int id, byte[] rv, string? descricao);
        Task<ResponseMessage> ExcluirAsync(int id);
    }
}
