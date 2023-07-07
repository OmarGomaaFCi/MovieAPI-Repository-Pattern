﻿using Movies.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Core.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IMoviesRepository Movies { get; }
		IBaseRepository<Genre> Genres { get; }

		ICharactersRepository Characters { get; }

		int Complete();
	}
}
