using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class ClientesService : IClientesService
    {
        private readonly IClientesRepository _repo;
        public ClientesService(IClientesRepository repo) => _repo = repo;

        public Task<List<ClientesEntity>> ListarAsync() => _repo.ListarAsync();

        public async Task<ClientesEntity> ObterAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            var c = await _repo.ObterAsync(id);
            return c ?? throw new KeyNotFoundException("Cliente não encontrado.");
        }

        public async Task CriarAsync(ClientesEntity entity)
        {
            var c = Sanitizar(entity);

            if (string.IsNullOrWhiteSpace(c.Nome))
                throw new ArgumentException("Nome é obrigatório.");
            if (!string.IsNullOrWhiteSpace(c.Cpf))
            {
                c.Cpf = SomenteDigitos(c.Cpf);
                if (c.Cpf.Length is not (11 or 14))
                    throw new ArgumentException("CPF/CNPJ inválido.");
            }

            await _repo.CriarAsync(c);
        }

        public async Task<bool> AtualizarAsync(int id, ClientesEntity entity)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            _ = await _repo.ObterAsync(id) ?? throw new KeyNotFoundException("Cliente não encontrado.");

            var c = Sanitizar(entity);

            if (!string.IsNullOrWhiteSpace(c.Cpf))
            {
                c.Cpf = SomenteDigitos(c.Cpf);
                if (c.Cpf.Length is not (11 or 14))
                    throw new ArgumentException("CPF/CNPJ inválido.");
            }

            return await _repo.AtualizarAsync(id, c);
        }

        public async Task RemoverAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            var ok = await _repo.RemoverAsync(id);
            if (!ok) throw new KeyNotFoundException("Cliente não encontrado.");
        }

        // Helpers
        private static ClientesEntity Sanitizar(ClientesEntity c) => new()
        {
            Id = c.Id,
            Nome = c.Nome?.Trim(),
            Cpf = c.Cpf?.Trim(),
            Email = c.Email?.Trim(),
            Telefone = c.Telefone?.Trim()
        };

        private static string SomenteDigitos(string s) =>
            new(s.Where(char.IsDigit).ToArray());
    }
}
