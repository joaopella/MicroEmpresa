using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class UsuariosLojaData : IUsuariosLojaRepository
    {
        private readonly AppDbContext _db;
        public UsuariosLojaData(AppDbContext db) => _db = db;

        public Task<UsuariosLojaEntity?> GetByIdAsync(int id)
            => _db.UsuariosLoja.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public Task<UsuariosLojaEntity?> GetByLoginAsync(int idLoja, string login)
            => _db.UsuariosLoja.FirstOrDefaultAsync(x => x.IdLoja == idLoja && x.Login == login);

        public async Task<IReadOnlyList<UsuariosLojaEntity>> ListAsync(
            int? idLoja = null, int? idFuncionario = null, string? q = null, int skip = 0, int take = 50)
        {
            var query = _db.UsuariosLoja.AsNoTracking().AsQueryable();

            if (idLoja.HasValue)
                query = query.Where(x => x.IdLoja == idLoja.Value);

            if (idFuncionario.HasValue)
                query = query.Where(x => x.IdFuncionario == idFuncionario.Value);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var like = $"%{q}%";
                query = query.Where(x =>
                    EF.Functions.Like(x.Login, like) ||
                    (x.Email != null && EF.Functions.Like(x.Email, like)));
            }

            return await query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public Task<int> CountAsync(int? idLoja = null, int? idFuncionario = null, string? q = null)
        {
            var query = _db.UsuariosLoja.AsQueryable();

            if (idLoja.HasValue)
                query = query.Where(x => x.IdLoja == idLoja.Value);

            if (idFuncionario.HasValue)
                query = query.Where(x => x.IdFuncionario == idFuncionario.Value);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var like = $"%{q}%";
                query = query.Where(x =>
                    EF.Functions.Like(x.Login, like) ||
                    (x.Email != null && EF.Functions.Like(x.Email, like)));
            }

            return query.CountAsync();
        }

        public async Task<UsuariosLojaEntity> AddAsync(UsuariosLojaEntity entity)
        {
            _db.UsuariosLoja.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(UsuariosLojaEntity entity)
        {
            _db.UsuariosLoja.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(UsuariosLojaEntity entity)
        {
            _db.UsuariosLoja.Remove(entity);
            await _db.SaveChangesAsync();
        }

        // ----------------------------
        // MÉTODOS PARA BUSCAR POR CNPJ
        // ----------------------------

        // 1) Pegar o ID da loja pelo CNPJ (retorna null se não achar ou CNPJ inválido)
        public async Task<int> BuscarIdLojaPorCnpjAsync(string cnpj)
        {
            var c = SomenteDigitos(cnpj);

            if (c.Length != 14)
            {
                return 0; ;
            }

            var id = await _db.Lojas
                .AsNoTracking()
                .Where(x => x.Cnpj == c)   // <- lambda
                .Select(x => x.Id)
                .FirstOrDefaultAsync();     // 0 se não houver

            return id == 0 ? 0 : id;
        }

        // 2) Apenas verificar se existe loja com esse CNPJ
        public async Task<bool> ExisteCnpjAsync(string cnpj)
        {
            var c = SomenteDigitos(cnpj);
            if (c.Length != 14) return false;

            return await _db.Lojas
                .AsNoTracking()
                .AnyAsync(x => x.Cnpj == c); // <- lambda
        }

        private static string SomenteDigitos(string? s)
            => string.IsNullOrEmpty(s) ? string.Empty : new string(s.Where(char.IsDigit).ToArray());
    }
}

