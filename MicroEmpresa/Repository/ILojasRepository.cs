using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository;

public interface ILojasRepository
{
    Task<List<LojasEntity>> ListarAsync();
    Task<LojasEntity?> ObterAsync(int id);
    Task<bool> CnpjExisteAsync(string cnpj);
    Task<bool> TemDependenciasAsync(int id);

    Task<LojasEntity> CriarAsync(LojasEntity entity);
    Task<LojasEntity> AtualizarAsync(int id, LojasEntity entity);
    Task<bool> RemoverAsync(int id);
}
