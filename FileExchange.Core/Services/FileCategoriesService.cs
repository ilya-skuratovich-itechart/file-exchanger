using System.Collections.Generic;
using System.Linq;
using Autofac;
using FileExchange.Core.DAL.Entity;
using FileExchange.Core.DAL.Repository;
using FileExchange.Core.UOW;

namespace FileExchange.Core.Services
{
    public class FileCategoriesService : IFileCategoriesService
    {
        private IGenericRepository<FileCategories> _repository;

        public FileCategoriesService(IUnitOfWork unitOfWork)
        {
            _repository = BootStrap.Container.Resolve<IGenericRepository<FileCategories>>();
            _repository.InitializeDbContext(unitOfWork.DbContext);
        }

        public List<FileCategories> GetAll()
        {
            return _repository.GetAll()
                .ToList();
        }
    }
}