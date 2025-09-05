using Microsoft.EntityFrameworkCore;
using MicroEmpresa.Date;
using MicroEmpresa.Entity;

namespace MicroEmpresa.Repository;

public class LojasData : ILojasRepository
{
    private readonly AppDbContext _db;
    public LojasData(AppDbContext db) => _db = db;

    public Task<List<LojasEntity>> ListarAsync() =>
        _db.Lojas.AsNoTracking().OrderBy(l => l.NomeFantasia).ToListAsync();

    public Task<LojasEntity?> ObterAsync(int id) =>
        _db.Lojas.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);

    public Task<bool> CnpjExisteAsync(string cnpj)
    {
        return _db.Lojas.AsNoTracking().AnyAsync(l => l.Cnpj == cnpj);
    }


    public Task<bool> TemDependenciasAsync(int id) =>
        Task.FromResult(
            _db.Produtos.Any(p => p.IdLoja == id) ||
            _db.Funcionarios.Any(f => f.IdLoja == id) ||
            _db.Clientes.Any(c => c.IdLoja == id));

    public async Task<LojasEntity> CriarAsync(LojasEntity entity)
    {
        entity.CriadoEm = DateTime.UtcNow;
        _db.Lojas.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<LojasEntity> AtualizarAsync(int id, LojasEntity entity)
    {
        var atual = await _db.Lojas.FirstOrDefaultAsync(l => l.Id == id);
        if (atual is null) throw new KeyNotFoundException("Loja não encontrada.");

        atual.NomeFantasia = entity.NomeFantasia;
        atual.Cnpj = entity.Cnpj;
        atual.Telefone = entity.Telefone;
        atual.AtualizadoEm = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return atual;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var atual = await _db.Lojas.FirstOrDefaultAsync(l => l.Id == id);
        if (atual is null) return false;

        _db.Lojas.Remove(atual);
        await _db.SaveChangesAsync();
        return true;
    }
}
