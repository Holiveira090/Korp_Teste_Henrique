using AutoMapper;
using Billing.Application.Services.Interfaces;
using Billing.Domain.Interfaces;

namespace Billing.Application.Services
{
    public abstract class ServiceBase<TDto, TEntity> : IServiceBase<TDto> where TEntity : class
    {
        protected readonly IRepositoryBase<TEntity> _repository;
        protected readonly IMapper _mapper;

        protected ServiceBase(IRepositoryBase<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual async Task<TDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto?>(entity);
        }

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(created);
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(updated);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
