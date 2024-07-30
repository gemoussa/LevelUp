using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelUp.Core;
using LevelUp.Infrastructure;

namespace LevelUp.Application.LevelUp.Services
{
    public interface IProgressService
    {
        Task<Progress> GetProgressByIdAsync(int id);
        Task<IEnumerable<Progress>> GetAllProgressAsync();
        Task<int> CreateProgressAsync(Progress progress);
        Task<bool> UpdateProgressAsync(Progress progress);
   
    }
    public class ProgressService:IProgressService
    {
        private readonly IProgressRepository _progressRepository;

        public ProgressService(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        public async Task<Progress> GetProgressByIdAsync(int id)
        {
            return await _progressRepository.GetProgressByIdAsync(id);
        }
        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            return await _progressRepository.GetAllProgressAsync();
        }

        public async Task<int> CreateProgressAsync(Progress progress)
        {
            return await _progressRepository.CreateProgressAsync(progress);
        }

        public async Task<bool> UpdateProgressAsync(Progress progress)
        {
            return await _progressRepository.UpdateProgressAsync(progress);
        }

      
    }
}
