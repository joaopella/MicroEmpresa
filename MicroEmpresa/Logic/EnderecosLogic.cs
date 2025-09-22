using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class EnderecosLogic : IEnderecosLogic
    {
        private readonly IEnderecosRepository _repo;
        public EnderecosLogic(IEnderecosRepository repo) => _repo = repo;

        public Task<List<EnderecosEntity>> ListarAsync() => _repo.ListarAsync();

        public async Task<EnderecosEntity> ObterAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            var e = await _repo.ObterAsync(id);
            return e ?? throw new KeyNotFoundException("Endereço não encontrado.");
        }

        public async Task CriarAsync(EnderecosEntity entity)
        {
            var e = Sanitizar(entity);

            if (string.IsNullOrWhiteSpace(e.Logradouro))
                throw new ArgumentException("Logradouro é obrigatório.");
            if (string.IsNullOrWhiteSpace(e.Cidade))
                throw new ArgumentException("Cidade é obrigatória.");
            if (string.IsNullOrWhiteSpace(e.Uf) || e.Uf.Length != 2)
                throw new ArgumentException("UF deve conter 2 letras.");

            if (!string.IsNullOrWhiteSpace(e.Cep))
            {
                e.Cep = SomenteDigitos(e.Cep);
                if (e.Cep.Length != 8)
                    throw new ArgumentException("CEP deve conter 8 dígitos.");
            }

            await _repo.CriarAsync(e);
        }

        public async Task<bool> AtualizarAsync(int id, EnderecosEntity entity)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            _ = await _repo.ObterAsync(id) ?? throw new KeyNotFoundException("Endereço não encontrado.");

            var e = Sanitizar(entity);

            if (e.Uf is not null && e.Uf.Length != 2)
                throw new ArgumentException("UF deve conter 2 letras.");
            if (!string.IsNullOrWhiteSpace(e.Cep))
            {
                e.Cep = SomenteDigitos(e.Cep);
                if (e.Cep.Length != 8)
                    throw new ArgumentException("CEP deve conter 8 dígitos.");
            }

            return await _repo.AtualizarAsync(id, e);
        }

        public async Task RemoverAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");
            var ok = await _repo.RemoverAsync(id);
            if (!ok) throw new KeyNotFoundException("Endereço não encontrado.");
        }

        // Helpers
        private static EnderecosEntity Sanitizar(EnderecosEntity e) => new()
        {
            Id = e.Id,
            IdCliente = e.IdCliente,
            IdLoja = e.IdLoja,
            Tipo = e.Tipo?.Trim(),
            Logradouro = e.Logradouro?.Trim() ?? string.Empty,
            Numero = e.Numero?.Trim(),
            Complemento = e.Complemento?.Trim(),
            Bairro = e.Bairro?.Trim(),
            Cidade = e.Cidade?.Trim() ?? string.Empty,
            Uf = e.Uf?.Trim().ToUpperInvariant() ?? string.Empty,
            Cep = e.Cep?.Trim()
        };

        private static string SomenteDigitos(string s) =>
            new(s.Where(char.IsDigit).ToArray());
    }
}
