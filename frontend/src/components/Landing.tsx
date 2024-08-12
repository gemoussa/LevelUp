import React, { useEffect, useState } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/common.css';
import '../styles/landing.css';
import '../styles/navbar.css';

import { Navbar, Nav, Container, Card, Button, Modal, Form, ListGroup } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen, faTrash, faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { Link, useNavigate } from 'react-router-dom';
import { Purpose, Goal, Habit } from '../types';
import { useAuth } from '../context/AuthContext';
import axiosInstance from './api/axiosInstance';

const Landing: React.FC = () => {
  const { userId } = useAuth();
  const navigate = useNavigate();
  const [purpose, setPurpose] = useState<Purpose | null>(null);
  const [goals, setGoals] = useState<Goal[]>([]);
  const [habits, setHabits] = useState<{ [goalId: number]: Habit[] }>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showPurposeModal, setShowPurposeModal] = useState(false);
  const [showGoalModal, setShowGoalModal] = useState(false);
  const [editingPurpose, setEditingPurpose] = useState<Purpose | null>(null);
  const [editingGoal, setEditingGoal] = useState<Goal | null>(null);
  const [expandedGoalId, setExpandedGoalId] = useState<number | null>(null);

  const handleEditPurpose = () => {
    setEditingPurpose(purpose);
    setShowPurposeModal(true);
  };

  const handleDeletePurpose = async () => {   //done
    if (window.confirm('Are you sure you want to delete your purpose?')) {
      try {
        await axiosInstance.delete(`/purpose/user/${userId}/purpose`);
        setPurpose(null);
      } catch (error) {
        console.error('Error deleting purpose:', error);
      }
    }
  };

  const handleSavePurpose = async () => {
    try {
      const response = await axiosInstance.put(`/purpose/user/${userId}/purpose`, editingPurpose);
      const updatePurpose = await axiosInstance.get(`/purpose/user/${userId}/purpose`);
      setPurpose(updatePurpose.data);
      setShowPurposeModal(false);
    } catch (error) {
      console.error('Error updating purpose:', error);
    }
  };

  const handleEditGoal = (goal: Goal) => {
    setEditingGoal(goal);
    setShowGoalModal(true);
  };

  const handleSaveGoal = async () => {
    try {
      const response = await axiosInstance.put(`/Goal/${editingGoal?.id}`, editingGoal);
      setGoals(goals.map(g => g.id === editingGoal?.id ? response.data : g));
      setShowGoalModal(false);
    } catch (error) {
      console.error('Error updating goal:', error);
    }
  };

  const handleDeleteGoal = async (goalId: number) => {
    if (window.confirm('Are you sure you want to delete this goal?')) {
      try {
        await axiosInstance.delete(`/Goal/${goalId}`);
        setGoals(goals.filter(g => g.id !== goalId));
        setHabits(prev => {
          const newHabits = { ...prev };
          delete newHabits[goalId];
          return newHabits;
        });
      } catch (error) {
        console.error('Error deleting goal:', error);
      }
    }
  };

  const fetchHabitsForGoal = async (goalId: number) => {
    try {
      const response = await axiosInstance.get(`/Habit/goal/${goalId}`);
      setHabits(prev => ({ ...prev, [goalId]: response.data }));
    } catch (error) {
      console.error('Error fetching habits:', error);
    }
  };

  const handleToggle = (goalId: number) => {
    if (expandedGoalId === goalId) {
      // If the clicked goal is already expanded, collapse it
      setExpandedGoalId(null);
    } else {
      // Expand the clicked goal
      setExpandedGoalId(goalId);
      // Fetch habits for the goal if not already fetched
      if (!habits[goalId]) {
        fetchHabitsForGoal(goalId);
      }
    }
  };

  useEffect(() => {
    let isMounted = true;

    if (userId === null) {
      navigate('/login');
      return;
    }

    const fetchData = async () => {
      try {
        const [purposesResponse, goalsResponse] = await Promise.all([
          axiosInstance.get(`/purpose/user/${userId}/purpose`),
          axiosInstance.get(`/Goal/user/${userId}`)
        ]);
        if (isMounted) {
          setPurpose(purposesResponse.data);
          setGoals(goalsResponse.data);
        }
      } catch (err) {
        if (isMounted) {
          if (axios.isAxiosError(err)) {
            if (err.response?.status === 404) {
              console.log('404 error received, setting empty arrays');
              setPurpose(null);
              setGoals([]);
            } else {
              setError(err.message);
            }
          } else {
            setError('An unexpected error occurred');
          }
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    fetchData();

    return () => {
      isMounted = false;
    };
  }, [userId, navigate]);

  const handleLogout = () => {
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
    navigate('/login'); 
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }

  return (
    <div className="landing-background">
      <Navbar className="navbar-custom" variant="dark" expand="lg">
        <Container>
          <Navbar.Brand as={Link} to="/landing">LevelUp</Navbar.Brand>
          <Navbar.Toggle aria-controls="navbarNav" />
          <Navbar.Collapse id="navbarNav">
            <Nav className="ms-auto">
              <Nav.Link as={Link} to="/landing">Home</Nav.Link>
              <Nav.Link as={Link} to="/template">Templates</Nav.Link>
              <Nav.Link as={Link} to="/resources">Resources</Nav.Link>
              <Nav.Link as={Link} to="/features">Features</Nav.Link>
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

      <main className="container mt-4">
        <h2>Your Purpose</h2>
        <div className="purpose-container">
          {purpose ? (
            <Card className="purpose-card">
              <Card.Header>
                <div className="d-flex justify-content-between align-items-center">
                  <h3>{purpose.title}</h3>
                  <div className="card-actions">
                    <FontAwesomeIcon icon={faPen} onClick={handleEditPurpose} className="edit-icon" />
                    <FontAwesomeIcon icon={faTrash} onClick={handleDeletePurpose} className="delete-icon" />
                  </div>
                </div>
              </Card.Header>
            </Card>
          ) : (
            <p>No purpose found. Why not add one?</p>
          )}
        </div>

        <h2>Your Goals</h2>
        <div className="goals-container">
          {goals.length > 0 ? (
            goals.map(goal => (
              <Card key={goal.id} className="goal-card">
                <Card.Header>
                  <div className="d-flex justify-content-between align-items-center">
                    <h3>{goal.title}</h3>
                    <div className="card-actions">
                      <FontAwesomeIcon icon={faPen} onClick={() => handleEditGoal(goal)} className="edit-icon" />
                      <FontAwesomeIcon icon={faTrash} onClick={() => handleDeleteGoal(goal.id)} className="delete-icon" />
                    </div>
                    <Button variant="link" onClick={() => handleToggle(goal.id)}>
                    <FontAwesomeIcon icon={expandedGoalId===goal.id ?faChevronUp:faChevronDown} className="toggle-icon" />
                    </Button>
                  </div>
                </Card.Header>
                {expandedGoalId === goal.id && (
                  <Card.Body>
                    <ListGroup>
                      {habits[goal.id]?.length ? (
                        habits[goal.id].map(habit => (
                          <ListGroup.Item key={habit.id}>{habit.title}</ListGroup.Item>
                        ))
                      ) : (
                        <p>No habits found for this goal.</p>
                      )}
                    </ListGroup>
                  </Card.Body>
                )}
              </Card>
            ))
          ) : (
            <p>No goals found. Add some goals to get started!</p>
          )}
        </div>
      </main>

      {/* Modals for Editing Purpose and Goal */}
      <Modal show={showPurposeModal} onHide={() => setShowPurposeModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Edit Purpose</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group className="mb-3">
              <Form.Label>Title</Form.Label>
              <Form.Control
                type="text"
                value={editingPurpose?.title || ''}
                onChange={e => setEditingPurpose(prev => ({ ...prev!, title: e.target.value }))}
              />
            </Form.Group>
            <Button variant="primary" onClick={handleSavePurpose}>Save</Button>
          </Form>
        </Modal.Body>
      </Modal>

      <Modal show={showGoalModal} onHide={() => setShowGoalModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Edit Goal</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group className="mb-3">
              <Form.Label>Title</Form.Label>
              <Form.Control
                type="text"
                value={editingGoal?.title || ''}
                onChange={e => setEditingGoal(prev => ({ ...prev!, title: e.target.value }))}
              />
            </Form.Group>
            <Button variant="primary" onClick={handleSaveGoal}>Save</Button>
          </Form>
        </Modal.Body>
      </Modal>
    </div>
  );
};

export default Landing;
