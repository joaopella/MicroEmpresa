using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IFuncionariosRepository
    {
        Task<List<FuncionariosEntity>> ListarAsync();
        Task<FuncionariosEntity?> ObterAsync(int id);
        Task CriarAsync(FuncionariosEntity entity);
        Task<bool> AtualizarAsync(int id, FuncionariosEntity entity);
        Task<bool> RemoverAsync(int id);

        Task<bool> CpfExisteAsync(string cpf, int? ignoreId = null);
    }
}
