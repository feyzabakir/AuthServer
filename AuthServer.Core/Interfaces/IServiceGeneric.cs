using SharedLibrary.Dtos;
using System.Linq.Expressions;

namespace AuthServer.Core.Interfaces;

public interface IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
{
    Task<ResponseDto<TDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync(CancellationToken cancellationToken);

    Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    Task<ResponseDto<TDto>> AddAsync(TDto entity, CancellationToken cancellationToken);

    Task<ResponseDto<NoDataDto>> Remove(int id, CancellationToken cancellationToken);

    Task<ResponseDto<NoDataDto>> Update(TDto entity, int id, CancellationToken cancellationToken);
}
