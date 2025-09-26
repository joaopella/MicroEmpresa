using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IUsuariosOnlineRepository
    {
        Task<List<UsuariosOnlineEntity>> ListarAsync();
        Task<List<UsuariosOnlineEntity>> ListarPorLojaAsync(int idLoja);
        Task<UsuariosOnlineEntity?> ObterAsync(int id);
        Task<UsuariosOnlineEntity?> ObterPorLojaLoginAsync(int idLoja, string login);

        Task CriarAsync(UsuariosOnlineEntity entity);
        Task<bool> AtualizarAsync(UsuariosOnlineEntity entity);                    // full (requer Rv)
        Task<bool> AtualizarCamposAsync(UsuariosOnlineEntity entity);             // parcial (requer Rv)
        Task<bool> AtualizarSenhaAsync(int id, byte[] novaHash, byte[] rv);       // só senha (requer Rv)
        Task<bool> ExcluirAsync(int id);
    }
}
