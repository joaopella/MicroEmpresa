using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IEnderecosRepository
    {
        Task<List<EnderecosEntity>> ListarAsync();
        Task<EnderecosEntity?> ObterAsync(int id);
        Task CriarAsync(EnderecosEntity entity);
        Task<bool> AtualizarAsync(int id, EnderecosEntity entity); // update parcial
        Task<bool> RemoverAsync(int id);
    }
}
