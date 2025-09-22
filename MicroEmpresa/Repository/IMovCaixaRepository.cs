using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IMovCaixaRepository
    {
        Task<List<MovCaixaEntity>> ListarPorCaixaAsync(int idCaixa);
        Task<MovCaixaEntity?> ObterAsync(int id);

        Task<int> CriarAsync(MovCaixaEntity entity);
        Task<bool> AtualizarDescricaoAsync(int id, byte[] rv, string? descricao);
        Task<bool> ExcluirAsync(int id);
    }
}
