using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IClientesLogic
    {
        Task<List<ClientesEntity>> ListarAsync();
        Task<ClientesEntity> ObterAsync(int id);
        Task CriarAsync(ClientesEntity entity);
        Task<bool> AtualizarAsync(int id, ClientesEntity entity);
        Task RemoverAsync(int id);
    }
}
