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

        public async Task<ResponseMessage> CriarAsync(FuncionariosEntity funcionariosEntity)
        {
            var funcionario = Sanitizar(funcionariosEntity);

            if (string.IsNullOrEmpty(funcionario.Nome))
            {
                return new ResponseMessage { Message = "Nome é obrigatório." };
            }

            if (string.IsNullOrEmpty(funcionario.Email))
            {
                return new ResponseMessage { Message = "Email é obrigatório." };
            }

            if (!string.IsNullOrEmpty(funcionario.Cpf))
            {
                funcionario.Cpf = SomenteDigitos(funcionario.Cpf);
                if (funcionario.Cpf.Length != 11)
                {
                    return new ResponseMessage { Message = "CPF deve conter 11 dígitos." };
                }
                    
                if (await _repo.CpfExisteAsync(funcionario.Cpf))
                {
                    return new ResponseMessage { Message = "Já existe um funcionário com este CPF." };
                }
            }

            if (!string.IsNullOrEmpty(funcionario.Telefone))
            {
                funcionario.Telefone = SomenteDigitos(funcionario.Telefone);
            }

            bool existe = await _repo.ExisteCnpjAsync(funcionario.Cnpj);

            if (existe)
            {
                int id = await _repo.BuscarIdLojaPorCnpjAsync(funcionario.Cnpj);

                if (id <= 0)
                {
                    return new ResponseMessage { Message = "CNPJ não encontrado." };
                }
                else 
                {
                    int PerfilId = await _repo.GetPerfilIdByNomeAsync(funcionario.PerfilNome);

                    if (PerfilId <= 0)
                    {
                        return new ResponseMessage { Message = "Perfil não encontrado." };
                    }

                    funcionario.IdPerfil = PerfilId;
                    funcionario.IdLoja = id;

                    await _repo.CriarAsync(funcionario);
                    return new ResponseMessage { Message = "Funcionário cadastrado com sucesso!" };
                }
            }
            else
            {
                return new ResponseMessage { Message = "CNPJ inválido ou loja não encontrada." };
            }
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
            Telefone = f.Telefone?.Trim(),
            Cnpj = f.Cnpj.Trim(),
            PerfilNome = f.PerfilNome.Trim()
        };

        private static string SomenteDigitos(string s) => new(s.Where(char.IsDigit).ToArray());
    }
}
