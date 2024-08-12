import React, { useEffect, useState } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/common.css';
import '../styles/template.css';
import '../styles/navbar.css';
import { Modal } from 'react-bootstrap';
import Loading from './Loading';
import axiosInstance from './api/axiosInstance';
import { useNavigate } from 'react-router-dom';
import { Navbar, Nav, Container, Table, Alert, Button, Card } from 'react-bootstrap';
import { Purpose, Goal, Habit } from '../types';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faChevronDown, faChevronUp, faPlus } from '@fortawesome/free-solid-svg-icons';
import { Link } from 'react-router-dom';
import { BrowserRouter as Router } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Template: React.FC = () => {
  const [purposes, setPurposes] = useState<Purpose[]>([]);
  const [goals, setGoals] = useState<Goal[]>([]);
  const [habits, setHabits] = useState<Record<number, Habit[]>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [expandedGoals, setExpandedGoals] = useState<Record<number, boolean>>({});
  const [currentUserId, setCurrentUserId] = useState<number | null>(null);
  const { userId} = useAuth();
  const navigate = useNavigate();
  const [showErrorModal, setShowErrorModal] = useState(false);
const [errorMessage, setErrorMessage] = useState<string | null>(null);

const handleCloseErrorModal = () => setShowErrorModal(false);


  useEffect(() => {
    const setUserId = (id: string | number | null) => {
      if (id === null) {
        setCurrentUserId(null);
      } else {
        const parsedId = typeof id === 'string' ? parseInt(id, 10) : id;
        setCurrentUserId(isNaN(parsedId) ? null : parsedId);
      }
    };
    if (userId) {
      setUserId(userId);
    } else {
      // If there's no userId in context, try to get it from storage
      const storedUserId = localStorage.getItem('userId') || sessionStorage.getItem('userId');
      if (storedUserId) {
        setUserId(storedUserId);
      } else {
        // If there's no userId in storage either, redirect to login
        navigate('/login');
      }
    }
  }, [userId, navigate]);

  useEffect(() => {
    const fetchData = async () => {
      if(!currentUserId) return;
      try {
        const [purposesResponse, goalsResponse] = await Promise.all([
          axios.get('https://localhost:7073/api/Templates/purposes'),
          axios.get('https://localhost:7073/api/Templates/goals')
        ]);

        setPurposes(purposesResponse.data);
        setGoals(goalsResponse.data);

        const habitsResponses = await Promise.all(
          goalsResponse.data.map((goal: Goal) =>
            axios.get(`https://localhost:7073/api/Templates/goals/${goal.id}/habits`)
          )
        );

        const habitsByGoalId: Record<number, Habit[]> = {};
        habitsResponses.forEach((response, index) => {
          const goalId = goalsResponse.data[index].id;
          habitsByGoalId[goalId] = response.data;
        });

        setHabits(habitsByGoalId); 
      } catch (err) {
        if (axios.isAxiosError(err)) {
          setError(err.message);
        } else {
          setError('An unexpected error occurred');
        }
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [currentUserId]);

  const getHabitsForGoal = (goalId: number) => {
    return habits[goalId] || [];
  };

  const toggleExpandGoal = (goalId: number) => {
    setExpandedGoals(prevState => ({
      ...prevState,
      [goalId]: !prevState[goalId]
    }));
  };

  const addPurposeToHomePage = async (purpose: Purpose) => {
    if (!currentUserId) {
      setErrorMessage('User ID not found. Please log in again.');
      setShowErrorModal(true);
      return;
    }
  
    try {
      console.log('Adding purpose to home page:', purpose.id);
      const response = await axiosInstance.post(`/User/${currentUserId}/add-purpose`, purpose.id);
      console.log('Response:', response);
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error('Axios error:', error.response?.data);
        setErrorMessage(`Failed to add purpose: ${error.response?.data.title || error.message}`);
      } else {
        console.error('Error:', error);
        setErrorMessage('An unexpected error occurred while adding the purpose');
      }
      setShowErrorModal(true);
    }
  };
  

  const addGoalToHomePage = async (goal: Goal) => {
    const goalData = {
      id: goal.id,
      userId: currentUserId,
      title: goal.title,
 
      startDate: new Date().toISOString(), 
      dueDate: new Date().toISOString(),   
      isAchieved: false,
      status: 'Pending'                 
    };

    try {
      console.log('Adding goal to home page:', goalData);
      const response = await axios.post(`https://localhost:7073/api/User/${currentUserId}/add-goal-and-habits`, goalData);
      console.log('Response:', response);
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error('Axios error:', error.response?.data);
      } else {
        console.error('Error:', error);
      }
      setError('Failed to add goal to home page');
    }
  };

  if (loading) return <Loading />;
  if (error) return <Alert variant="danger">Error: {error}</Alert>;
  const handleLogout = () => {
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
   navigate('/login'); // Redirect to login page
  };
  return (
    <div className="template-background">
      <Navbar className="navbar-custom" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand href="/landing">LevelUp</Navbar.Brand>
          <Navbar.Toggle aria-controls="navbarNav" />
          <Navbar.Collapse id="navbarNav">
            <Nav className="ms-auto">
              <Nav.Link href="/landing">Home</Nav.Link>
              <Nav.Link href="/template">Templates</Nav.Link>
              <Nav.Link href="/resources">Resources</Nav.Link>
              <Nav.Link href="/features">Features</Nav.Link>
              <Button variant="link" onClick={handleLogout}>Logout</Button> 
            </Nav>
          </Navbar.Collapse>
          <Nav className="ms-auto">
            <Nav.Link as={Link} to="/setup-profile" className="profile-icon">
              <FontAwesomeIcon icon={faUser} size="lg" />
            </Nav.Link>
          </Nav>
        </Container>
      </Navbar>

      <div className="container mt-4">
        <h2>Welcome to the LevelUp Templates Page</h2>
        <p>Here are samples of some purposes, goals, and habits, which you can add to your personal page and then modify them:</p>
        
        <h3>Purposes</h3>
        <p>You can add the purpose of your journey:</p>
        <div className="purposes-container">
          {purposes.map(purpose => (
            <Card key={purpose.id} className="purpose-card">
              <Card.Header className="d-flex justify-content-between align-items-center">
                <h3>{purpose.title}</h3>
                <Button variant="primary" onClick={() => addPurposeToHomePage(purpose)}>
                  <FontAwesomeIcon icon={faPlus} />
                </Button>
              </Card.Header>
            </Card>
          ))}
        </div>

        <h3>Goals</h3>
        <p>You can add some goals from here:</p>
        {goals.map(goal => (
          <div key={goal.id} className="goal-container">
            <div className="goal-header d-flex justify-content-between align-items-center">
              <h3>{goal.title}</h3>
              <div className="goal-actions">
                <Button variant="link" onClick={() => toggleExpandGoal(goal.id)}>
                  <FontAwesomeIcon 
                    icon={expandedGoals[goal.id] ? faChevronUp : faChevronDown} 
                    className="goal-toggle-icon"
                  />
                </Button>
                <Button variant="primary" onClick={() => addGoalToHomePage(goal)}>
                  <FontAwesomeIcon icon={faPlus} />
                </Button>
              </div>
            </div>
            {expandedGoals[goal.id] && (
              <Table striped bordered hover>
                <thead>
                  <tr>
                    <th>Name</th>
                  </tr>
                </thead>
                <tbody>
                  {getHabitsForGoal(goal.id).map(habit => (
                    <tr key={habit.id}>
                      <td>{habit.title}</td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            )}
          </div>
        ))}
      </div>
      <Modal show={showErrorModal} onHide={handleCloseErrorModal}>
      <Modal.Header closeButton>
        <Modal.Title>Error</Modal.Title>
      </Modal.Header>
      <Modal.Body>{errorMessage}</Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleCloseErrorModal}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
    </div>
  );
};

export default Template;
