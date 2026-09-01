using TradeERP.BLL.IServices.ICommons;
using TradeERP.DAL.IRepositories.ICommons;

namespace TradeERP.BLL.Services.Commons
{
    public class ValidatorService<T> : IValidatorService<T> where T : class
    {
        private readonly IValidatorRepository<T> _repository;

        public ValidatorService(IValidatorRepository<T> repository)
        {
            _repository = repository;
        }

        public bool IsCodeExist(int id, string code) => _repository.IsCodeExist(id, code);

        public bool IsArNameExist(int id, string arName) => _repository.IsArNameExist(id, arName);

        public bool IsEnNameExist(int id, string enName) => _repository.IsEnNameExist(id, enName);
    }
}
