using MicroEmpresa.Entity;

namespace MicroEmpresa.LogicInterface
{
    public interface IUsuariosLojaLogic
    {
        Task<UsuariosLojaEntity?> GetAsync(int id);
        Task<(IReadOnlyList<UsuariosLojaEntity> Items, int Total)> ListAsync(int? idLoja = null, int? idFuncionario = null, string? q = null, int page = 1, int pageSize = 50);

        Task<ResponseMessage> CreateAsync(UsuariosLojaEntity entity);
        Task<UsuariosLojaEntity?> UpdateAsync(UsuariosLojaEntity entity, byte[]? rv);
        Task<bool> DeleteAsync(int id, byte[]? rv);

        Task<UsuariosLojaEntity?> LoginAsync(int idLoja, string login, string senhaPura);
        Task<bool> ChangePasswordAsync(int id, string senhaAtual, string novaSenha, byte[]? rv);
    }
}
