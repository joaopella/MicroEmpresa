using MicroEmpresa.Entity;
using MicroEmpresa.Repository;
using Microsoft.EntityFrameworkCore;

namespace MicroEmpresa.Date
{
    public class UsuariosOnlineData : IUsuariosOnlineRepository
    {
        private readonly AppDbContext _ctx;
        public UsuariosOnlineData(AppDbContext ctx) => _ctx = ctx;

        public Task<List<UsuariosOnlineEntity>> ListarAsync() =>
            _ctx.Set<UsuariosOnlineEntity>().AsNoTracking()
                .Include(x => x.Loja)
                .Include(x => x.Cliente)
                .ToListAsync();

        public Task<List<UsuariosOnlineEntity>> ListarPorLojaAsync(int idLoja) =>
            _ctx.Set<UsuariosOnlineEntity>().AsNoTracking()
                .Where(x => x.IdLoja == idLoja)
                .Include(x => x.Loja)
                .Include(x => x.Cliente)
                .ToListAsync();

        public Task<UsuariosOnlineEntity?> ObterAsync(int id) =>
            _ctx.Set<UsuariosOnlineEntity>()
                .Include(x => x.Loja)
                .Include(x => x.Cliente)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<UsuariosOnlineEntity?> ObterPorLojaLoginAsync(int idLoja, string login) =>
            _ctx.Set<UsuariosOnlineEntity>()
                .Include(x => x.Loja)
                .Include(x => x.Cliente)
                .FirstOrDefaultAsync(x => x.IdLoja == idLoja && x.Login == login);

        public async Task CriarAsync(UsuariosOnlineEntity e)
        {
            _ctx.Set<UsuariosOnlineEntity>().Add(e);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> AtualizarAsync(UsuariosOnlineEntity e)
        {
            var tracked = await _ctx.Set<UsuariosOnlineEntity>().FirstOrDefaultAsync(x => x.Id == e.Id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = e.Rv;

            tracked.IdLoja = e.IdLoja;
            tracked.IdCliente = e.IdCliente;
            tracked.Login = e.Login;
            tracked.SenhaHash = e.SenhaHash;
            tracked.Email = e.Email;
            tracked.Nome = e.Nome;
            tracked.Ativo = e.Ativo;
            tracked.AtualizadoEm = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtualizarCamposAsync(UsuariosOnlineEntity e)
        {
            var tracked = await _ctx.Set<UsuariosOnlineEntity>().FirstOrDefaultAsync(x => x.Id == e.Id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = e.Rv;

            if (e.IdLoja > 0) tracked.IdLoja = e.IdLoja;
            tracked.IdCliente = e.IdCliente; // pode ser nulo

            if (!string.IsNullOrWhiteSpace(e.Login)) tracked.Login = e.Login;
            if (e.SenhaHash is { Length: > 0 }) tracked.SenhaHash = e.SenhaHash;
            if (!string.IsNullOrWhiteSpace(e.Email)) tracked.Email = e.Email;
            if (!string.IsNullOrWhiteSpace(e.Nome)) tracked.Nome = e.Nome;

            // bool: aplica o valor atual recebido
            tracked.Ativo = e.Ativo;

            tracked.AtualizadoEm = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AtualizarSenhaAsync(int id, byte[] novaHash, byte[] rv)
        {
            var tracked = await _ctx.Set<UsuariosOnlineEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (tracked is null) return false;

            _ctx.Entry(tracked).Property(p => p.Rv).OriginalValue = rv;
            tracked.SenhaHash = novaHash;
            tracked.AtualizadoEm = DateTime.UtcNow;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var e = await _ctx.Set<UsuariosOnlineEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return false;

            _ctx.Remove(e);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
