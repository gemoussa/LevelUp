using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using LevelUp.Core;

namespace LevelUp.Infrastructure
{
    public interface ISampleRepository
    {
        Task<IEnumerable<SampleGoal>> GetSampleGoalsAsync();
        Task<IEnumerable<SampleHabit>> GetSampleHabitsAsync();
        Task<IEnumerable<SamplePurpose>> GetSamplePurposesAsync();
        Task<IEnumerable<Habit>> GetHabitsByGoalIdAsync(int goalId);
        Task<SamplePurpose> GetSamplePurposeByIdAsync(int samplePurposeId);
        Task<SampleGoal> GetSampleGoalByIdAsync(int sampleGoalId);
        Task<IEnumerable<Habit>> GetHabitsBySampleGoalIdAsync(int sampleGoalId);
        Task AddSampleGoalAsync(SampleGoal goal);
        Task AddSampleHabitAsync(SampleHabit habit);
        Task AddSamplePurposeAsync(SamplePurpose purpose);
    }

    public class SampleRepository : ISampleRepository
    {
        private readonly IDbConnection _dbConnection;

        public SampleRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<SampleGoal>> GetSampleGoalsAsync()
        {
            //var q2 = "select purposeid from userpurpose where userid=25";
            //var list = await _dbConnection.QueryAsync<long>(q2);
            //string ids = "";
            //for(int i = 0; i < list.Count(); i++)
            //{
            //    ids += list.ElementAt(i) + "";
            //    if(i!=list.Count()-1)ids+= ",";
            //}

            var query = "SELECT * FROM SampleGoal";// +(ids!=""?" where id not in("+ids+")":"");
            return await _dbConnection.QueryAsync<SampleGoal>(query);
        }

        public async Task<IEnumerable<SampleHabit>> GetSampleHabitsAsync()
        {
            var query = "SELECT * FROM SampleHabit";
            return await _dbConnection.QueryAsync<SampleHabit>(query);
        }

        public async Task<IEnumerable<SamplePurpose>> GetSamplePurposesAsync()
        {
            var query = "SELECT * FROM SamplePurpose";
            return await _dbConnection.QueryAsync<SamplePurpose>(query);
        }
        public async Task<IEnumerable<Habit>> GetHabitsByGoalIdAsync(int goalId)
        {
            var query = "SELECT * FROM SampleHabit WHERE GoalId = @GoalId";
            return await _dbConnection.QueryAsync<Habit>(query, new { GoalId = goalId });
        }
        public async Task<SampleGoal> GetSampleGoalByIdAsync(int sampleGoalId)
        {
            var query = @"
            SELECT Id, Title, DueDate
            FROM SampleGoal
            WHERE Id = @Id";

            return await _dbConnection.QueryFirstOrDefaultAsync<SampleGoal>(query, new { Id = sampleGoalId });
        }
        //add samplw purpose to homepage
      
        public async Task<SamplePurpose> GetSamplePurposeByIdAsync(int samplePurposeId)
        {
            var query = "SELECT * FROM SamplePurpose WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<SamplePurpose>(query, new { Id = samplePurposeId });
        }
        public async Task<IEnumerable<Habit>> GetHabitsBySampleGoalIdAsync(int sampleGoalId)
        {
            var query = "SELECT * FROM SampleHabit WHERE GoalId = @GoalId";
            return await _dbConnection.QueryAsync<Habit>(query, new { SampleGoalId = sampleGoalId });
        }
        public async Task AddSampleGoalAsync(SampleGoal goal)
        {
            var query = "INSERT INTO SampleGoal (Title, StartDate, DueDate) VALUES (@Title, @StartDate, @DueDate)";
            await _dbConnection.ExecuteAsync(query, goal);
        }

        public async Task AddSampleHabitAsync(SampleHabit habit)
        {
            var query = "INSERT INTO SampleHabit (Title, Frequency, GoalId) VALUES (@Title, @Frequency, @GoalId)";
            await _dbConnection.ExecuteAsync(query, habit);
        }

        public async Task AddSamplePurposeAsync(SamplePurpose purpose)
        {
            var query = "INSERT INTO SamplePurpose (Title) VALUES (@Title)";
            await _dbConnection.ExecuteAsync(query, purpose);
        }
    }

}
