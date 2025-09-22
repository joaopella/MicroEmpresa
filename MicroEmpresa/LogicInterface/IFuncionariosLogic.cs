using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IFuncionariosLogic
    {
        Task<List<FuncionariosEntity>> ListarAsync();
        Task<FuncionariosEntity?> ObterAsync(int id);
        Task<ResponseMessage> CriarAsync(FuncionariosEntity entity);
        Task<ResponseMessage> AtualizarAsync(int id, FuncionariosEntity entity);
        Task<ResponseMessage> RemoverAsync(int id);
    }
}
