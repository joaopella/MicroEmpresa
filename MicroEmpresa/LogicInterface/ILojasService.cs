using MicroEmpresa.Entity;

namespace MicroEmpresa.Logic.Lojas;

public interface ILojasService
{
    Task<List<LojasEntity>> ListarAsync();
    Task<LojasEntity?> ObterLoja(int id);
    Task<LojasEntity> CriarAsync(LojasEntity input);
    Task<bool> AtualizarAsync(int id, LojasEntity input);
    Task<bool> RemoverAsync(int id);
}
