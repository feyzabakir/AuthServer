﻿using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
