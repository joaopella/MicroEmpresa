using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IClientesRepository
    {
        Task<List<ClientesEntity>> ListarAsync();
        Task<ClientesEntity?> ObterAsync(int id);
        Task CriarAsync(ClientesEntity entity);
        Task<bool> AtualizarAsync(int id, ClientesEntity entity);
        Task<bool> RemoverAsync(int id);
    }
}
