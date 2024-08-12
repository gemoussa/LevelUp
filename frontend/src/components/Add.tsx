import React, { useState } from 'react';
import axios from 'axios';

const Add: React.FC = () => {
  const [purpose, setPurpose] = useState('');
  const [goal, setGoal] = useState('');
  const [habit, setHabit] = useState('');
  const [habits, setHabits] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);

  const handleAddHabit = () => {
    if (habit) {
      setHabits([...habits, habit]);
      setHabit('');
    }
  };

  const handleSubmit = async () => {
    try {
      await axios.post('https://localhost:7073/api/purposes', { purpose });
      const response = await axios.post('https://localhost:7073/api/goals', { goal });
      await axios.post('https://localhost:7073/api/habits', { habits: habits, goalId: response.data.id });
      // Handle success (e.g., redirect or clear form)
    } catch (err) {
      setError('An error occurred while saving data.');
    }
  };

  return (
    <div className="container">
      <h2>Add New Purpose, Goal, and Habits</h2>
      <form>
        <div className="form-group">
          <label htmlFor="purpose">Purpose</label>
          <input
            type="text"
            className="form-control"
            id="purpose"
            value={purpose}
            onChange={(e) => setPurpose(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label htmlFor="goal">Goal</label>
          <input
            type="text"
            className="form-control"
            id="goal"
            value={goal}
            onChange={(e) => setGoal(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label htmlFor="habit">Habit</label>
          <input
            type="text"
            className="form-control"
            id="habit"
            value={habit}
            onChange={(e) => setHabit(e.target.value)}
          />
          <button type="button" className="btn btn-primary mt-2" onClick={handleAddHabit}>
            Add Habit
          </button>
        </div>
        <div className="form-group">
          <label>Habits</label>
          <ul>
            {habits.map((h, index) => (
              <li key={index}>{h}</li>
            ))}
          </ul>
        </div>
        <button type="button" className="btn btn-success" onClick={handleSubmit}>
          Submit
        </button>
        {error && <p className="text-danger">{error}</p>}
      </form>
    </div>
  );
};

export default Add;
