using FluentValidation.Results;
using GameZone.Core.Data;

namespace GameZone.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AdicionarErro("Houve um erro ao persistir os dados");

            return ValidationResult;
        }
        protected ValidationResult PersistiuDados(bool DeuCerto)
        {
            if (!DeuCerto) AdicionarErro("Houve um erro ao persistir os dados");

            return ValidationResult;
        }
    }
}