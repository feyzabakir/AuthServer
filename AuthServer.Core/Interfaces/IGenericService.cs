using SharedLibrary.Dtos;
using System.Linq.Expressions;

namespace AuthServer.Core.Interfaces;

public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    Task<ResponseDto<TDto>> GetByIdAsync(int id);

    Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync();

    Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

    Task<ResponseDto<TDto>> AddAsync(TDto entity);

    Task<ResponseDto<NoDataDto>> Remove(int id);

    Task<ResponseDto<NoDataDto>> Update(TDto entity, int id);
}
