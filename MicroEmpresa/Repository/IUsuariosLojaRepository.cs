using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository
{
    public interface IUsuariosLojaRepository
    {
        Task<UsuariosLojaEntity?> GetByIdAsync(int id);
        Task<UsuariosLojaEntity?> GetByLoginAsync(int idLoja, string login);

        Task<IReadOnlyList<UsuariosLojaEntity>> ListAsync(
            int? idLoja = null, int? idFuncionario = null, string? q = null, int skip = 0, int take = 50);

        Task<int> CountAsync(
            int? idLoja = null, int? idFuncionario = null, string? q = null);

        Task<UsuariosLojaEntity> AddAsync(UsuariosLojaEntity entity);
        Task UpdateAsync(UsuariosLojaEntity entity);
        Task DeleteAsync(UsuariosLojaEntity entity);
        public Task<int> BuscarIdLojaPorCnpjAsync(string cnpj);

        public Task<bool> ExisteCnpjAsync(string cnpj);
    }
}
