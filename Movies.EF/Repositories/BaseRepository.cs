﻿using FirstWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Movies.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.EF.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;

		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int?> AddAsync(T entity)
		{
			_context.Set<T>().Add(entity);
			return await _context.SaveChangesAsync();
		}

		public async Task<int?> DeleteByIdAsync(int id)
		{
			var entity = await _context.Set<T>().FindAsync(id);
			if (entity is null)
				return -1;

			_context.Set<T>().Remove(entity);
			return _context.SaveChanges();
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			var entity = await _context.Set<T>().FindAsync(id);
			return entity;
		}

		public T UpdateAsync(T entity)
		{
			_context.Update(entity);
			return entity;
		}
	}
}