using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class FuncionariosData : IFuncionariosRepository
    {
        private readonly AppDbContext _db;
        public FuncionariosData(AppDbContext db) => _db = db;

        public Task<List<FuncionariosEntity>> ListarAsync() =>
            _db.Funcionarios.AsNoTracking()
               .OrderBy(f => f.Nome)
               .ToListAsync();

        public Task<FuncionariosEntity?> ObterAsync(int id) =>
            _db.Funcionarios.AsNoTracking()
               .FirstOrDefaultAsync(f => f.Id == id);

        public async Task CriarAsync(FuncionariosEntity entity)
        {
            entity.CriadoEm = DateTime.UtcNow;
            _db.Funcionarios.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> AtualizarAsync(int id, FuncionariosEntity entity)
        {
            entity.Id = id;
            entity.AtualizadoEm = DateTime.UtcNow;

            _db.Attach(entity);

            if (!string.IsNullOrWhiteSpace(entity.Nome))
                _db.Entry(entity).Property(x => x.Nome).IsModified = true;

            if (!string.IsNullOrWhiteSpace(entity.Cpf))
                _db.Entry(entity).Property(x => x.Cpf).IsModified = true;

            if (!string.IsNullOrWhiteSpace(entity.Email))
                _db.Entry(entity).Property(x => x.Email).IsModified = true;

            if (!string.IsNullOrWhiteSpace(entity.Telefone))
                _db.Entry(entity).Property(x => x.Telefone).IsModified = true;

            if (entity is { /* se tiver outros campos opcionais, marque aqui */ })
            {
                // Exemplo: Cargo, Salario, etc...
                // _db.Entry(entity).Property(x => x.Cargo).IsModified = true;
            }

            _db.Entry(entity).Property(x => x.AtualizadoEm).IsModified = true;

            var linhas = await _db.SaveChangesAsync();
            return linhas > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var atual = await _db.Funcionarios.FirstOrDefaultAsync(f => f.Id == id);
            if (atual is null) return false;

            _db.Funcionarios.Remove(atual);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<bool> CpfExisteAsync(string cpf, int? ignoreId = null)
        {
            var q = _db.Funcionarios.AsNoTracking().Where(f => f.Cpf == cpf);
            if (ignoreId is not null) q = q.Where(f => f.Id != ignoreId);
            return q.AnyAsync();
        }

        // ----------------------------
        // MÉTODOS PARA BUSCAR POR CNPJ
        // ----------------------------

        // 1) Pegar o ID da loja pelo CNPJ (retorna null se não achar ou CNPJ inválido)
        public async Task<int> BuscarIdLojaPorCnpjAsync(string cnpj)
        {
            if (cnpj.Length != 14)
            {
                return 0; ;
            }

            var id = await _db.Lojas
                .AsNoTracking()
                .Where(x => x.Cnpj == cnpj)   // <- lambda
                .Select(x => x.Id)
                .FirstOrDefaultAsync();     // 0 se não houver

            return id == 0 ? 0 : id;
        }

        // 2) Apenas verificar se existe loja com esse CNPJ
        public async Task<bool> ExisteCnpjAsync(string cnpj)
        {
            if (cnpj.Length != 14) return false;

            return await _db.Lojas
                .AsNoTracking()
                .AnyAsync(x => x.Cnpj == cnpj); // <- lambda
        }
    }
}
