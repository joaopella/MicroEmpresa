using MicroEmpresa.Entity;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic.Lojas;

public class LojasLogic : ILojasService
{
    private readonly ILojasRepository _repo;
    public LojasLogic(ILojasRepository repo) => _repo = repo;

    public Task<List<LojasEntity>> ListarAsync() => _repo.ListarAsync();

    public Task<LojasEntity?> ObterAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID inválido.");
        }
            
        return _repo.ObterAsync(id);
    }

    public async Task<LojasEntity> CriarAsync(LojasEntity lojasEntity)
    {
        try
        {
            LojasEntity loja = Tratamento(lojasEntity);

            if (string.IsNullOrWhiteSpace(loja.NomeFantasia))
            {
                throw new ArgumentException("Nome Fantasia é obrigatório.");
            }

            if (loja.NomeFantasia.Length > 150)
            {
                throw new ArgumentException("Nome Fantasia deve ter no máximo 150 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(loja.Cnpj))
            {
                loja.Cnpj = SomenteDigitos(loja.Cnpj);
                if (loja.Cnpj!.Length != 14)
                {
                    throw new ArgumentException("CNPJ deve conter 14 dígitos.");
                }
                    
                if (await _repo.CnpjExisteAsync(loja.Cnpj))
                {
                    throw new ArgumentException("Já existe uma loja com este CNPJ.");
                }

            }
            else
            {
                throw new ArgumentException("CNPJ deve ser preenchido.");
            }

            if (!string.IsNullOrWhiteSpace(loja.Telefone))
            {
                loja.Telefone = SomenteDigitos(loja.Telefone!);
            }
                

            return await _repo.CriarAsync(loja);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    public async Task<LojasEntity> AtualizarAsync(int id, LojasEntity lojasEntity)
    {
        await _repo.ObterAsync(id);
        if(await _repo.ObterAsync(id) is null)
        {
            throw new KeyNotFoundException("Loja não encontrada.");
        }

        LojasEntity loja = Tratamento(lojasEntity);

        if (string.IsNullOrWhiteSpace(loja.NomeFantasia))
        {
            throw new ArgumentException("Nome Fantasia é obrigatório.");
        }
            
        if (loja.NomeFantasia.Length > 150)
        {
            throw new ArgumentException("Nome Fantasia deve ter no máximo 150 caracteres.");
        }
            

        if (!string.IsNullOrWhiteSpace(loja.Cnpj))
        {
            loja.Cnpj = SomenteDigitos(loja.Cnpj);
            if (loja.Cnpj!.Length != 14)
            {
                throw new ArgumentException("CNPJ deve conter 14 dígitos.");
            }
                
            if (await _repo.CnpjExisteAsync(loja.Cnpj))
            {
                throw new ArgumentException("Este CNPJ já está em uso por outra loja.");
            }
        }

        if (!string.IsNullOrWhiteSpace(loja.Telefone))
        {
            loja.Telefone = SomenteDigitos(loja.Telefone!);
        }
            
        return await _repo.AtualizarAsync(id, loja);
    }

   public async Task<bool> RemoverAsync(int id)
{
    // opção B — pré-checagem com mensagem de negócio
    if (await _repo.TemDependenciasAsync(id))
        throw new InvalidOperationException("Não é possível excluir: há dados vinculados.");

    var ok = await _repo.RemoverAsync(id);
    if (!ok) throw new KeyNotFoundException("Loja não encontrada.");
    return true;
}


    // helpers
    private static LojasEntity Tratamento(LojasEntity e) => new()
    {
        NomeFantasia = (e.NomeFantasia ?? string.Empty).Trim(),
        Cnpj = e.Cnpj?.Trim(),
        Telefone = e.Telefone?.Trim()
    };

    private static string SomenteDigitos(string s) => new(s.Where(char.IsDigit).ToArray());
}
