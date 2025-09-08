using MicroEmpresa.Entity;
using MicroEmpresa.Repository;

namespace MicroEmpresa.Logic.Lojas;

public class LojasLogic : ILojasService
{
    private readonly ILojasRepository _repo;
    public LojasLogic(ILojasRepository repo) => _repo = repo;

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

    public async Task<LojasEntity> CriarAsync(LojasEntity lojasEntity)
    {
        // Sanitização da loja
        var loja = Tratamento(lojasEntity);

        // ===== Validações de Loja =====
        if (string.IsNullOrWhiteSpace(loja.NomeFantasia))
            throw new ArgumentException("Nome Fantasia é obrigatório.");

        if (loja.NomeFantasia.Length > 150)
            throw new ArgumentException("Nome Fantasia deve ter no máximo 150 caracteres.");

        if (string.IsNullOrWhiteSpace(loja.Cnpj))
            throw new ArgumentException("CNPJ deve ser preenchido.");

        loja.Cnpj = SomenteDigitos(loja.Cnpj);
        if (loja.Cnpj.Length != 14)
            throw new ArgumentException("CNPJ deve conter 14 dígitos.");

        if (await _repo.CnpjExisteAsync(loja.Cnpj))
            throw new ArgumentException("Já existe uma loja com este CNPJ.");

        if (!string.IsNullOrWhiteSpace(loja.Telefone))
            loja.Telefone = SomenteDigitos(loja.Telefone);

        // ===== Validação de Endereço =====
        if (lojasEntity.Enderecos == null || !lojasEntity.Enderecos.Any())
        {
            throw new ArgumentException("Toda loja precisa de pelo menos um endereço.");
        }
            

        foreach (EnderecosEntity end in lojasEntity.Enderecos)
        {
            // sanitização
            end.Logradouro = end.Logradouro?.Trim() ?? string.Empty;
            end.Cidade = end.Cidade?.Trim() ?? string.Empty;
            end.Uf = end.Uf?.Trim().ToUpperInvariant() ?? string.Empty;
            end.Cep = !string.IsNullOrWhiteSpace(end.Cep)
                               ? SomenteDigitos(end.Cep)
                               : null;

            if (string.IsNullOrWhiteSpace(end.Logradouro))
                throw new ArgumentException("Logradouro do endereço é obrigatório.");
            if (string.IsNullOrWhiteSpace(end.Cidade))
                throw new ArgumentException("Cidade do endereço é obrigatória.");
            if (end.Uf.Length != 2)
                throw new ArgumentException("UF deve conter 2 letras.");
            if (!string.IsNullOrWhiteSpace(end.Cep) && end.Cep.Length != 8)
                throw new ArgumentException("CEP deve conter 8 dígitos.");

            // amarra o endereço à loja
            end.Loja = loja;
        }

        // Persiste a loja + endereço(s)
        return await _repo.CriarAsync(loja);
    }


    public async Task<bool> AtualizarAsync(int id, LojasEntity lojasEntity)
    {
        try
        {
            await _repo.ObterAsync(id);

            if (await _repo.ObterAsync(id) is null)
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

            bool teste = await _repo.AtualizarAsync(id, loja);

            return teste;
        }
        catch (Exception)
        {

            throw;
        }
        
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
