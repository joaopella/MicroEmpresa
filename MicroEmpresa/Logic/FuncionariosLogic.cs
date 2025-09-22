using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic
{
    public class FuncionariosLogic : IFuncionariosLogic
    {
        private readonly IFuncionariosRepository _repo;
        private readonly ILojasRepository _lojasRepo;

        public FuncionariosLogic(IFuncionariosRepository repo, ILojasRepository lojasRepo)
        {
            _repo = repo;
            _lojasRepo = lojasRepo;
        }

        public Task<List<FuncionariosEntity>> ListarAsync() => _repo.ListarAsync();

        public Task<FuncionariosEntity?> ObterAsync(int id) =>
            id <= 0 ? Task.FromResult<FuncionariosEntity?>(null) : _repo.ObterAsync(id);

        public async Task<ResponseMessage> CriarAsync(FuncionariosEntity entity)
        {
            var f = Sanitizar(entity);

            if (f.IdLoja <= 0)
                return new ResponseMessage { Message = "Loja é obrigatória." };

            _ = await _lojasRepo.ObterAsync(f.IdLoja) ?? null; // se quiser validar existência, mude para msg

            if (string.IsNullOrWhiteSpace(f.Nome))
                return new ResponseMessage { Message = "Nome é obrigatório." };

            if (!string.IsNullOrWhiteSpace(f.Cpf))
            {
                f.Cpf = SomenteDigitos(f.Cpf);
                if (f.Cpf.Length != 11)
                    return new ResponseMessage { Message = "CPF deve conter 11 dígitos." };

                if (await _repo.CpfExisteAsync(f.Cpf))
                    return new ResponseMessage { Message = "Já existe um funcionário com este CPF." };
            }

            if (!string.IsNullOrWhiteSpace(f.Telefone))
                f.Telefone = SomenteDigitos(f.Telefone);

            await _repo.CriarAsync(f);
            return new ResponseMessage { Message = "Funcionário cadastrado com sucesso!" };
        }

        public async Task<ResponseMessage> AtualizarAsync(int id, FuncionariosEntity entity)
        {
            var existe = await _repo.ObterAsync(id);
            if (existe is null) return new ResponseMessage { Message = "Funcionário não encontrado." };

            var f = Sanitizar(entity);

            if (f.IdLoja > 0)
                _ = await _lojasRepo.ObterAsync(f.IdLoja) ?? null;

            if (!string.IsNullOrWhiteSpace(f.Cpf))
            {
                f.Cpf = SomenteDigitos(f.Cpf);
                if (f.Cpf.Length != 11)
                    return new ResponseMessage { Message = "CPF deve conter 11 dígitos." };

                if (await _repo.CpfExisteAsync(f.Cpf, ignoreId: id))
                    return new ResponseMessage { Message = "Já existe um funcionário com este CPF." };
            }

            if (!string.IsNullOrWhiteSpace(f.Telefone))
                f.Telefone = SomenteDigitos(f.Telefone);

            var ok = await _repo.AtualizarAsync(id, f);
            return new ResponseMessage { Message = ok ? "Funcionário atualizado com sucesso!" : "Nada foi alterado." };
        }

        public async Task<ResponseMessage> RemoverAsync(int id)
        {
            if (id <= 0) return new ResponseMessage { Message = "ID inválido." };

            var ok = await _repo.RemoverAsync(id);
            return new ResponseMessage { Message = ok ? "Funcionário removido com sucesso!" : "Funcionário não encontrado." };
        }

        // helpers
        private static FuncionariosEntity Sanitizar(FuncionariosEntity f) => new()
        {
            Id = f.Id,
            IdLoja = f.IdLoja,
            Nome = f.Nome?.Trim(),
            Cpf = f.Cpf?.Trim(),
            Email = f.Email?.Trim(),
            Telefone = f.Telefone?.Trim()
        };

        private static string SomenteDigitos(string s) => new(s.Where(char.IsDigit).ToArray());
    }
}
