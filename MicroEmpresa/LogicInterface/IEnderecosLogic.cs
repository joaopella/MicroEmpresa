using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IEnderecosLogic
    {
        Task<List<EnderecosEntity>> ListarAsync();
        Task<EnderecosEntity> ObterAsync(int id);
        Task CriarAsync(EnderecosEntity entity);
        Task<bool> AtualizarAsync(int id, EnderecosEntity entity);
        Task RemoverAsync(int id);
    }
}
