using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface ILojasLogic
    {
        Task<List<LojasEntity>> ListarAsync();
        Task<LojasEntity?> ObterLoja(int id);
        Task<ResponseMessage> CriarAsync(LojasEntity entity);
        Task<ResponseMessage> AtualizarAsync(int id, LojasEntity entity);
        Task<ResponseMessage> RemoverAsync(int id);
    }

}

