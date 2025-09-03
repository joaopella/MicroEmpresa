using MicroEmpresa.Entity;

namespace MicroEmpresa.Logic.Lojas;

public interface ILojasService
{
    Task<List<LojasEntity>> ListarAsync();
    Task<LojasEntity?> ObterAsync(int id);
    Task<LojasEntity> CriarAsync(LojasEntity input);
    Task<LojasEntity> AtualizarAsync(int id, LojasEntity input);
    Task<bool> RemoverAsync(int id);
}
