using AutoMapper;
using Stock.Application.Services.Interfaces;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services
{
    public abstract class ServicesBase<TDomain, TDto> : IServicesBase<TDto>
        where TDomain : class
        where TDto : class
    {
        protected readonly IRepositoryBase<TDomain> _repository;
        protected readonly IMapper _mapper;

        public ServicesBase(IRepositoryBase<TDomain> repository, IMapper mapper)
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

        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TDomain>(dto);
            var added = await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(added);
        }

        public virtual async Task<TDto> UpdateAsync(int id, TDto dto)
        {
            var entity = _mapper.Map<TDomain>(dto);

            var idProp = typeof(TDomain).GetProperty("Id");
            if (idProp != null)
            {
                idProp.SetValue(entity, id);
            }

            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(updated);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
