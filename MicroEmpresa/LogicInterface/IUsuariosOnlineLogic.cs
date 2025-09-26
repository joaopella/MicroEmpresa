using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IUsuariosOnlineLogic
    {
        Task<List<UsuariosOnlineEntity>> ListarAsync();
        Task<List<UsuariosOnlineEntity>> ListarPorLojaAsync(int idLoja);
        Task<UsuariosOnlineEntity?> ObterAsync(int id);
        Task<UsuariosOnlineEntity?> ObterPorLojaLoginAsync(int idLoja, string login);
        Task<ResponseMessage> CriarAsync(UsuariosOnlineEntity entity);
        Task<ResponseMessage> AtualizarAsync(UsuariosOnlineEntity entity);
        Task<ResponseMessage> AtualizarSenhaAsync(int id, byte[] novaHash, byte[] rv);
        Task<ResponseMessage> ExcluirAsync(int id);
    }
}
