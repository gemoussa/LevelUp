import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../styles/common.css';
import '../styles/features.css';
import '../styles/navbar.css';
import { Navbar, Nav, Container, Row, Col } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { Link } from 'react-router-dom';

const Features: React.FC = () => {
  
  return (
    <div className="features-background">
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
              <Nav.Link href="/login">Logout</Nav.Link>
            </Nav>
          </Navbar.Collapse>
          <Nav className="ms-auto">
            <Nav.Link as={Link} to="/profile-setup" className="profile-icon">
              <FontAwesomeIcon icon={faUser} size="lg" />
            </Nav.Link>
          </Nav>
        </Container>
      </Navbar>

      <Container className="text-center my-5">
      <Row className="mb-4">
          <Col>
            <a href="/landing" className="back-arrow">
              <FontAwesomeIcon icon={faArrowLeft} /> Back to Home
            </a>
          </Col>
        
          <Col className="text-center">
            <h1>Our Features</h1>
            <p>Discover the tools that will help you on your journey of self-development.</p>
          </Col>
        </Row>
        <Row>
          <Col md={4} className="feature-item">
            <h3>Goal Setting</h3>
            <p>Set and manage your goals using the SMART framework. Define your purpose and outline necessary habits to achieve your objectives.</p>
          </Col>
          <Col md={4} className="feature-item">
            <h3>Task Management</h3>
            <p>Organize your daily tasks with ease. Our simple interface allows you to mark tasks as completed and track your productivity.</p>
          </Col>
          <Col md={4} className="feature-item">
            <h3>Progress Tracking</h3>
            <p>Monitor your achievements and areas needing improvement using visual tools like graphs and charts. See your growth over time.</p>
          </Col>
        </Row>
        <Row className="my-4">
          <Col md={4} className="feature-item">
            <h3>Resource Center</h3>
            <p>Access articles, motivational resources, and lifestyle adjustment tips to stay informed and inspired. Enhance your personal development journey.</p>
          </Col>
          <Col md={4} className="feature-item">
            <h3>Health Reminders</h3>
            <p>Receive reminders to maintain good health while working. Get alerts for posture adjustments, hydration, and stretch breaks.</p>
          </Col>
          <Col md={4} className="feature-item">
            <h3>Social Sharing</h3>
            <p>Share your progress, milestones, and achievements on social media or with friends directly from the app. Celebrate your successes and inspire others.</p>
          </Col>
        </Row>
      </Container>
    </div>
  );
};

export default Features;
