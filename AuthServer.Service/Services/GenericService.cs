﻿using AuthServer.Core.Interfaces;
using AuthServer.Core.Repositories;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System.Linq.Expressions;

namespace AuthServer.Service.Services;

public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IGenericRepository<TEntity> _genericRepository;

    public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = genericRepository;
    }

    public async Task<ResponseDto<TDto>> AddAsync(TDto entity)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

        await _genericRepository.AddAsync(newEntity);

        await _unitOfWork.CommitAsync();

        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

        return ResponseDto<TDto>.Success(newDto, 200);
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync()
    {
        var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());

        return ResponseDto<IEnumerable<TDto>>.Success(products, 200);
    }

    public async Task<ResponseDto<TDto>> GetByIdAsync(int id)
    {
        var product = await _genericRepository.GetByIdAsync(id);

        if (product == null)
        {
            return ResponseDto<TDto>.Fail("Id not found", 404, true);
        }

        return ResponseDto<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(product), 200);
    }

    public async Task<ResponseDto<NoDataDto>> Remove(int id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);

        if (isExistEntity == null)
        {
            return ResponseDto<NoDataDto>.Fail("Id not found", 404, true);
        }

        _genericRepository.Remove(isExistEntity);

        await _unitOfWork.CommitAsync();

        return ResponseDto<NoDataDto>.Success(204);
    }

    public async Task<ResponseDto<NoDataDto>> Update(TDto entity, int id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);

        if (isExistEntity == null)
        {
            return ResponseDto<NoDataDto>.Fail("Id not found", 404, true);
        }

        var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

        _genericRepository.Update(updateEntity);

        await _unitOfWork.CommitAsync();

        return ResponseDto<NoDataDto>.Success(204);
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        var list = _genericRepository.Where(predicate);

        return ResponseDto<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
    }
}
