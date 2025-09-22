using MicroEmpresa.Entity;
using MicroEmpresa.LogicInterface;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic.Lojas;

public class LojasLogic : ILojasLogic
{
    private readonly ILojasRepository _repo;
    private readonly IEnderecosLogic _enderecosSvc;

    public LojasLogic(ILojasRepository repo, IEnderecosLogic enderecosSvc)
    {
        _repo = repo;
        _enderecosSvc = enderecosSvc;
    }
    //Quando eu teste, so veio a lista da loja, não com as tabelas ligadas, por exemplo,
    //tem a tabela endereço conecta e quando eu chamo o metodo ele nao mostra os endereços. Tratar isso.
    public Task<List<LojasEntity>> ListarAsync() => _repo.ListarAsync();

    public async Task<LojasEntity> ObterLoja(int id)
    {
        LojasEntity lojasEntity = new LojasEntity();
        try
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido.");
            }

            lojasEntity = await _repo.ObterAsync(id);

            if (lojasEntity is null)
            {
                throw new ArgumentException("Loja não encontrada.");
            }
            else
            {
                return lojasEntity;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    public async Task<ResponseMessage> CriarAsync(LojasEntity lojasEntity)
    {
        
        try
        {
            // Sanitização da loja
            var loja = Tratamento(lojasEntity);

            // ===== Validações de Loja =====
            


            if (loja.NomeFantasia.Length > 150)
            {
                return new ResponseMessage { Message = "Nome Fantasia deve ter no máximo 150 caracteres." };
            }


            if (string.IsNullOrWhiteSpace(loja.Cnpj))
            {
                return new ResponseMessage { Message = "CNPJ deve ser preenchido." };
            }


            loja.Cnpj = SomenteDigitos(loja.Cnpj);

            if (loja.Cnpj.Length != 14)
            {
                return new ResponseMessage { Message = "CNPJ deve conter 14 dígitos." };
            }


            if (await _repo.CnpjExisteAsync(loja.Cnpj))
            {
                return new ResponseMessage {  Message = "Já existe uma loja com este CNPJ." };
            }


            if (!string.IsNullOrWhiteSpace(loja.Telefone))
            {
                loja.Telefone = SomenteDigitos(loja.Telefone);
            }

            if (lojasEntity.Enderecos == null || !lojasEntity.Enderecos.Any())
            {
                return new ResponseMessage { Message = "Toda loja precisa de pelo menos um endereço." };
            }

            LojasEntity criada = await _repo.CriarAsync(loja);

            foreach (var end in lojasEntity.Enderecos)
            {
                end.IdLoja = criada.Id;  
                await _enderecosSvc.CriarAsync(end);
            }

            return new ResponseMessage
            {
                Message = "Loja cadastrada com sucesso!",
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }   
    }

    public async Task<ResponseMessage> AtualizarAsync(int id, LojasEntity lojasEntity)
    {
        try
        {
            var existe = await _repo.ObterAsync(id);
            if (existe is null) return new ResponseMessage { Message = "Loja não encontrada." };

            var loja = Tratamento(lojasEntity);

            if (string.IsNullOrWhiteSpace(loja.NomeFantasia))
                return new ResponseMessage { Message = "Nome Fantasia é obrigatório." };

            if (loja.NomeFantasia.Length > 150)
                return new ResponseMessage { Message = "Nome Fantasia deve ter no máximo 150 caracteres." };

            if (!string.IsNullOrWhiteSpace(loja.Cnpj))
            {
                loja.Cnpj = SomenteDigitos(loja.Cnpj);
                if (loja.Cnpj.Length != 14)
                    return new ResponseMessage { Message = "CNPJ deve conter 14 dígitos." };

                if (await _repo.CnpjExisteAsync(loja.Cnpj))
                    return new ResponseMessage { Message = "Este CNPJ já está em uso por outra loja." };
            }

            if (!string.IsNullOrWhiteSpace(loja.Telefone))
                loja.Telefone = SomenteDigitos(loja.Telefone);

            var ok = await _repo.AtualizarAsync(id, loja);
            return new ResponseMessage { Message = ok ? "Loja atualizada com sucesso!" : "Nada foi alterado." };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    public async Task<ResponseMessage> RemoverAsync(int id)
    {
        if (id <= 0) return new ResponseMessage { Message = "ID inválido." };

        if (await _repo.TemDependenciasAsync(id))
            return new ResponseMessage { Message = "Não é possível excluir: há dados vinculados." };

        var ok = await _repo.RemoverAsync(id);
        return new ResponseMessage { Message = ok ? "Loja removida com sucesso!" : "Loja não encontrada." };
    }

    #region Metodos Auxiliares

    private static LojasEntity Tratamento(LojasEntity e) => new()
    {
        NomeFantasia = (e.NomeFantasia ?? string.Empty).Trim(),
        Cnpj = e.Cnpj?.Trim(),
        Telefone = e.Telefone?.Trim()
    };

    private static string SomenteDigitos(string s) => new(s.Where(char.IsDigit).ToArray());

    #endregion Metodos Auxiliares
}
